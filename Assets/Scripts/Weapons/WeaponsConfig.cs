using UnityEngine;

[CreateAssetMenu(fileName = "WeaponsConfig", menuName = "WeaponsConfig")]
public class WeaponsConfig : ScriptableObject
{
    [Header("Dagger")]
    public GameObject daggerPrefab;
    public int daggerDmgL1;
    public int daggerDmgL2;
    public int daggerBleedL2;
    public int daggerDmgL3;
    public int daggerBleedL3;
    
    [Header("Shield")]
    public GameObject shieldPrefab;
    public int shieldBlockL1;
    public int shieldBlockL2;
    public int shieldBlockL3;
    
    [Header("Bomb")]
    public GameObject bombPrefab;
    public int bombMaxAbsorbL1;
    public int bombMaxAbsorbL2;
    public int bombMaxAbsorbL3;
    public int bombBaseDmg;
    
    public int GetDaggerDamage(int level)
    {
        switch (level)
        {
            case 1:
                return daggerDmgL1;
            case 2:
                return daggerDmgL2;
            case 3:
                return daggerDmgL3;
            default:
                return 0;
        }
    }
    
    public int GetDaggerBleed(int level)
    {
        switch (level)
        {
            case 2:
                return daggerBleedL2;
            case 3:
                return daggerBleedL3;
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
                return shieldBlockL2;
            case 3:
                return shieldBlockL3;
            default:
                return 0;
        }
    }
    
    public int GetBombMaxAbsorb(int level)
    {
        switch (level)
        {
            case 1:
                return bombMaxAbsorbL1;
            case 2:
                return bombMaxAbsorbL2;
            case 3:
                return bombMaxAbsorbL3;
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
            case WeaponType.Bomb:
                return bombPrefab;
            default:
                return null;
        }
    }
}