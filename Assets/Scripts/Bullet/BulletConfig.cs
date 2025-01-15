﻿using UnityEngine;

[CreateAssetMenu(fileName = "BulletConfig", menuName = "Bullet Config")]
public class BulletConfig : ScriptableObject
{
    public float speed;
    public int damage;
    public float size;
    public float lifeTime;
    public Color color;
    
    [Header("Effect")]
    public bool hasEffect;
    public Effect effect;
    public float effectDuration;
    public bool penetrating;
    public float chargeSpawnTime;
    public float gravity;
    public bool spawnAcidPool;
}