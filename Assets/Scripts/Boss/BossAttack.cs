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

    private void Start()
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
    
    public void StartEnhancedAttack()
    {
        StartCoroutine(DoEnhancedAttack());
    }
    
    private IEnumerator DoEnhancedAttack()
    {
        loopNormalAttack = false;
        moveController.DoMove = false;
        EnhancedAttack();
        yield return new WaitForSeconds(enhancedAttackTime);
        loopNormalAttack = true;
        moveController.DoMove = true;
    }

    public virtual void Attack() { }

    public virtual void EnhancedAttack() { }
}