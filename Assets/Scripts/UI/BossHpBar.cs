using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    [SerializeField] private Image hp;
    [SerializeField] private Image hit;
    [SerializeField] private BossDamageable bossDamageable;
    
    private int _maxHp;
    private int _maxHit;
    
    private void Start()
    {
        _maxHp = DataManager.Instance.bossConfig.GetBossHp(bossDamageable.BossType);
        _maxHit = DataManager.Instance.bossConfig.GetBossHit(bossDamageable.BossType);
    }

    private void Update()
    {
        var fillAmountHp = (float) bossDamageable.Hp / _maxHp;
        hp.fillAmount = fillAmountHp;
        
        var fillAmountHit = (float) bossDamageable.HitCount / _maxHit;
        hit.fillAmount = fillAmountHit;
    }
}