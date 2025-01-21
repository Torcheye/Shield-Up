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

    [ShowInInspector] private List<List<Weapon>> _weapons;
    
    public void GetWeapon(int ringIndex, int weaponIndex, out WeaponType type, out int level)
    {
        if (ringIndex < 0 || ringIndex >= 3 || weaponIndex < 0 || weaponIndex >= _weapons[ringIndex].Count)
        {
            type = WeaponType.Dagger;
            level = 0;
            return;
        }

        type = _weapons[ringIndex][weaponIndex].Type;
        level = _weapons[ringIndex][weaponIndex].Level;
    }
    
    public void SetWeapon(Vector2Int slotIndex, WeaponType type, int level)
    {
        if (slotIndex.x < 0 || slotIndex.x >= 3 || slotIndex.y < 0)
        {
            return;
        }
        
        if (slotIndex.y >= _weapons[slotIndex.x].Count)
        {
            Debug.Log("adding " + type + " to " + slotIndex);
            SpawnWeapon(type, slotIndex);
            return;
        }

        Debug.Log("setting " + type + " to " + slotIndex);
        _weapons[slotIndex.x][slotIndex.y].Level = level;
        _weapons[slotIndex.x][slotIndex.y].Type = type;
    }

    private void Awake()
    {
        _weapons = new List<List<Weapon>>();
        for (int i = 0; i < 3; i++)
        {
            _weapons.Add(new List<Weapon>());
        }
    }

    private void Start()
    {
        if (isHostile)
            return;
        SpawnWeapon(startWeaponType, new Vector2Int(0, 0));
    }

    public void RemoveWeapon(GameObject weapon)
    {
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < _weapons[i].Count; j++)
            {
                if (_weapons[i][j].gameObject == weapon)
                {
                    Destroy(weapon);
                    _weapons[i].RemoveAt(j);
                    return;
                }
            }
        }
    }
    
    private void Update()
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
        _weapons[slotIndex.x].Add(w);

        w.Initialize(isHostile, type, this, playerDamageable);
        
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
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < _weapons[i].Count; j++)
            {
                other.SpawnWeapon(_weapons[i][j].Type, new Vector2Int(i, j));
            }
        }
    }
    
    public void ClearAll()
    {
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < _weapons[i].Count; j++)
            {
                Destroy(_weapons[i][j].gameObject);
            }
            _weapons[i].Clear();
        }
    }
}