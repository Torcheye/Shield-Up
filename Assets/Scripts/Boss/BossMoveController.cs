using UnityEngine;

public class BossMoveController : MonoBehaviour
{
    public bool DoMove { get; set; } = true;

    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
            OnSetIsActive();
        }
    }

    public bool CanSetInactive { get; set; } = true;

    [SerializeField] private BossType bossType;
    public BossType Type => bossType;
    [SerializeField] private Renderer rend;
    [SerializeField] private BossHpBar bossHpBar;
    
    protected float moveSpeed;
    private bool _isActive;

    protected virtual void Start()
    {
        var bossConfig = DataManager.Instance.bossConfig;
        
        moveSpeed = bossConfig.GetBossMoveSpeed(bossType);
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }

    protected virtual void OnSetIsActive()
    {
        if (!IsActive)
        {
            rend.material.EnableKeyword("GHOST_ON");
        }
        else
        {
            rend.material.DisableKeyword("GHOST_ON");
        }
        rend.sortingLayerName = IsActive ? "Enemy" : "Background";
        DoMove = IsActive;
        bossHpBar.gameObject.SetActive(IsActive);
    }
}