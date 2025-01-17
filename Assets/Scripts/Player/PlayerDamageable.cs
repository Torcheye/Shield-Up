public class PlayerDamageable : Damageable
{
    private void OnEnable()
    {
        isPlayer = true;
    }

    protected override void Start()
    {
        base.Start();
        Hp = DataManager.Instance.playerMaxHp;
        maxHp = Hp;
        UIManager.Instance.UpdatePlayerHp(Hp, DataManager.Instance.playerMaxHp);
    }

    protected override void OnTakeDamage(int dmg)
    {
        UIManager.Instance.UpdatePlayerHp(Hp, DataManager.Instance.playerMaxHp);
    }
    
    protected override void OnHeal(int amount)
    {
        UIManager.Instance.UpdatePlayerHp(Hp, DataManager.Instance.playerMaxHp);
    }
}