using System.Collections;
using Spine.Unity;
using UnityEngine;

public class EyeAttack : BossAttack
{
    [SerializeField] private float angle;
    [SerializeField] private float delta;
    [SerializeField] private BulletConfig normalBullet;
    [SerializeField] private BulletConfig enhancedBullet;
    
    [Header("Animation")]
    [SerializeField, SpineAnimation] private string attackAnimation;
    [SerializeField, SpineAnimation] private string idleAnimation;

    public override void Attack()
    {
        ShootBullet(Quaternion.Euler(0, 0, Random.Range(-delta, delta)) * GetTargetDirection());
        ShootBullet(Quaternion.Euler(0, 0, Random.Range(-delta, delta) - angle) * GetTargetDirection());
        ShootBullet(Quaternion.Euler(0, 0, Random.Range(-delta, delta) + angle) * GetTargetDirection());
        
        skeletonAnimation.AnimationState.SetAnimation(0, attackAnimation, false);
        skeletonAnimation.AnimationState.AddAnimation(0, idleAnimation, true, 0);
    }

    public override void EnhancedAttack()
    {
        StartCoroutine(DoEnhancedAttack());
    }
    
    private IEnumerator DoEnhancedAttack()
    {
        moveController.CanSetInactive = true;
        BulletFactory.Instance.SpawnBullet(enhancedBullet, transform.position, GetTargetDirection(), true, transform, DataManager.Instance.playerTransform);

        yield return new WaitForSeconds(enhancedBullet.chargeSpawnTime);
        moveController.CanSetInactive = true;
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