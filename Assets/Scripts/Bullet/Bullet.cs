﻿using System;
using System.Collections;
using DG.Tweening;
using TorcheyeUtility;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed { get; private set; }
    public Vector2 Direction { get; private set; }
    public int Damage { get; private set; }
    public float Size
    {
        get => _size;
        set
        {
            _size = value;
            OnSizeChanged();
        }
    }
    public float LifeLeft { get; set; }
    public bool IsHostile
    {
        get => _hostile;
        set
        {
            _hostile = value;
        }
    }
    public Transform Source { get; set; }
    public Effect Effect { get; set; }
    public float EffectDuration { get; set; }
    public bool HasEffect { get; set; }
    public bool Penetrating { get; set; }
    public float ChargeSpawnTime { get; set; }

    [SerializeField] private Transform col;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float shieldDeflectAngle = 15;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float spawnNoCollisionTime = 0.15f;
    [SerializeField] private Sprite friendlySprite;
    [SerializeField] private float acidSpawnRange;
    
    private float _size;
    private bool _hostile;
    private bool _doMove;
    private bool _doCollide;
    private Transform _dynamicTarget;
    private float _spawnNoCollisionTimer;
    private bool _spawnAcidPool;
    private float _spiralAngle;
    private float _spiralRadius;
    private float _spiralSpeed; // Angle increment speed
    private Collider2D[] _groundInRange = new Collider2D[4];
    private ContactFilter2D _contactFilter = new ContactFilter2D();
    
    public void Initialize(BulletConfig config, Vector2 position, Vector2 direction, bool hostile, Transform source, Transform dynamicTarget = null)
    {
        Speed = config.speed;
        Damage = config.damage;
        Size = config.size;
        LifeLeft = config.lifeTime;
        IsHostile = hostile;
        Source = source;
        spriteRenderer.sprite = config.sprite;
        Effect = config.effect;
        EffectDuration = config.effectDuration;
        HasEffect = config.hasEffect;
        Penetrating = config.penetrating;
        ChargeSpawnTime = config.chargeSpawnTime;
        _spawnAcidPool = config.spawnAcidPool;
        _spiralSpeed = config.spiralSpeed;
        // direction to spiral angle
        _spiralAngle = Mathf.Atan2(direction.y, direction.x);
        _spiralRadius = 0;
        
        rb.gravityScale = config.gravity;
        if (rb.gravityScale != 0)
        {
            rb.AddForce(direction * Speed, ForceMode2D.Impulse);
        }
        
        transform.position = position;
        Direction = direction.normalized;
        _dynamicTarget = dynamicTarget;
        _doMove = true;
        _doCollide = true;
        
        if (ChargeSpawnTime > 0)
        {
            StartCoroutine(ChargeSpawn());
        }
        else
        {
            _spawnNoCollisionTimer = spawnNoCollisionTime;
        }
    }

    private IEnumerator ChargeSpawn()
    {
        var size = _size;
        _size = 0;
        _doMove = false;
        _doCollide = false;
        
        DOTween.To(() => Size, x => Size = x, size, ChargeSpawnTime);
        yield return new WaitForSeconds(ChargeSpawnTime);
        _doMove = true;
        _doCollide = true;
        _spawnNoCollisionTimer = spawnNoCollisionTime;
        
        Direction = (_dynamicTarget.position - transform.position).normalized;
    }
    
    private void OnSizeChanged()
    {
        col.localScale = new Vector3(Size, Size, 1);
    }

    private void Awake()
    {
        _contactFilter = new ContactFilter2D
        {
            layerMask = LayerMask.GetMask("Ground"), 
            useLayerMask = true
        };
    }

    private void Update()
    {
        LifeLeft -= Time.deltaTime;
        if (LifeLeft <= 0)
        {
            BulletFactory.Instance.DestroyBullet(this);
        }
        
        if (_spawnNoCollisionTimer > 0)
        {
            _spawnNoCollisionTimer -= Time.deltaTime;
        }
    }
    
    private void FixedUpdate()
    {
        if (_doMove && rb.gravityScale == 0)
        {
            if (_spiralSpeed > 0)
            {
                // Update the spiral's angle and radius
                _spiralAngle += _spiralSpeed * Time.fixedDeltaTime; // Increment the angle
                _spiralRadius += Speed * Time.fixedDeltaTime; // Increment the radius
            
                if (_spiralRadius > 0)
                {
                    _spiralAngle += (_spiralSpeed / _spiralRadius) * Time.fixedDeltaTime;
                }

                // Convert polar coordinates to Cartesian coordinates
                float x = _spiralRadius * Mathf.Cos(_spiralAngle);
                float y = _spiralRadius * Mathf.Sin(_spiralAngle);

                // Offset the spiral position by the initial direction
                Vector2 offset = new Vector2(x, y);
                transform.position += (Vector3)offset * Time.fixedDeltaTime;
            }
            else
            {
                var translation = Speed * Time.fixedDeltaTime * Direction;
                transform.Translate(translation);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!_doCollide)
            return;
        
        if (other.CompareTag("Damageable"))
        {
            var damageable = other.GetComponentInParent<Damageable>();
            bool hit = damageable.TakeDamage(Damage, _hostile);
            if (hit)
            {
                if (HasEffect)
                    damageable.ApplyEffect(Effect, EffectDuration);
                BulletFactory.Instance.DestroyBullet(this);
                _doCollide = false;
                
                AudioManager.Instance.PlaySoundEffect(AudioManager.SoundEffect.BulletHitFlesh);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_doCollide)
            return;
        
        if (other.CompareTag("Deflect"))
        {
            if (Penetrating || _spawnNoCollisionTimer > 0) 
                return;
            
            var shield = other.attachedRigidbody.GetComponent<Shield>();
            
            if (shield.IsHostile == IsHostile || !shield.Deflect())
                return;

            if (Source != null)
            {
                var direction = (Source.position - transform.position).normalized;
                // randomize direction a bit
                var randomAngle = UnityEngine.Random.Range(-shieldDeflectAngle, shieldDeflectAngle);
                direction = Quaternion.Euler(0, 0, randomAngle) * direction;

                if (!shield.IsHostile)
                {
                    BulletFactory.Instance.SpawnBullet(DataManager.Instance.deflectBullet, transform.position, direction, !IsHostile, DataManager.Instance.playerTransform);
                    if (shield.Level == 3)
                    {
                        direction = Quaternion.Euler(0, 0, randomAngle) * direction;
                        BulletFactory.Instance.SpawnBullet(DataManager.Instance.deflectBullet, transform.position, direction, !IsHostile, DataManager.Instance.playerTransform);
                    }
                }
                else
                {
                    BulletFactory.Instance.SpawnBullet(DataManager.Instance.normalBullet, transform.position, direction, IsHostile, Source);
                }
                
                AudioManager.Instance.PlaySoundEffect(AudioManager.SoundEffect.ShieldDeflect);
            }
            else
            {
                Debug.LogError("Bullet source is null");
            }
            
            BulletFactory.Instance.DestroyBullet(this);
            _doCollide = false;
        }

        if (other.CompareTag("Potion"))
        {
            if (Penetrating || _spawnNoCollisionTimer > 0) 
                return;
            
            var potion = other.GetComponent<Potion>();
            
            if (potion.IsHostile == IsHostile)
                return;
            
            potion.Charge();
            BulletFactory.Instance.DestroyBullet(this);
            _doCollide = false;
        }
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (Penetrating || _spawnNoCollisionTimer > 0) 
                return;
            
            BulletFactory.Instance.DestroyBullet(this);
            _doCollide = false;

            var hasGroundBlock = other.transform.parent.TryGetComponent<GroundBlock>(out var groundBlock);
            if (!hasGroundBlock)
                return;

            if (!_spawnAcidPool)
            {
                groundBlock.TakeHit();
            }
            else
            {
                Physics2D.OverlapCircle(transform.position, Size, _contactFilter, _groundInRange);
            
                foreach (var g in _groundInRange)
                {
                    if (g != null
                        && g.transform.parent.TryGetComponent<GroundBlock>(out groundBlock)
                        && transform.position.y > groundBlock.transform.position.y)
                    {
                        groundBlock.AttachAcidPool(DataManager.Instance.acidPoolDuration);
                    }
                }
            }
        }
    }
}