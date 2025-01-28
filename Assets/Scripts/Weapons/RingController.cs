using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class RingController : MonoBehaviour
{
    [SerializeField] private Transform ringPivot;
    [SerializeField] private Transform[] weaponPivots;
    [SerializeField] private float[] rotateSpeeds;
    [SerializeField] private float firstRingRadius;
    [SerializeField] private float ringRadiusStep;
    
    [SerializeField] private WeaponType startWeaponType;
    [SerializeField] private bool isHostile;
    [SerializeField] private float moveSpeed;
    [SerializeField] private PlayerDamageable playerDamageable;

    [ShowInInspector] private Dictionary<Vector2Int, Weapon> _weapons;
    
    public void GetWeapon(Vector2Int slotIndex, out WeaponType type, out int level)
    {
        if (slotIndex.x < 0 || slotIndex.x >= 3 || slotIndex.y < 0 || slotIndex.y >= 3)
        {
            throw new Exception("Invalid ring or weapon index");
        }
        
        if (!_weapons.TryGetValue(slotIndex, out var weapon))
        {
            type = WeaponType.Dagger;
            level = 0;
            return;
        }

        type = weapon.Type;
        level = weapon.Level;
    }
    
    public void AddNewWeapon(Vector2Int slotIndex, WeaponType type)
    {
        if (slotIndex.x < 0 || slotIndex.x >= 3 || slotIndex.y < 0 || slotIndex.y >= 3)
        {
            throw new Exception("Invalid ring or weapon index");
        }
        
        if (_weapons.ContainsKey(slotIndex))
        {
            Debug.LogError("Weapon already exists at " + slotIndex);
            return;
        }
        
        SpawnWeapon(type, slotIndex);
    }
    
    public void UpgradeWeapon(Vector2Int slotIndex)
    {
        if (slotIndex.x < 0 || slotIndex.x >= 3 || slotIndex.y < 0 || slotIndex.y >= 3)
        {
            throw new Exception("Invalid ring or weapon index");
        }
        
        if (!_weapons.TryGetValue(slotIndex, out var weapon))
        {
            Debug.LogError("No weapon to upgrade at " + slotIndex);
            return;
        }

        weapon.Level++;
    }
    
    public void RemoveWeapon(Vector2Int slotIndex)
    {
        if (slotIndex.x < 0 || slotIndex.x >= 3 || slotIndex.y < 0 || slotIndex.y >= 3)
        {
            throw new Exception("Invalid ring or weapon index");
        }
        
        if (!_weapons.TryGetValue(slotIndex, out var weapon))
        {
            Debug.LogError("No weapon to remove at " + slotIndex);
            return;
        }
        
        Destroy(weapon.gameObject);
        _weapons.Remove(slotIndex);
    }

    private void Awake()
    {
        _weapons = new Dictionary<Vector2Int, Weapon>();
    }

    private void Start()
    {
        if (isHostile)
            return;
        SpawnWeapon(startWeaponType, new Vector2Int(1, 0));
    }
    
    private void LateUpdate()
    {
        if (ringPivot == null) return;
        
        transform.position = Vector2.Lerp(transform.position, ringPivot.position, moveSpeed * Time.deltaTime);
        
        for (var i = 0; i < 3; i++)
        {
            weaponPivots[i].Rotate(Vector3.back, rotateSpeeds[i] * Time.deltaTime);
        }
    }

    private void SpawnWeapon(WeaponType type, Vector2Int slotIndex)
    {
        var w = Instantiate(DataManager.Instance.weaponsConfig.GetWeaponPrefab(type), 
            weaponPivots[slotIndex.x]).GetComponent<Weapon>();
        _weapons[slotIndex] = w;

        w.Initialize(isHostile, type, this, playerDamageable, slotIndex);
        
        var ringIndex = slotIndex.x;
        var weaponIndex = slotIndex.y;
        const float angleStep = Mathf.PI * 2 / 3;
        var angle = weaponIndex * angleStep;
        var x = Mathf.Cos(angle) * (firstRingRadius + ringRadiusStep * ringIndex);
        var y = Mathf.Sin(angle) * (firstRingRadius + ringRadiusStep * ringIndex);
        w.transform.localPosition = new Vector3(x, y, 0);
    }

    public void CopyOver(RingController other)
    {
        foreach (var weapon in _weapons)
        {
            other.AddNewWeapon(weapon.Key, weapon.Value.Type);
            Debug.Log("Copied over weapon " + weapon.Value.Type + " at " + weapon.Key);
        }
    }
    
    public void ClearAll()
    {
        foreach (var weapon in _weapons)
        {
            Destroy(weapon.Value.gameObject);
        }
        _weapons.Clear();
    }
}