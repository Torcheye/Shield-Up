using UnityEngine;
using UnityEngine.InputSystem;

public class GeneralInput : MonoBehaviour
{
    public static GeneralInput Instance;
    
    private Camera _mainCamera;
    
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        _mainCamera = Camera.main;
    }
    
    public Vector3 GetMousePosition()
    {
        var pos = _mainCamera.ScreenToWorldPoint(Pointer.current.position.ReadValue());
        pos.z = 0;
        return pos;
    }
}