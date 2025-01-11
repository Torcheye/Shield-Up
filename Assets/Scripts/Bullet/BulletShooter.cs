using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    public BulletConfig bulletConfig;
    public Transform target;

    protected virtual void Shoot()
    {
        ShootBullet(GetTargetDirection());
    }
    
    protected void ShootBullet(Vector2 dir)
    {
        BulletFactory.Instance.SpawnBullet(bulletConfig, transform.position, dir, true, transform);
    }
    
    protected Vector2 GetTargetDirection()
    {
        return target.position - transform.position;
    }
}