using System.Collections;
using UnityEngine;

public class FootMoveController : BossMoveController
{
    [SerializeField] private FootAttack attack;
    [SerializeField] private float normalAttackPrepTime;
    [SerializeField] private float normalAttackPostTime;
    
    protected override void Start()
    {
        base.Start();
        
        StartCoroutine(DoLoopMove());
    }
    
    private IEnumerator DoLoopMove()
    {
        while (doMove)
        {
            yield return new WaitForSeconds(normalAttackPrepTime);
            yield return attack.DoAttack();
            yield return new WaitForSeconds(normalAttackPostTime);
        }
    }
}