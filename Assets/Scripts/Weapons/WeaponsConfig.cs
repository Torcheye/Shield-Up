using UnityEngine;

[CreateAssetMenu(fileName = "WeaponsConfig", menuName = "WeaponsConfig")]
public class WeaponsConfig : ScriptableObject
{
    [Header("Dagger")]
    public GameObject daggerPrefab;
    public Sprite daggerSprite;
    public Sprite daggerL3Sprite;
    public int daggerDmgL1;
    public int daggerDmgL2;
    public int daggerBleed;
    
    [Header("Shield")]
    public GameObject shieldPrefab;
    public Sprite shieldSprite;
    public Sprite shieldL3Sprite;
    public float shieldCooldown;
    public int shieldBlockL1;
    public int shieldBlockL2;
    
    [Header("Potion")]
    public GameObject potionPrefab;
    public Sprite potionSprite;
    public Sprite potionL3Sprite;
    public int potionChargeL1;
    public int potionChargeL2;
    
    public int GetDaggerDamage(int level)
    {
        switch (level)
        {
            case 1:
                return daggerDmgL1;
            case 2:
                return daggerDmgL2;
            case 3:
            default:
                return 0;
        }
    }
    
    public int GetShieldBlock(int level)
    {
        switch (level)
        {
            case 1:
                return shieldBlockL1;
            case 2:
            case 3:
                return shieldBlockL2;
            default:
                return 0;
        }
    }
    
    public GameObject GetWeaponPrefab(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Dagger:
                return daggerPrefab;
            case WeaponType.Shield:
                return shieldPrefab;
            case WeaponType.Potion:
                return potionPrefab;
            default:
                return null;
        }
    }
    
    public Sprite GetWeaponSprite(WeaponType weaponType, bool isL1)
    {
        switch (weaponType)
        {
            case WeaponType.Dagger:
                return isL1 ? daggerSprite : daggerL3Sprite;
            case WeaponType.Shield:
                return isL1 ? shieldSprite : shieldL3Sprite;
            case WeaponType.Potion:
                return isL1 ? potionSprite : potionL3Sprite;
            default:
                return null;
        }
    }
    
    public int GetPotionCharge(int level)
    {
        switch (level)
        {
            case 1:
                return potionChargeL1;
            case 2:
            case 3:
                return potionChargeL2;
            default:
                return 0;
        }
    }
}