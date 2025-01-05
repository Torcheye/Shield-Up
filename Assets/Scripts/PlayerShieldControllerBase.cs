using UnityEngine;

public class PlayerShieldControllerBase : MonoBehaviour
{
    public Transform shieldPivot;
    public DeflectCollider deflectCollider;

    private void Update()
    {
        Rotate();
        
        deflectCollider.normal = shieldPivot.right;
    }

    private void Rotate()
    {
        var mouseWorldPosition = GeneralInput.Instance.GetMousePosition();
        
        var direction = (mouseWorldPosition - shieldPivot.position).normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        shieldPivot.rotation = Quaternion.Euler(0, 0, angle);
    }
}