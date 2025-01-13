using System;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] protected BossMoveController moveController;
    [SerializeField] protected float attackInterval;
    //[SerializeField] protected BossDamageable damageable;

    private void Start()
    {
        InvokeRepeating(nameof(Attack), attackInterval, attackInterval);
    }

    protected virtual void Attack()
    {
        
    }
}