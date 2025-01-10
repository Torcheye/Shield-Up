using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossConfig", menuName = "Boss Config")]
public class BossConfig : ScriptableObject
{
    public int baseHp;
    public float baseMoveSpeed;

    public List<bool> enables;
    public List<bool> hpOverrides;
    public List<bool> moveSpeedOverrides;

    [Header("Eye")] 
    public int eyeHp;
    public float eyeMoveSpeed;
    public BulletConfig eyeBaseBullet;
    public float eyeBaseShootInterval;
    
    [Header("Brain")] 
    public int brainHp;
    public float brainMoveSpeed;
    
    [Header("Mouth")] 
    public int mouthHp;
    public float mouthMoveSpeed;
    
    [Header("Hand")] 
    public int handHp;
    public float handMoveSpeed;
    
    [Header("Foot")] 
    public int footHp;
    public float footMoveSpeed;
    
    [Header("Heart")] 
    public int heartHp;
    public float heartMoveSpeed;

    public bool GetBossEnable(BossType type)
    {
        return enables[(int)type];
    }

    public int GetBossHp(BossType type)
    {
        if (hpOverrides[(int)type])
        {
            return type switch
            {
                BossType.Eye => eyeHp,
                BossType.Brain => brainHp,
                BossType.Mouth => mouthHp,
                BossType.Hand => handHp,
                BossType.Foot => footHp,
                BossType.Heart => heartHp,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        return baseHp;
    }
    
    public float GetBossMoveSpeed(BossType type)
    {
        if (moveSpeedOverrides[(int)type])
        {
            return type switch
            {
                BossType.Eye => eyeMoveSpeed,
                BossType.Brain => brainMoveSpeed,
                BossType.Mouth => mouthMoveSpeed,
                BossType.Hand => handMoveSpeed,
                BossType.Foot => footMoveSpeed,
                BossType.Heart => heartMoveSpeed,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        return baseMoveSpeed;
    }
} 