using UnityEngine;

public class DeflectCollider : MonoBehaviour
{
    public Vector2 normal;

#if UNITY_EDITOR
    public Transform center;
    
    private void OnDrawGizmos()
    {
        if (center == null)
        {
            return;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawLine(center.position, (Vector2)center.position + normal);
    }
    #endif
}