using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    public static BulletFactory Instance { get; private set; }
    public GameObject bulletPrefab;
    public Transform bulletParent;
    public int maxBulletCount = 100;
    
    private List<Bullet> _bullets = new List<Bullet>();
    
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        for (var i = 0; i < maxBulletCount; i++)
        {
            var bullet = Instantiate(bulletPrefab, bulletParent).GetComponent<Bullet>();
            bullet.gameObject.SetActive(false);
            _bullets.Add(bullet);
        }
    }
    
    public Bullet SpawnBullet(BulletConfig config, Vector2 position, Vector2 direction)
    {
        var bullet = _bullets.Find(b => !b.gameObject.activeSelf);
        if (bullet == null)
        {
            throw new Exception("Max bullet count reached");
        }
        
        bullet.gameObject.SetActive(true);
        bullet.Initialize(config, position, direction);
        bullet.LifeLeft = config.lifeTime;
        return bullet;
    }
    
    public void DestroyBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
}