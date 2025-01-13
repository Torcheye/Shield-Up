using UnityEngine;

public class EyeNormalShooter : BossAttack
{
    [SerializeField] private float angle;
    [SerializeField] private float attackTime;
    [SerializeField] private BulletConfig bulletConfig;
    [SerializeField] private Transform target;

    protected override void Attack()
    {
        ShootBullet(GetTargetDirection());
        ShootBullet(Quaternion.Euler(0, 0, angle) * GetTargetDirection());
        ShootBullet(Quaternion.Euler(0, 0, -angle) * GetTargetDirection());
    }

    public float AttackTime => attackTime;
    
    private void ShootBullet(Vector2 dir)
    {
        BulletFactory.Instance.SpawnBullet(bulletConfig, transform.position, dir, true, transform);
    }
    
    private Vector2 GetTargetDirection()
    {
        return target.position - transform.position;
    }
}