using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int Level { get; set; }
    public bool IsHostile {get; set; }
    public WeaponType Type { get; set; }
    
    protected RingController ringController;
    protected PlayerDamageable playerDamageable;
    
    public void Initialize(bool isHostile, WeaponType type, RingController rc, PlayerDamageable pd)
    {
        ringController = rc;
        playerDamageable = pd;
        
        Level = 1;
        IsHostile = isHostile;
        Type = type;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}