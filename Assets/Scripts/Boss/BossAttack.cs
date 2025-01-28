using System.Collections;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public float EnhancedAttackTime => enhancedAttackTime;
    
    [SerializeField] protected BossMoveController moveController;
    [SerializeField, ShowIf(nameof(autoAttack))] protected float normalAttackInterval;
    [SerializeField] protected float enhancedAttackTime;
    [SerializeField] protected bool autoAttack = true;
    [SerializeField] protected SkeletonAnimation skeletonAnimation;

    private bool _loopNormalAttack = true;
    private bool _isDoingEnhancedAttack;

    protected virtual void Start()
    {
        if (autoAttack)
        {
            InvokeRepeating(nameof(DoAttack), normalAttackInterval, normalAttackInterval);
        }
    }
    
    private void OnDisable()
    {
        CancelInvoke(nameof(DoAttack));
    }
    
    public void ResetAutoAttack()
    {
        CancelInvoke(nameof(DoAttack));
        InvokeRepeating(nameof(DoAttack), normalAttackInterval, normalAttackInterval);
    }

    private void DoAttack()
    {
        if (_loopNormalAttack && moveController.IsActive)
        {
            Attack();
        }
    }
    
    public bool StartEnhancedAttack()
    {
        if (_isDoingEnhancedAttack || !moveController.IsActive)
        {
            return false;
        }
        StartCoroutine(DoEnhancedAttack());
        return true;
    }
    
    private IEnumerator DoEnhancedAttack()
    {
        _loopNormalAttack = false;
        moveController.DoMove = false;
        _isDoingEnhancedAttack = true;
        EnhancedAttack();
        yield return new WaitForSeconds(enhancedAttackTime);
        _loopNormalAttack = true;
        moveController.DoMove = true;
        _isDoingEnhancedAttack = false;
    }

    public virtual void Attack() { }

    public virtual void EnhancedAttack() { }
}