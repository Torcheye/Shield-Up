using UnityEngine;

[CreateAssetMenu(fileName = "WeaponsConfig", menuName = "WeaponsConfig")]
public class WeaponsConfig : ScriptableObject
{
    public GameObject daggerPrefab;
    public GameObject shieldPrefab;
    public GameObject bombPrefab;
    
    public float daggerDmg;
    public float shieldBlock;
    public float bombMaxAbsorb;
    
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