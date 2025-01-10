using System;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    [SerializeField] private Image bar;
    [SerializeField] private BossDamageable bossDamageable;
    
    private int _maxHp;
    
    private void Start()
    {
        _maxHp = DataManager.Instance.bossConfig.GetBossHp(bossDamageable.BossType);
    }

    private void Update()
    {
        var fillAmount = (float) bossDamageable.GetHp() / _maxHp;
        bar.fillAmount = fillAmount;
    }
    
    
}