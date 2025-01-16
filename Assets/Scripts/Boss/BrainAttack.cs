using System.Collections;
using UnityEngine;

public class BrainAttack : BossAttack
{
    [SerializeField] private BulletConfig normalBullet;
    [SerializeField] private BulletConfig enhancedBullet;
    [SerializeField] private int normalBulletAmount;
    [SerializeField] private int normalBurstAmount;
    [SerializeField] private float normalBurstInterval;

    public override void Attack()
    {
        StartCoroutine(DoAttack());
    }
    
    private IEnumerator DoAttack()
    {
        var amount = normalBulletAmount;
        for (int i = 0; i < normalBurstAmount; i++)
        {
            BurstAttack(amount--);
            yield return new WaitForSeconds(normalBurstInterval);
        }
    }
    
    private void BurstAttack(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var angle = 360f / amount * i;
            var dir = Quaternion.Euler(0, 0, angle) * Vector3.up;
            ShootBullet(dir);
        }
    }

    public override void EnhancedAttack()
    {
    }

    private void ShootBullet(Vector2 dir)
    {
        BulletFactory.Instance.SpawnBullet(normalBullet, transform.position, dir, true, transform);
    }
}