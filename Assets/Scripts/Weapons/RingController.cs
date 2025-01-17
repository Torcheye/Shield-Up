using System.Collections.Generic;
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

    private List<List<Weapon>> _weapons;

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
        SpawnWeapon(startWeaponType, 1);
        UpdateRing(1);
    }
    
    private int _nextWeaponRingIndex;
    private WeaponType _nextWeaponType = WeaponType.Dagger;
    public void AddWeapon()
    {
        SpawnWeapon(_nextWeaponType, _nextWeaponRingIndex);
        UpdateRing(_nextWeaponRingIndex);
        
        _nextWeaponRingIndex++;
        if (_nextWeaponRingIndex >= 3)
        {
            _nextWeaponRingIndex = 0;
        }
        
        if (_nextWeaponType == WeaponType.Dagger)
        {
            _nextWeaponType = WeaponType.Shield;
        }
        else
        {
            _nextWeaponType = WeaponType.Dagger;
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

    private void UpdateRing(int ringIndex)
    {
        var count = _weapons[ringIndex].Count;
        var angleStep = Mathf.PI * 2 / count;
        
        for (var i = 0; i < count; i++)
        {
            var angle = i * angleStep;
            var x = Mathf.Cos(angle) * (firstRingRadius + ringRadiusStep * ringIndex);
            var y = Mathf.Sin(angle) * (firstRingRadius + ringRadiusStep * ringIndex);
            _weapons[ringIndex][i].transform.localPosition = new Vector3(x, y, 0);
        }
    }

    private void SpawnWeapon(WeaponType type, int ringIndex)
    {
        var w = Instantiate(DataManager.Instance.weaponsConfig.GetWeaponPrefab(type), 
            weaponPivots[ringIndex]).GetComponent<Weapon>();
        _weapons[ringIndex].Add(w);

        w.Initialize(isHostile, type);
    }

    public void CopyOver(RingController other)
    {
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < _weapons[i].Count; j++)
            {
                other.SpawnWeapon(_weapons[i][j].Type, i);
                other.UpdateRing(i);
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