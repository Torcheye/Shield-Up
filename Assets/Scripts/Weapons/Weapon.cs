using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int Level { get; set; }
    public bool IsHostile {get; set; }
    public WeaponType Type { get; set; }
    
    public void Initialize(bool isHostile, WeaponType type)
    {
        Level = 1;
        IsHostile = isHostile;
        Type = type;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}