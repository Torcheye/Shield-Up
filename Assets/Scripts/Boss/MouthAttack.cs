﻿using System;
using System.Collections;
using UnityEngine;

public class MouthAttack : BossAttack
{
    [SerializeField] private BulletConfig acidBullet;
    [SerializeField] private Vector2 normalLaunchPower;
    [SerializeField] private int burstAmount;
    [SerializeField] private float burstRandomAngle;
    [SerializeField] private float burstRandomPower;
    [SerializeField] private float burstUpPower;
    [SerializeField] private float burstInterval;
    
    [Header("Suction")]
    [SerializeField] private float suctionPower;
    [SerializeField] private float suctionDuration;
    [SerializeField] private float suctionRampUpTime;
    [SerializeField] private MouthSuctionTrigger suctionTrigger;
    [SerializeField] private ParticleSystem suctionParticles;

    private bool _isSucking;
    private float _suctionTimer;

    protected override void Attack()
    {
        var playerPos = DataManager.Instance.playerTransform.position;
        var x = (playerPos.x - transform.position.x) * normalLaunchPower.x;
        var y = (playerPos.y - transform.position.y) * normalLaunchPower.y;
        var dir = new Vector2(x, y);
        BulletFactory.Instance.SpawnBullet(acidBullet, transform.position, dir, true, transform);
    }

    protected override void EnhancedAttack()
    {
        StartCoroutine(DoSuction());
    }
    
    private IEnumerator DoSuction()
    {
        _isSucking = true;
        suctionParticles.Play();
        yield return new WaitForSeconds(suctionDuration);
        _isSucking = false;
        suctionParticles.Stop();
        
        StartCoroutine(ShootBulletBurst());
    }
    
    private IEnumerator ShootBulletBurst()
    {
        for (int i = 0; i < burstAmount; i++)
        {
            var randomAngle = UnityEngine.Random.Range(-burstRandomAngle, burstRandomAngle);
            Vector2 randomDir = Quaternion.Euler(0, 0, randomAngle) * Vector3.up * burstUpPower;
            randomDir += new Vector2(UnityEngine.Random.Range(-burstRandomPower, burstRandomPower), UnityEngine.Random.Range(-burstRandomPower, burstRandomPower));
            BulletFactory.Instance.SpawnBullet(acidBullet, transform.position, randomDir, true, transform);
            yield return new WaitForSeconds(burstInterval);
        }
    }

    private void Update()
    {
        if (_isSucking)
        {
            _suctionTimer += Time.deltaTime;
        }
        else
        {
            _suctionTimer = 0;
        }
    }

    private void FixedUpdate()
    {
        if (_isSucking)
        {
            foreach (var rb in suctionTrigger.ObjectsInTrigger)
            {
                var suctionDir = ((Vector2)transform.position - rb.position).normalized;
                var rampUp = Mathf.Clamp01(_suctionTimer / suctionRampUpTime);
                rb.AddForce(rampUp * suctionPower * rb.gravityScale * suctionDir);
            }
        }
    }
}