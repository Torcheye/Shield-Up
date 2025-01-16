using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    
    [Header("Player")]
    public int playerMaxHp;
    public int playerHp;
    public int playerXp = 0;
    public int xpToNextLevel = 3;
    public Transform playerTransform;
    
    [Header("Weapons")]
    public WeaponsConfig weaponsConfig;
    
    [Header("Combat")]
    public Color damageColor = Color.yellow;
    public float damageColorLastDuration;
    public float damageColorFadeDuration;
    public float damageCooldown;
    
    [Header("Effects")]
    public float acidHurtInterval;
    public int acidDamage;

    [Header("Boss")]
    public BossConfig bossConfig;
    
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
}