using System.Collections;
using Spine.Unity;
using TorcheyeUtility;
using UnityEngine;
using Random = UnityEngine.Random;

public class EyeAttack : BossAttack
{
    [SerializeField] private float angle;
    [SerializeField] private float delta;
    [SerializeField] private BulletConfig normalBullet;
    [SerializeField] private BulletConfig enhancedBullet;
    [SerializeField] private float enhancedAttackInterval;
    [SerializeField] private int enhancedAttackCount;
    
    [Header("Animation")]
    [SerializeField, SpineAnimation] private string attackAnimation;
    [SerializeField, SpineAnimation] private string idleAnimation;
    
    [Header("Boost")]
    [SerializeField] private float normalAttackBoostIntervalMultiplier;
    
    private Coroutine _enhancedAttackCoroutine;

    protected override void Start()
    {
        base.Start();
        
        DataManager.Instance.OnBossAttackBoostEnable.AddListener(() =>
        {
            _normalAttackInterval = normalAttackInterval * normalAttackBoostIntervalMultiplier;
            ResetAutoAttack();
        });
        
        DataManager.Instance.OnBossAttackBoostDisable.AddListener(() =>
        {
            _normalAttackInterval = normalAttackInterval;
        });
    }

    public override void OnSetInactive()
    {
        base.OnSetInactive();
        
        if (_enhancedAttackCoroutine != null)
            StopCoroutine(_enhancedAttackCoroutine);
    }

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
        _enhancedAttackCoroutine = StartCoroutine(DoEnhancedAttack());
    }

    private IEnumerator DoEnhancedAttack()
    {
        moveController.CanSetInactive = true;
        DataManager.Instance.ToggleShake(true);
        for (int i = 0; i < enhancedAttackCount; i++)
        {
            var dir = Quaternion.Euler(0, 0, Random.Range(-delta, delta)) * GetTargetDirection();
            BulletFactory.Instance.SpawnBullet(enhancedBullet, transform.position, dir, true, transform, DataManager.Instance.playerTransform);
            AudioManager.Instance.PlaySoundEffect(AudioManager.SoundEffect.BossChargeBullet, 0.5f);
            yield return new WaitForSeconds(enhancedAttackInterval);
        }
        DataManager.Instance.ToggleShake(false);
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