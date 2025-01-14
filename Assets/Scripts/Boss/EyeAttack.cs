using System.Collections;
using UnityEngine;

public class EyeAttack : BossAttack
{
    [SerializeField] private float angle;
    [SerializeField] private BulletConfig normalBullet;
    [SerializeField] private BulletConfig enhancedBullet;
    [SerializeField] private Transform target;

    protected override void Attack()
    {
        ShootBullet(GetTargetDirection());
        ShootBullet(Quaternion.Euler(0, 0, angle) * GetTargetDirection());
        ShootBullet(Quaternion.Euler(0, 0, -angle) * GetTargetDirection());
    }

    protected override void EnhancedAttack()
    {
        BulletFactory.Instance.SpawnBullet(enhancedBullet, transform.position, GetTargetDirection(), true, transform, target);
    }

    private void ShootBullet(Vector2 dir)
    {
        BulletFactory.Instance.SpawnBullet(normalBullet, transform.position, dir, true, transform);
    }
    
    private Vector2 GetTargetDirection()
    {
        return target.position - transform.position;
    }
}