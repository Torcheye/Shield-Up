using UnityEngine;

public class BossDamageable : Damageable
{
    [SerializeField] private BossType bossType;
    
    private void OnEnable()
    {
        IsPlayer = false;
    }

    protected override void Start()
    {
        base.Start();

        Hp = DataManager.Instance.bossConfig.GetBossHp(bossType);
    }
}