using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletConfig", menuName = "Bullet Config")]
public class BulletConfig : ScriptableObject
{
    public float speed;
    public int damage;
    public float size;
    public float lifeTime;
    [PreviewField] public Sprite sprite;

    [Header("Movement")] 
    public float spiralSpeed;
    
    [Header("Effect")]
    public bool hasEffect;
    public Effect effect;
    public float effectDuration;
    public bool penetrating;
    public float chargeSpawnTime;
    public float gravity;
    public bool spawnAcidPool;
    
    // copy constructor
    public BulletConfig(BulletConfig config)
    {
        speed = config.speed;
        damage = config.damage;
        size = config.size;
        lifeTime = config.lifeTime;
        sprite = config.sprite;
        spiralSpeed = config.spiralSpeed;
        hasEffect = config.hasEffect;
        effect = config.effect;
        effectDuration = config.effectDuration;
        penetrating = config.penetrating;
        chargeSpawnTime = config.chargeSpawnTime;
        gravity = config.gravity;
        spawnAcidPool = config.spawnAcidPool;
    }
}