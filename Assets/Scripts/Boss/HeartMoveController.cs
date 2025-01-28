using UnityEngine;

public class HeartMoveController : BossMoveController
{
    [SerializeField] private Bounds normalMoveBounds;
    [SerializeField] private float switchYLine;
    
    private void Update()
    {
        var playerPos = DataManager.Instance.playerTransform.position;
        Vector2 target = transform.position;
        target.y = playerPos.y > switchYLine ? normalMoveBounds.min.y : normalMoveBounds.max.y;
        target.x = playerPos.x > normalMoveBounds.center.x ? normalMoveBounds.min.x : normalMoveBounds.max.x;
        if (!normalMoveBounds.Contains(transform.position))
            target = normalMoveBounds.center;
        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(normalMoveBounds.center, normalMoveBounds.size);
    }
#endif
}