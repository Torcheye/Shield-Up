using UnityEngine;
using UnityEngine.InputSystem;

public enum SkillType
{
    Primary,
    Secondary,
    Passive
}

public class SkillBase : MonoBehaviour
{
    public SkillType skillType;
    
    protected PlayerController playerController;
    
    private InputAction _primaryAction;
    private InputAction _secondaryAction;
    
    public void Initialize(PlayerController pc)
    { 
        playerController = pc;
    }
    
    protected virtual void Awake()
    {
        _primaryAction = InputSystem.actions.FindAction("Primary");
        _secondaryAction = InputSystem.actions.FindAction("Secondary");
    }

    protected virtual void OnPrimaryActionStart(){}
    
    protected virtual void OnPrimaryActionEnd(){}
    
    protected virtual void OnSecondaryActionStart(){}
    
    protected virtual void OnSecondaryActionEnd(){}
    
    protected virtual void OnUpdate(float deltaTime){}
    
    protected virtual void Update()
    {
        if (skillType == SkillType.Primary)
        {
            UpdatePrimaryAction();
        }
        else if (skillType == SkillType.Secondary)
        {
            UpdateSecondaryAction();
        }
        
        OnUpdate(Time.deltaTime);
    }
    
    private void UpdatePrimaryAction()
    {
        if (_primaryAction.WasPressedThisFrame())
        {
            OnPrimaryActionStart();
        }
        else if (_primaryAction.WasReleasedThisFrame())
        {
            OnPrimaryActionEnd();
        }
    }
    
    private void UpdateSecondaryAction()
    {
        if (_secondaryAction.WasPressedThisFrame())
        {
            OnSecondaryActionStart();
        }
        else if (_secondaryAction.WasReleasedThisFrame())
        {
            OnSecondaryActionEnd();
        }
    }
}