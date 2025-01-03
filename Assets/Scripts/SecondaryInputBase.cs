using UnityEngine;
using UnityEngine.InputSystem;

public abstract class SecondaryInputBase : MonoBehaviour
{
    private InputAction _secondaryAction;
    
    protected virtual void Awake()
    {
        _secondaryAction = InputSystem.actions.FindAction("Secondary");
    }

    protected abstract void OnSecondaryActionStart();
    
    protected abstract void OnSecondaryActionEnd();
    
    protected virtual void Update()
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