using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public float EnhancedAttackTime => enhancedAttackTime;
    
    [SerializeField] protected BossMoveController moveController;
    [SerializeField, ShowIf(nameof(autoAttack))] protected float normalAttackInterval;
    [SerializeField] protected float enhancedAttackTime;
    [SerializeField] protected bool autoAttack = true;

    protected bool loopNormalAttack = true;
    protected bool isDoingEnhancedAttack;

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

    private void DoAttack()
    {
        if (loopNormalAttack)
        {
            Attack();
        }
    }
    
    public bool StartEnhancedAttack()
    {
        if (isDoingEnhancedAttack)
        {
            return false;
        }
        StartCoroutine(DoEnhancedAttack());
        return true;
    }
    
    private IEnumerator DoEnhancedAttack()
    {
        loopNormalAttack = false;
        moveController.DoMove = false;
        isDoingEnhancedAttack = true;
        EnhancedAttack();
        yield return new WaitForSeconds(enhancedAttackTime);
        loopNormalAttack = true;
        moveController.DoMove = true;
        isDoingEnhancedAttack = false;
    }

    public virtual void Attack() { }

    public virtual void EnhancedAttack() { }
}