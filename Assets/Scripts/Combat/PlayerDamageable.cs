public class PlayerDamageable : Damageable
{
    protected override void Start()
    {
        base.Start();
        DataManager.Instance.playerHp = DataManager.Instance.playerMaxHp;
        hp = DataManager.Instance.playerHp;
        UIManager.Instance.UpdatePlayerHp(DataManager.Instance.playerHp, DataManager.Instance.playerMaxHp);
    }

    protected override void OnTakeDamage(int dmg)
    {
        DataManager.Instance.playerHp = hp;
        UIManager.Instance.UpdatePlayerHp(DataManager.Instance.playerHp, DataManager.Instance.playerMaxHp);
    }
}