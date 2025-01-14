using System;
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
            spriteRenderer.color = value ? hostileColor : friendlyColor;
        }
    }
    public Transform Source { get; set; }
    public Effect Effect { get; set; }
    public float EffectDuration { get; set; }
    public bool HasEffect { get; set; }

    [SerializeField] private Transform col;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color hostileColor;
    [SerializeField] private Color friendlyColor;
    
    private float _size;
    private bool _hostile;
    
    public void Initialize(BulletConfig config, Vector2 position, Vector2 direction, bool hostile, Transform source)
    {
        Speed = config.speed;
        Damage = config.damage;
        Size = config.size;
        LifeLeft = config.lifeTime;
        IsHostile = hostile;
        Source = source;
        Effect = config.effect;
        EffectDuration = config.effectDuration;
        HasEffect = config.hasEffect;
        
        transform.position = position;
        Direction = direction.normalized;
    }
    
    private void OnSizeChanged()
    {
        col.localScale = new Vector3(Size, Size, 1);
    }

    private void Update()
    {
        LifeLeft -= Time.deltaTime;
        if (LifeLeft <= 0)
        {
            BulletFactory.Instance.DestroyBullet(this);
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(Speed * Time.fixedDeltaTime * Direction);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Deflect"))
        {
            var deflectCollider = other.gameObject.GetComponent<DeflectCollider>();
            var normal = deflectCollider.normal;
            
            //if (Vector2.Dot(Direction, normal) > 0) return;

            if (deflectCollider.returnToSource && Source != null)
            {
                var direction = (Source.position - transform.position).normalized;
                // randomize direction a bit
                var randomAngle = UnityEngine.Random.Range(-DataManager.Instance.shieldDeflectAngle, DataManager.Instance.shieldDeflectAngle);
                direction = Quaternion.Euler(0, 0, randomAngle) * direction;
                Direction = direction;
            }
            else
            {
                Direction = Vector2.Reflect(Direction, normal);
            }

            if (deflectCollider.isPlayer)
            {
                IsHostile = false;
            }
        }
        
        if (other.CompareTag("Damageable"))
        {
            var damageable = other.GetComponentInParent<Damageable>();
            bool hit = damageable.TakeDamage(Damage, _hostile);
            if (hit)
            {
                if (HasEffect)
                    damageable.ApplyEffect(Effect, EffectDuration);
                BulletFactory.Instance.DestroyBullet(this);
            }
        }
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && gameObject.activeInHierarchy)
        {
            BulletFactory.Instance.DestroyBullet(this);
        }
    }
}