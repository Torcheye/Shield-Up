using UnityEngine;

[CreateAssetMenu(fileName = "BulletConfig", menuName = "Bullet Config")]
public class BulletConfig : ScriptableObject
{
    public float speed;
    public float damage;
    public float size;
    public int bounceLeft;
    public float lifeTime;
}