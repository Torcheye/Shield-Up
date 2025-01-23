using UnityEngine;

public class EyeAttack : BossAttack
{
    [SerializeField] private float angle;
    [SerializeField] private BulletConfig normalBullet;
    [SerializeField] private BulletConfig enhancedBullet;

    public override void Attack()
    {
        for (int i = 0; i < 3; i++)
        {
            var dir = Quaternion.Euler(0, 0, Random.Range(-angle, angle)) * GetTargetDirection();
            ShootBullet(dir);
        }
    }

    public override void EnhancedAttack()
    {
        BulletFactory.Instance.SpawnBullet(enhancedBullet, transform.position, GetTargetDirection(), true, transform, DataManager.Instance.playerTransform);
    }

    private void ShootBullet(Vector2 dir)
    {
        BulletFactory.Instance.SpawnBullet(normalBullet, transform.position, dir, true, transform);
    }
    
    private Vector2 GetTargetDirection()
    {
        return DataManager.Instance.playerTransform.position - transform.position;
    }
}