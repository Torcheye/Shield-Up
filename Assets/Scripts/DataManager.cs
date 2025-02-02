using System;
using System.Collections;
using System.Collections.Generic;
using TorcheyeUtility;
using Unity.Cinemachine;
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
    public int XpToNextLevel => GetXpToNextLevel();
    public Transform playerTransform;
    public Color playerHitColor = Color.red;
    public float playerVignetteLastDuration;
    public int playerLevel;
    
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

    [Header("Boss Rotation")] 
    public float bossRotationTime;
    public List<BossType> bossRotationOrderFTUE;
    
    [Header("Environment")]
    public float breakableGroundRegenTime;
    
    [Header("Effects")]
    public float acidPoolDuration;
    public float acidHurtInterval;
    public int acidDamage;
    public float slowEffectMultiplier;
    public float hitStopDuration;
    public float hitStopTimeScale;
    public float bossAttackBoostTime;
    public float bleedDuration;
    public float bleedHurtInterval;
    public int bleedDamage;
    public bool IsBossAttackBoostEnabled { get; private set; }
    [SerializeField] private GameObject normalBG;
    [SerializeField] private GameObject boostBG;
    [SerializeField] private CinemachineBasicMultiChannelPerlin shake;
    
    public void ToggleShake(bool enable)
    {
        shake.enabled = enable;
    }
    
    public int GetXpToNextLevel()
    {
        return Mathf.RoundToInt(Mathf.Pow(1.15f, playerLevel + 11) - 1.7f);
    }
    
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
        normalBG.SetActive(false);
        boostBG.SetActive(true);
        AudioManager.Instance.PlaySoundEffect(AudioManager.SoundEffect.BossBoost);
        StartCoroutine(DoDisableBossAttackBoost());
    }

    private IEnumerator DoDisableBossAttackBoost()
    {
        IsBossAttackBoostEnabled = true;
        OnBossAttackBoostEnable.Invoke();
        yield return new WaitForSeconds(bossAttackBoostTime);
        OnBossAttackBoostDisable.Invoke();
        IsBossAttackBoostEnabled = false;
        normalBG.SetActive(true);
        boostBG.SetActive(false);
    }
}