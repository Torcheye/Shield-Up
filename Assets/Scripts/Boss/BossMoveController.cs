using System;
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
    [SerializeField] protected BossDamageable bossDamageable;
    [SerializeField] private BossAttack bossAttack;
    [SerializeField] private LineRenderer vessel;
    
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
        if (bossType != BossType.Heart)
        {
            vessel.SetPosition(1, transform.position);
        }
    }

    protected virtual void OnDisable()
    {
        if (bossType != BossType.Heart)
            vessel.enabled = false;
    }

    protected virtual void OnSetIsActive()
    {
        if (!IsActive)
        {
            rend.material.EnableKeyword("GHOST_ON");
            bossDamageable.ToggleBoostMaterial(false);
            
            bossAttack.OnSetInactive();
        }
        else
        {
            rend.material.DisableKeyword("GHOST_ON");
            bossDamageable.ToggleBoostMaterial(DataManager.Instance.IsBossAttackBoostEnabled);
        }
        rend.sortingLayerName = IsActive ? "Enemy" : "Background";
        DoMove = IsActive;
        bossHpBar.gameObject.SetActive(IsActive);
    }
}