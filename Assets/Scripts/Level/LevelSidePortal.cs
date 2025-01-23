using UnityEngine;

public class LevelSidePortal : MonoBehaviour
{
    [SerializeField] private float destinationX;
    [SerializeField] private bool isRight;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Damageable")) 
            return;
        
        var playerController = other.GetComponentInParent<PlayerController>();
        
        if (playerController == null) 
            return;
        
        if (playerController.GetVelocityX() > 0 && isRight || playerController.GetVelocityX() < 0 && !isRight )
        {
            playerController.Teleport(new Vector2(destinationX, playerController.playerTransform.position.y));
        }
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(destinationX, -1000), new Vector3(destinationX, 1000));
    }
    #endif
}