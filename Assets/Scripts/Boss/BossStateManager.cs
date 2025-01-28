using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BossStateManager : MonoBehaviour
{
    public List<BossMoveController> bosses;
    
    [SerializeField] private BossType startBoss;
    
    [Header("Surrounding Movement")]
    [SerializeField] private BossMoveController surroundingTarget;
    [SerializeField] private Transform surroundingPivot;
    [SerializeField] private float surroundingMoveSpeed;
    [SerializeField] private float surroundingRadius;
    private readonly List<BossMoveController> _surroundingBosses = new();

    private void Start()
    {
        _surroundingBosses.Clear();
        
        SetBossActive(startBoss);
    }
    
    [Button]
    public void SetBossActive(BossType activeBoss)
    {
        foreach (var boss in bosses)
        {
            if (boss.Type == activeBoss)
            {
                boss.IsActive = true;
                RemoveFromSurroundingBosses(boss);
            }
            else
            {
                boss.IsActive = false;
                if (boss != surroundingTarget)
                {
                    AddToSurroundingBosses(boss);
                }
            }
        }
    }

    private void Update()
    {
        surroundingPivot.Rotate(0, 0, surroundingMoveSpeed * Time.deltaTime);
    }

    private void AddToSurroundingBosses(BossMoveController boss)
    {
        if (_surroundingBosses.Contains(boss))
        {
            Debug.Log($"{boss} is already in surrounding bosses");
            return;
        }
        _surroundingBosses.Add(boss);
        UpdateSurroundingBossPositions();
    }
    
    private void RemoveFromSurroundingBosses(BossMoveController boss)
    {
        if (!_surroundingBosses.Contains(boss))
        {
            Debug.Log($"{boss} is not in surrounding bosses");
            return;
        }
        _surroundingBosses.Remove(boss);
        boss.transform.SetParent(null);
        UpdateSurroundingBossPositions();
    }
    
    private void UpdateSurroundingBossPositions()
    {
        for (int i = 0; i < _surroundingBosses.Count; i++)
        {
            var angle = Mathf.PI * 2 / _surroundingBosses.Count * i;
            var x = Mathf.Cos(angle) * surroundingRadius;
            var y = Mathf.Sin(angle) * surroundingRadius;
            var targetPos = new Vector3(x, y, 0);
            _surroundingBosses[i].transform.SetParent(surroundingPivot);
            _surroundingBosses[i].transform.localPosition = targetPos;
        }
    }
}