using System;
using UnityEngine;

public class BossDamageable : Damageable
{
    public int HitCount { get; private set; }
    public BossType BossType => bossType;
    
    [SerializeField] private BossType bossType;
    [SerializeField] private BossAttack bossAttack;

    private int _maxHit;
    private float _enhancedAttackTimer;
    
    private void OnEnable()
    {
        IsPlayer = false;
    }

    protected override void Start()
    {
        base.Start();

        Hp = DataManager.Instance.bossConfig.GetBossHp(bossType);
        HitCount = 0;
        _maxHit = DataManager.Instance.bossConfig.GetBossHit(bossType);
    }

    private void LateUpdate()
    {
        if (_enhancedAttackTimer > 0)
        {
            _enhancedAttackTimer -= Time.deltaTime;
        }
    }

    protected override void OnTakeDamage(int dmg)
    {
        if (_enhancedAttackTimer > 0)
        {
            return;
        }
        
        HitCount++;
        XpPickupFactory.Instance.GetXp(transform.position);
        
        if (HitCount >= _maxHit)
        {
            bossAttack.StartEnhancedAttack();
            HitCount = 0;
            _enhancedAttackTimer = bossAttack.EnhancedAttackTime;
        }
    }
}