using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int Level { get; set; }
    public bool IsHostile {get; set; }
    public WeaponType Type { get; set; }
    
    protected RingController ringController;
    protected PlayerDamageable playerDamageable;
    protected Vector2Int slotIndex;
    
    [SerializeField] private TrailRenderer trailRenderer;
    
    public void Initialize(bool isHostile, WeaponType type, RingController rc, PlayerDamageable pd, Vector2Int slotIndex)
    {
        ringController = rc;
        playerDamageable = pd;
        
        Level = 1;
        IsHostile = isHostile;
        Type = type;
        this.slotIndex = slotIndex;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
    
    public void ClearTrail()
    {
        trailRenderer.Clear();
    }
}