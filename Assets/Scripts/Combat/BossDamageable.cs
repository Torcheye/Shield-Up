﻿using TorcheyeUtility;
using UnityEngine;

public class BossDamageable : Damageable
{
    public int HitCount { get; private set; }
    public BossType BossType => bossType;
    
    [SerializeField] private BossType bossType;
    [SerializeField] private BossAttack bossAttack;
    [SerializeField] private BossMoveController bossMoveController;
    [SerializeField] private BossStateManager bossStateManager;
    [SerializeField] private ParticleSystem normalBleed;

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
        
        DataManager.Instance.OnBossAttackBoostEnable.AddListener(() =>
        {
            if (bossMoveController.IsActive)
            {
                ToggleBoostMaterial(true);
            }
        });
        
        DataManager.Instance.OnBossAttackBoostDisable.AddListener(() =>
        {
            ToggleBoostMaterial(false);
        });
    }
    
    public void ToggleBoostMaterial(bool enable)
    {
        if (enable)
        {
            rend.material.EnableKeyword(BossType == BossType.Foot ? "ALPHAOUTLINE_ON" : "OUTBASE_ON");
            rend.material.EnableKeyword("COLORRAMP_ON");
        }
        else
        {
            rend.material.DisableKeyword(BossType == BossType.Foot ? "ALPHAOUTLINE_ON" : "OUTBASE_ON");
            rend.material.DisableKeyword("COLORRAMP_ON");
        }
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
        if (bossType == BossType.Heart)
        {
            var heartMoveController = bossMoveController as HeartMoveController;
            return heartMoveController != null && heartMoveController.CanTakeDamage();
        }
        return bossMoveController.IsActive;
    }

    protected override void OnTakeDamage(int dmg)
    {
        if (_enhancedAttackTimer > 0 || !bossMoveController.IsActive || dmg == 0)
        {
            return;
        }
        
        HitCount++;
        XpPickupFactory.Instance.SpawnItem(transform.position);
        AudioManager.Instance.PlaySoundEffect(AudioManager.SoundEffect.BulletHitFlesh);
        normalBleed.Play();
        
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

    protected override void Die()
    {
        if (BossType == BossType.Heart)
        {
            var heartMoveController = bossMoveController as HeartMoveController;
            if (heartMoveController != null) 
                heartMoveController.OnHpIsZero();
        }
        else
        {
            bossMoveController.IsActive = false;
            gameObject.SetActive(false);
            bossStateManager.RemoveBoss(bossMoveController);
        }
    }
}