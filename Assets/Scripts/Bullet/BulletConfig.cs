using UnityEngine;

[CreateAssetMenu(fileName = "BulletConfig", menuName = "Bullet Config")]
public class BulletConfig : ScriptableObject
{
    public float speed;
    public int damage;
    public float size;
    public float lifeTime;
    public bool hasEffect;
    public Effect effect;
    public float effectDuration;
}