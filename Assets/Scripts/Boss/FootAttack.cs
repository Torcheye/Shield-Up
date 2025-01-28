using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Spine.Unity;
using Unity.Cinemachine;
using UnityEngine;

public class FootAttack : BossAttack
{
    [Header("Normal Attack Movement")] 
    [SerializeField] private float normalAttackPrepTime;
    [SerializeField] private float normalAttackPostTime;
    [SerializeField] private float normalAttackPerDuration;
    [SerializeField] private int normalAttackCount;
    [SerializeField] private float normalAttackJumpPower;
    [SerializeField] private Bounds normalAttackBounds;
    [SerializeField] private Transform spriteTransform;
    
    [Header("Normal Attack Damage")]
    [SerializeField] private int normalAttackDamage;
    [SerializeField] private float normalAttackDamageDuration;
    [SerializeField] private float normalAttackRadius;
    [SerializeField] private bool normalAttackHasEffect;
    [SerializeField, ShowIf(nameof(normalAttackHasEffect))] private Effect normalAttackEffect;
    [SerializeField, ShowIf(nameof(normalAttackHasEffect))] private float normalAttackEffectDuration;
    [SerializeField] private BulletConfig normalAttackBulletConfig;
    [SerializeField] private CinemachineImpulseSource normalAttackImpulseSource;

    [Header("Enhanced Attack")] 
    [SerializeField] private float enhancedAttackPrepTime;
    [SerializeField] private float enhancedAttackPerDuration;
    [SerializeField] private float enhancedAttackWidth;
    [SerializeField] private Vector2[] enhancedAttackStartPositions;
    [SerializeField] private int enhancedAttackDamage;
    [SerializeField] private bool enhancedAttackHasEffect;
    [SerializeField, ShowIf(nameof(enhancedAttackHasEffect))] private Effect enhancedAttackEffect;
    [SerializeField, ShowIf(nameof(enhancedAttackHasEffect))] private float enhancedAttackEffectDuration;
    [SerializeField] private DamageSourceTrigger enhancedAttackTrigger;
    
    [Header("Animation")]
    [SerializeField, SpineAnimation] private string attackAnimation;
    [SerializeField, SpineAnimation] private string idleAnimation;

    private readonly List<Vector2> _normalAttackPositions = new();
    private IEnumerator _normalAttackCoroutine;
    private IEnumerator _loopAttackCoroutine;
    private readonly List<DamageIndicator> _generatedIndicators = new();

    protected override void Start()
    {
        enhancedAttackTrigger.enabled = false;
        enhancedAttackTime = enhancedAttackPrepTime + enhancedAttackPerDuration * 3;
    }
    
    public void OnSetIsActive(bool isActive)
    {
        if (isActive)
        {
            if (_loopAttackCoroutine != null)
                StopCoroutine(_loopAttackCoroutine);
            _loopAttackCoroutine = LoopAttack();
            StartCoroutine(_loopAttackCoroutine);
        }
        else
        {
            if (_loopAttackCoroutine != null)
                StopCoroutine(_loopAttackCoroutine);
            if (_normalAttackCoroutine != null)
                StopCoroutine(_normalAttackCoroutine);
            _generatedIndicators.ForEach(indicator => indicator.gameObject.SetActive(false));
            _generatedIndicators.Clear();
        }
    }

    public override void EnhancedAttack()
    {
        _generatedIndicators.ForEach(indicator => indicator.gameObject.SetActive(false));
        _generatedIndicators.Clear();
        StopCoroutine(_loopAttackCoroutine);
        StopCoroutine(_normalAttackCoroutine);
        StartCoroutine(DoEnhancedAttack());
    }

    private IEnumerator DoEnhancedAttack()
    {
        // reset
        moveController.DoMove = false;

        for (int i = 0; i < 3; i++)
        {
            var flip = enhancedAttackStartPositions[i].x > 0 ? 1 : -1;
            var direction = new Vector2(flip, 0);
            var warningTime = enhancedAttackPrepTime + enhancedAttackPerDuration * i;
            var pos = new Vector2(0, enhancedAttackStartPositions[i].y);
            var indicator = DamageIndicatorFactory.Instance.SpawnItem(pos).GetComponent<DamageIndicator>();
            indicator.InitializeLineNoDamage(warningTime, 0, direction, Mathf.Abs(enhancedAttackStartPositions[i].x * 2), enhancedAttackWidth);
        }
        
        yield return new WaitForSeconds(enhancedAttackPrepTime);

        enhancedAttackTrigger.Initialize(enhancedAttackDamage, enhancedAttackHasEffect, enhancedAttackEffect, enhancedAttackEffectDuration, enhancedAttackWidth / 2);
        enhancedAttackTrigger.enabled = true;

        for (int i = 0; i < 3; i++)
        {
            var flip = enhancedAttackStartPositions[i].x > 0 ? 1 : -1;
            spriteTransform.localScale = new Vector3(flip, 1, 1);
            spriteTransform.localRotation = Quaternion.Euler(0, 0, -flip * 90);
            transform.position = enhancedAttackStartPositions[i];
            transform.DOMoveX(-enhancedAttackStartPositions[i].x, enhancedAttackPerDuration);
            yield return new WaitForSeconds(enhancedAttackPerDuration);
        }
        
        _loopAttackCoroutine = LoopAttack();
        StartCoroutine(_loopAttackCoroutine);
        enhancedAttackTrigger.enabled = false;
        spriteTransform.localRotation = Quaternion.identity;
    }

    private IEnumerator LoopAttack()
    {
        while (gameObject.activeInHierarchy)
        {
            if (!moveController.IsActive)
            {
                yield return null;
                continue;
            }
            _normalAttackCoroutine = DoAttack();
            yield return _normalAttackCoroutine;
            yield return new WaitForSeconds(normalAttackPostTime);
        }
    }

    private IEnumerator DoAttack()
    {
        // reset
        moveController.DoMove = false;
        _normalAttackPositions.Clear();
        
        // calculate positions
        var playerPos = DataManager.Instance.playerTransform.position;
        var direction = -(playerPos - transform.position).normalized;
        var ray = new Ray(transform.position, direction);
        // find destination on bounds in player direction
        normalAttackBounds.IntersectRay(ray, out var distance);
        var distanceSeg = distance / normalAttackCount;
        for (var i = 1; i < normalAttackCount; i++)
        {
            var pos = ray.GetPoint(distanceSeg * i);
            _normalAttackPositions.Add(pos);
        }
        
        var flip = _normalAttackPositions[^1].x < 0 ? 1 : -1;
        spriteTransform.localScale = new Vector3(flip, 1, 1);
        
        // show indicators
        for (var i = 0; i < _normalAttackPositions.Count; i++)
        {
            var pos = _normalAttackPositions[i];
            var warningTime = normalAttackPerDuration * (i + 1) + normalAttackPrepTime;
            var indicator = DamageIndicatorFactory.Instance.SpawnItem(pos).GetComponent<DamageIndicator>();
            _generatedIndicators.Add(indicator);
            indicator.InitializeCircle(true, warningTime, normalAttackDamageDuration, normalAttackRadius, normalAttackDamage, normalAttackHasEffect, normalAttackEffect, normalAttackEffectDuration);
        }

        yield return new WaitForSeconds(normalAttackPrepTime);
        
        // move
        for (var i = 0; i < _normalAttackPositions.Count; i++)
        {
            transform.DOJump(_normalAttackPositions[i], normalAttackJumpPower, 1, normalAttackPerDuration).SetEase(Ease.InExpo);
            yield return new WaitForSeconds(normalAttackPerDuration);
            
            // shoot in normal of direction
            BulletFactory.Instance.SpawnBullet(normalAttackBulletConfig, transform.position, new Vector2(direction.y, -direction.x), true, transform);
            BulletFactory.Instance.SpawnBullet(normalAttackBulletConfig, transform.position, new Vector2(-direction.y, direction.x), true, transform);
            
            normalAttackImpulseSource.GenerateImpulse();
            
            skeletonAnimation.AnimationState.SetAnimation(0, attackAnimation, false);
            skeletonAnimation.AnimationState.AddAnimation(0, idleAnimation, true, 0);
        }
        moveController.DoMove = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(normalAttackBounds.center, normalAttackBounds.size);
        
        if (_normalAttackPositions.Count > 0)
        {
            Gizmos.color = Color.red;
            foreach (var pos in _normalAttackPositions)
            {
                Gizmos.DrawWireSphere(pos, 0.5f);
            }
        }
    }
#endif
}