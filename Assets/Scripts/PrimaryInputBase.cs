using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PrimaryInputBase : MonoBehaviour
{
    private InputAction _primaryAction;
    
    protected virtual void Awake()
    {
        _primaryAction = InputSystem.actions.FindAction("Primary");
    }

    protected abstract void OnPrimaryActionStart();
    
    protected abstract void OnPrimaryActionEnd();
    
    protected virtual void Update()
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
}