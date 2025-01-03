using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    public BulletConfig defaultBulletConfig;
    public Transform target;
    public float shootInterval = 1f;

    public void ShootBullet()
    {
        var shootDirection = target.position - transform.position;
        BulletFactory.Instance.SpawnBullet(defaultBulletConfig, transform.position, shootDirection);
    }
    
    private void Start()
    {
        InvokeRepeating(nameof(ShootBullet), 1, shootInterval);
    }
}