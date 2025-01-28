using System.Collections;
using DG.Tweening;
using UnityEngine;

public class BrainMoveController : BossMoveController
{
    [SerializeField] private BossAttack attack;
    [SerializeField] private float normalAttackPrepTime;
    [SerializeField] private float normalAttackPostTime;
    [SerializeField] private Bounds bounds;
    [SerializeField] private float disappearTime;
    [SerializeField] private Transform sprite;
    [SerializeField] private Vector2 shakeStrength;
    [SerializeField] private int shakeVibrato;

    private float _moveTime;

    protected override void Start()
    {
        base.Start();
        _moveTime = 1 / moveSpeed;
    }

    protected override void OnSetIsActive()
    {
        base.OnSetIsActive();
        
        if (IsActive)
        {
            StartCoroutine(DoLoopMove());
            attack.ResetAutoAttack();
        }
    }

    private IEnumerator DoLoopMove()
    {
        while (gameObject.activeInHierarchy)
        {
            if (!DoMove)
            {
                yield return null;
                continue;
            }
            
            yield return MoveToRandomPosition();
            yield return new WaitForSeconds(normalAttackPrepTime);
            
            attack.Attack();
            sprite.DOShakePosition(normalAttackPostTime, shakeStrength, shakeVibrato, 40, false, true, ShakeRandomnessMode.Harmonic);
            yield return new WaitForSeconds(normalAttackPostTime);
        }
    }

    private IEnumerator MoveToRandomPosition()
    {
        transform.DOScale(Vector3.zero, _moveTime).SetEase(Ease.InBack);
        yield return new WaitForSeconds(_moveTime);
        
        transform.position = GetRandomPosition();
        yield return new WaitForSeconds(disappearTime);
        
        transform.DOScale(Vector3.one, _moveTime).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(_moveTime);
    }

    private Vector2 GetRandomPosition()
    {
        var x = Random.Range(bounds.min.x, bounds.max.x);
        var y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(x, y);
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
    #endif
}