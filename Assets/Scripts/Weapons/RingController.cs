using System.Collections.Generic;
using UnityEngine;

public class RingController : MonoBehaviour
{
    [Header("Ring")]
    [SerializeField] private Transform ringPivot;
    [SerializeField] private Transform[] weaponPivots;
    [SerializeField] private float[] rotateSpeeds;
    [SerializeField] private float firstRingRadius;
    [SerializeField] private float ringRadiusStep;

    private List<List<Weapon>> _weapons;
    private int _nextWeaponId;

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
        SpawnWeapon(WeaponType.Dagger, 0);
        SpawnWeapon(WeaponType.Shield, 0);
        UpdateRing(0);
    }
    
    private void Update()
    {
        transform.position = ringPivot.position;
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
        
        w.Initialize(_nextWeaponId++);
    }
}