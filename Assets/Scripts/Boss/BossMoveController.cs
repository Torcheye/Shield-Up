using System.Collections;
using DG.Tweening;
using UnityEngine;

public class BossMoveController : MonoBehaviour
{
    [SerializeField] private BossType bossType;
    [SerializeField] private float moveInterval = 3f;
    
    private float _moveSpeed;
    private Bounds _moveBounds;
    
    private void Start()
    {
        var bossConfig = DataManager.Instance.bossConfig;
        if (!bossConfig.GetBossEnable(bossType))
        {
            gameObject.SetActive(false);
        }
        
        _moveSpeed = bossConfig.GetBossMoveSpeed(bossType);
        _moveBounds = DataManager.Instance.bossMoveBounds;

        StartCoroutine(DoLoop());
    }
    
    private IEnumerator DoLoop()
    {
        while (gameObject.activeInHierarchy)
        {
            var targetPosition = GetRandomPositionWithinBounds(out var distance);
            var duration = distance / _moveSpeed;
            transform.DOMove(targetPosition, duration);
            yield return new WaitForSeconds(duration);
            
            yield return new WaitForSeconds(moveInterval);
        }
    }
    
    private Vector2 GetRandomPositionWithinBounds(out float distance)
    {
        var randomX = Random.Range(_moveBounds.min.x, _moveBounds.max.x);
        var randomY = Random.Range(_moveBounds.min.y, _moveBounds.max.y);
        var targetPosition = new Vector2(randomX, randomY);
        distance = Vector2.Distance(transform.position, targetPosition);
        return targetPosition;
    }
}