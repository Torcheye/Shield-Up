using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed { get; private set; }
    public Vector2 Direction { get; private set; }
    public float Damage { get; private set; }

    public float Size
    {
        get => _size;
        set
        {
            _size = value;
            OnSizeChanged();
        }
    }
    
    public int BounceLeft { get; private set; }
    
    public float LifeLeft { get; set; }

    public Transform col;
    
    private float _size;
    
    public void Initialize(BulletConfig config, Vector2 position, Vector2 direction)
    {
        Speed = config.speed;
        Damage = config.damage;
        Size = config.size;
        BounceLeft = config.bounceLeft;
        LifeLeft = config.lifeTime;
        
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
            
            if (Vector2.Dot(Direction, normal) > 0)
            {
                return;
            }
            
            Direction = Vector2.Reflect(Direction, normal);
            BounceLeft--;
            if (BounceLeft <= 0)
            {
                BulletFactory.Instance.DestroyBullet(this);
            }
        }
    }
}