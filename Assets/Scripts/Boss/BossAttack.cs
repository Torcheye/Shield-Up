using System.Collections;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public float EnhancedAttackTime => enhancedAttackTime;
    
    [SerializeField] protected BossMoveController moveController;
    [SerializeField] protected float normalAttackInterval;
    [SerializeField] protected float enhancedAttackTime;

    protected bool loopNormalAttack = true;

    private void Start()
    {
        InvokeRepeating(nameof(DoAttack), normalAttackInterval, normalAttackInterval);
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

    protected virtual void Attack() { }

    protected virtual void EnhancedAttack() { }
}