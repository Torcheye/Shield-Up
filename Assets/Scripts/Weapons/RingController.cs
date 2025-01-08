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
    
    [Header("Weapons")]
    [SerializeField] private WeaponsConfig weaponsConfig;

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
        SpawnWeapon(WeaponType.Dagger, 0);
        UpdateRing(0);
        SpawnWeapon(WeaponType.Dagger, 1);
        UpdateRing(1);
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
            Debug.Log(new Vector3(x, y, 0));
        }
    }

    private void SpawnWeapon(WeaponType type, int ringIndex)
    {
        var w = Instantiate(weaponsConfig.GetWeaponPrefab(type), weaponPivots[ringIndex]).GetComponent<Weapon>();
        _weapons[ringIndex].Add(w);
        
        w.SetRingIndex(ringIndex);
    }

    private void Update()
    {
        transform.position = ringPivot.position;
        for (var i = 0; i < 3; i++)
        {
            weaponPivots[i].Rotate(Vector3.back, rotateSpeeds[i] * Time.deltaTime);
        }
    }
}