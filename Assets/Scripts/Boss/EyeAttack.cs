using UnityEngine;

public class EyeAttack : BossAttack
{
    [SerializeField] private float angle;
    [SerializeField] private float delta;
    [SerializeField] private BulletConfig normalBullet;
    [SerializeField] private BulletConfig enhancedBullet;

    public override void Attack()
    {
        ShootBullet(Quaternion.Euler(0, 0, Random.Range(-delta, delta)) * GetTargetDirection());
        ShootBullet(Quaternion.Euler(0, 0, Random.Range(-delta, delta) - angle) * GetTargetDirection());
        ShootBullet(Quaternion.Euler(0, 0, Random.Range(-delta, delta) + angle) * GetTargetDirection());
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