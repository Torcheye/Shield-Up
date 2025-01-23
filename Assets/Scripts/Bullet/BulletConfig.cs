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
}