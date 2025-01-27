using UnityEngine;

public class MouthMoveController : BossMoveController
{
    private void Update()
    {
        if (DoMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                DataManager.Instance.playerTransform.position, moveSpeed * Time.deltaTime);
        }
    }
}