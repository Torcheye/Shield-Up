using System.Collections;
using Spine.Unity;
using UnityEngine;

public class BrainAttack : BossAttack
{
    [SerializeField] private BulletConfig normalBullet;
    [SerializeField] private BulletConfig enhancedBullet;
    [SerializeField] private int normalBulletAmount;
    [SerializeField] private int normalBurstAmount;
    [SerializeField] private float normalBurstInterval;
    [SerializeField] private int enhancedShootAmount;
    [SerializeField] private float enhancedShootPrepTime;
    
    [Header("Animation")]
    [SerializeField, SpineAnimation] private string attackAnimation;
    [SerializeField, SpineAnimation] private string idleAnimation;
    [SerializeField, SpineAnimation] private string enhancedAnimation;

    public override void Attack()
    {
        StartCoroutine(DoAttack());
    }
    
    private IEnumerator DoAttack()
    {
        moveController.CanSetInactive = false;

        for (int i = 0; i < normalBurstAmount; i++)
        {
            if (!moveController.IsActive)
            {
                yield break;
            }

            StartCoroutine(BurstAttack(normalBulletAmount, normalBullet));
            
            skeletonAnimation.AnimationState.SetAnimation(0, attackAnimation, false);
            if (i == normalBurstAmount - 1)
            {
                skeletonAnimation.AnimationState.AddAnimation(0, idleAnimation, true, 0);
            }
            
            yield return new WaitForSeconds(normalBurstInterval);
        }
        moveController.CanSetInactive = true;
    }
    
    private IEnumerator BurstAttack(int amount, BulletConfig config, bool enhanced = false)
    {
        if (enhanced)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, enhancedAnimation, false);
            skeletonAnimation.AnimationState.AddAnimation(0, idleAnimation, true, 0);
            moveController.CanSetInactive = false;
        }
        yield return new WaitForSeconds(enhanced ? enhancedShootPrepTime : 0);
        for (int i = 0; i < amount; i++)
        {
            var angle = 360f / amount * i;
            var dir = Quaternion.Euler(0, 0, angle) * Vector3.up;
            BulletFactory.Instance.SpawnBullet(config, transform.position, dir, true, transform, DataManager.Instance.playerTransform);
        }
        moveController.CanSetInactive = true;
    }

    public override void EnhancedAttack()
    {
        StartCoroutine(BurstAttack(enhancedShootAmount, enhancedBullet, true));
    }
}