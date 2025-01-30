using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    
    public bool IsGamePaused
    {
        get => _isGamePaused;
        set
        {
            _isGamePaused = value;
            Time.timeScale = value ? 0 : 1;
        }
    }
    private bool _isGamePaused;
    
    [Header("Player")]
    public int playerMaxHp;
    public int playerXp = 0;
    public int xpToNextLevel = 3;
    public Transform playerTransform;
    
    [Header("Config")]
    public WeaponsConfig weaponsConfig;
    public BossConfig bossConfig;
    public BulletConfig deflectBullet;
    public BulletConfig normalBullet;
    
    [Header("Combat")]
    public Color damageColor = Color.yellow;
    public float damageColorLastDuration;
    public float damageColorFadeDuration;
    public float damageCooldown;
    public Color healColor = Color.green;
    public float healColorLastDuration;
    public float healColorFadeDuration;
    public Color bossInactiveColor = Color.black;
    public float bossInactiveFillAmount;

    [Header("Boss Rotation")] 
    public float bossRotationTime;
    public List<BossType> bossRotationOrderFTUE;
    
    [Header("Environment")]
    public float breakableGroundRegenTime;
    
    [Header("Effects")]
    public float acidHurtInterval;
    public int acidDamage;
    public float slowEffectMultiplier;
    public float hitStopDuration;
    public float hitStopTimeScale;
    public float bossAttackBoostTime;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public UnityEvent OnBossAttackBoostEnable { get; private set; }
    public UnityEvent OnBossAttackBoostDisable { get; private set; }

    private void OnEnable()
    {
        OnBossAttackBoostEnable = new UnityEvent();
        OnBossAttackBoostDisable = new UnityEvent();
    }

    private void OnDisable()
    {
        OnBossAttackBoostEnable.RemoveAllListeners();
        OnBossAttackBoostDisable.RemoveAllListeners();
    }

    public void EnableBossAttackBoost()
    {
        StartCoroutine(DoDisableBossAttackBoost());
    }

    private IEnumerator DoDisableBossAttackBoost()
    {
        OnBossAttackBoostEnable.Invoke();
        yield return new WaitForSeconds(bossAttackBoostTime);
        OnBossAttackBoostDisable.Invoke();
    }
}