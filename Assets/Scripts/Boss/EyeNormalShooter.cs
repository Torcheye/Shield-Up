using UnityEngine;

public class EyeNormalShooter : BulletShooter, IBossAttack
{
    [SerializeField] private float angle;
    [SerializeField] private float attackTime;
    
    public void Attack()
    {
        Shoot();
    }

    public float AttackTime => attackTime;

    protected override void Shoot()
    {
        ShootBullet(GetTargetDirection());
        ShootBullet(Quaternion.Euler(0, 0, angle) * GetTargetDirection());
        ShootBullet(Quaternion.Euler(0, 0, -angle) * GetTargetDirection());
    }
}