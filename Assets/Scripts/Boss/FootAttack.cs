using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class FootAttack : BossAttack
{
    [Header("Normal Attack Movement")] 
    [SerializeField] private float normalAttackPrepTime;
    [SerializeField] private float normalAttackPerDuration;
    [SerializeField] private int normalAttackCount;
    [SerializeField] private float normalAttackJumpPower;
    [SerializeField] private Bounds normalAttackBounds;
    
    [Header("Normal Attack Damage")]
    [SerializeField] private int normalAttackDamage;
    [SerializeField] private float normalAttackDamageDuration;
    [SerializeField] private float normalAttackRadius;
    [SerializeField] private bool normalAttackHasEffect;
    [SerializeField, ShowIf(nameof(normalAttackHasEffect))] private Effect normalAttackEffect;
    [SerializeField, ShowIf(nameof(normalAttackHasEffect))] private float normalAttackEffectDuration;
    
    [Header("Normal Attack Bullet")]
    [SerializeField] private BulletConfig normalAttackBulletConfig;

    private readonly List<Vector2> _normalAttackPositions = new();
    
    public IEnumerator DoAttack()
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
        
        // show indicators
        for (var i = 0; i < _normalAttackPositions.Count; i++)
        {
            var pos = _normalAttackPositions[i];
            var warningTime = normalAttackPerDuration * (i + 1) + normalAttackPrepTime;
            var indicator = DamageIndicatorFactory.Instance.SpawnItem(pos).GetComponent<DamageIndicator>();
            StartCoroutine(indicator.Initialize(pos, warningTime, normalAttackDamageDuration, normalAttackRadius, 
                normalAttackDamage, normalAttackHasEffect, normalAttackEffect, normalAttackEffectDuration));
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
        }
        moveController.DoMove = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
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