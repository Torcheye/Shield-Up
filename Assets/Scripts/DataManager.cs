using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    
    [Header("Player")]
    public int playerMaxHp;
    public int playerHp;
    
    [Header("Weapons")]
    public WeaponsConfig weaponsConfig;
    public float shieldDeflectAngle;
    
    [Header("Combat")]
    public Color damageColor = Color.yellow;
    public float damageColorLastDuration;
    public float damageColorFadeDuration;
    public float damageCooldown;
    
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