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

    [SerializeField] private BossType bossType;
    public BossType Type => bossType;
    [SerializeField] private Renderer rend;
    [SerializeField] private BossHpBar bossHpBar;
    
    protected float moveSpeed;
    private bool _isActive;
    
    private static readonly int FillPhase = Shader.PropertyToID("_FillPhase");
    private static readonly int FillColor = Shader.PropertyToID("_FillColor");

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
        rend.material.SetColor(FillColor, IsActive ? Color.white : DataManager.Instance.bossInactiveColor);
        rend.material.SetFloat(FillPhase, IsActive ? 0 : DataManager.Instance.bossInactiveFillAmount);
        rend.sortingLayerName = IsActive ? "Enemy" : "Background";
        DoMove = IsActive;
        bossHpBar.gameObject.SetActive(IsActive);
    }
}