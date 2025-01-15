using System;
using UnityEngine;

public class MouthMoveController : BossMoveController
{
    private void Update()
    {
        if (doMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                DataManager.Instance.playerTransform.position, moveSpeed * Time.deltaTime);
        }
    }
}