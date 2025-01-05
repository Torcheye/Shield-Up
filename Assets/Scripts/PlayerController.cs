using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform playerTransform;
    public PlayerShieldControllerBase shieldController;
    
    public bool IsInvincible
    {
        get => _isInvincible;
        set => OnSetInvincible(value);
    }
    
    private bool _isInvincible;

    private void OnEnable()
    {
        foreach (var skill in GetComponentsInChildren<SkillBase>())
        {
            skill.Initialize(this);
        }
    }
    
    private void OnSetInvincible(bool value)
    {
        _isInvincible = value;
        // TODO: Implement invincibility effect
    }
}