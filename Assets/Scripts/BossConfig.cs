using UnityEngine;

[CreateAssetMenu(fileName = "BossConfig", menuName = "Boss Config")]
public class BossConfig : ScriptableObject
{
    [Header("Worm")]
    public float wormSpeed;
    public int wormBodyHp;
    public float wormBodyShootingInterval;
} 