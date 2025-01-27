﻿using UnityEngine;

public class BossDamageable : Damageable
{
    public int HitCount { get; private set; }
    public BossType BossType => bossType;
    
    [SerializeField] private BossType bossType;
    [SerializeField] private BossAttack bossAttack;
    [SerializeField] private BossMoveController bossMoveController;

    private int _maxHit;
    private float _enhancedAttackTimer;
    
    private void OnEnable()
    {
        isPlayer = false;
    }

    protected override void Start()
    {
        base.Start();

        Hp = DataManager.Instance.bossConfig.GetBossHp(bossType);
        maxHp = Hp;
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

    protected override bool TryTakeDamage()
    {
        return bossMoveController.IsActive;
    }

    protected override void OnTakeDamage(int dmg)
    {
        if (_enhancedAttackTimer > 0 || !bossMoveController.IsActive)
        {
            return;
        }
        
        HitCount++;
        XpPickupFactory.Instance.SpawnItem(transform.position);
        
        if (HitCount >= _maxHit)
        {
            var success = bossAttack.StartEnhancedAttack();
            if (success)
            {
                HitCount = 0;
                _enhancedAttackTimer = bossAttack.EnhancedAttackTime;
            }
        }
    }
}