using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    [SerializeField] private Image hp;
    [SerializeField] private Image hit;
    [SerializeField] private BossDamageable bossDamageable;
    
    private int _maxHp;
    private int _maxHit;
    private static readonly int Value = Shader.PropertyToID("_Value");
    private static readonly int MaxValue = Shader.PropertyToID("_MaxValue");
    
    private void Start()
    {
        _maxHp = DataManager.Instance.bossConfig.GetBossHp(bossDamageable.BossType);
        _maxHit = DataManager.Instance.bossConfig.GetBossHit(bossDamageable.BossType);
    }

    private void Update()
    {
        hp.material.SetFloat(Value, bossDamageable.Hp);
        hp.material.SetFloat(MaxValue, _maxHp);
        
        hit.material.SetFloat(Value, bossDamageable.HitCount);
        hit.material.SetFloat(MaxValue, _maxHit);
    }
}