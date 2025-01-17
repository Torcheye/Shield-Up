public class PlayerDamageable : Damageable
{
    private void OnEnable()
    {
        IsPlayer = true;
    }

    protected override void Start()
    {
        base.Start();
        DataManager.Instance.playerHp = DataManager.Instance.playerMaxHp;
        Hp = DataManager.Instance.playerHp;
        UIManager.Instance.UpdatePlayerHp(DataManager.Instance.playerHp, DataManager.Instance.playerMaxHp);
    }

    protected override void OnTakeDamage(int dmg)
    {
        DataManager.Instance.playerHp = Hp;
        UIManager.Instance.UpdatePlayerHp(DataManager.Instance.playerHp, DataManager.Instance.playerMaxHp);
    }
}