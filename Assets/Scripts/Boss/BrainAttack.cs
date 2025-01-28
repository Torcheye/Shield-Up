using System.Collections;
using UnityEngine;

public class BrainAttack : BossAttack
{
    [SerializeField] private BulletConfig normalBullet;
    [SerializeField] private BulletConfig enhancedBullet;
    [SerializeField] private int normalBulletAmount;
    [SerializeField] private int normalBurstAmount;
    [SerializeField] private float normalBurstInterval;
    [SerializeField] private int enhancedShootAmount;

    public override void Attack()
    {
        StartCoroutine(DoAttack());
    }
    
    private IEnumerator DoAttack()
    {
        for (int i = 0; i < normalBurstAmount; i++)
        {
            if (!moveController.IsActive)
            {
                yield break;
            }
            BurstAttack(normalBulletAmount, normalBullet);
            yield return new WaitForSeconds(normalBurstInterval);
        }
    }
    
    private void BurstAttack(int amount, BulletConfig config)
    {
        for (int i = 0; i < amount; i++)
        {
            var angle = 360f / amount * i;
            var dir = Quaternion.Euler(0, 0, angle) * Vector3.up;
            BulletFactory.Instance.SpawnBullet(config, transform.position, dir, true, transform);
        }
    }

    public override void EnhancedAttack()
    {
        BurstAttack(enhancedShootAmount, enhancedBullet);
    }
}