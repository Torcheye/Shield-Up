using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShieldControllerBase : MonoBehaviour
{
    public Transform shieldPivot;
    
    private Camera _mainCamera;

    protected virtual void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        var mousePosition = Pointer.current.position.ReadValue();
        var mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);
        
        var direction = (mouseWorldPosition - shieldPivot.position).normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        shieldPivot.rotation = Quaternion.Euler(0, 0, angle);
    }
}