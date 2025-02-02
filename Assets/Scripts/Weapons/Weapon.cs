using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int Level 
    {
        get => _level;
        set
        {
            _level = value;
            OnLevelChange(_level);
        }
    }
    public bool IsHostile {get; set; }
    public WeaponType Type { get; set; }
    
    protected RingController ringController;
    protected PlayerDamageable playerDamageable;
    protected Vector2Int slotIndex;
    private int _level;
    
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] protected GameObject[] levelObjects;
    
    public void Initialize(bool isHostile, WeaponType type, RingController rc, PlayerDamageable pd, Vector2Int slotIndex)
    {
        ringController = rc;
        playerDamageable = pd;
        
        Level = 1;
        IsHostile = isHostile;
        Type = type;
        this.slotIndex = slotIndex;
    }
    
    private void OnLevelChange(int newLevel)
    {
        foreach (var obj in levelObjects)
        {
            obj.SetActive(false);
        }
        
        levelObjects[newLevel - 1].SetActive(true);
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