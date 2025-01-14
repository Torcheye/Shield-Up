using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "BossConfig", menuName = "Boss Config")]
public class BossConfig : ScriptableObject
{
    public int baseHp;
    public float baseMoveSpeed;
    public int baseHit;

    public List<bool> enables;
    public List<bool> hpOverrides;
    public List<bool> moveSpeedOverrides;
    public List<bool> hitOverrides;

    [Header("Eye")] 
    public int eyeHp;
    public float eyeMoveSpeed;
    public int eyeHit;
    
    [Header("Brain")] 
    public int brainHp;
    public float brainMoveSpeed;
    public int brainHit;
    
    [Header("Mouth")] 
    public int mouthHp;
    public float mouthMoveSpeed;
    public int mouthHit;
    
    [Header("Hand")] 
    public int handHp;
    public float handMoveSpeed;
    public int handHit;
    
    [Header("Foot")] 
    public int footHp;
    public float footMoveSpeed;
    public int footHit;
    
    [Header("Heart")] 
    public int heartHp;
    public float heartMoveSpeed;
    public int heartHit;

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
    
    public int GetBossHit(BossType type)
    {
        if (hitOverrides[(int)type])
        {
            return type switch
            {
                BossType.Eye => eyeHit,
                BossType.Brain => brainHit,
                BossType.Mouth => mouthHit,
                BossType.Hand => handHit,
                BossType.Foot => footHit,
                BossType.Heart => heartHit,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        return baseHit;
    }
} 