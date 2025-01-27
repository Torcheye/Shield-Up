using System;
using System.Collections.Generic;
using UnityEngine;

public class BossStateManager : MonoBehaviour
{
    public List<BossMoveController> bosses;
    
    [SerializeField] private BossType startBoss;
    [SerializeField] private bool enableStartBoss;
    
    [Header("Surrounding Movement")]
    [SerializeField] private BossMoveController surroundingTarget;
    [SerializeField] private Transform surroundingPivot;
    [SerializeField] private float surroundingMoveSpeed;
    [SerializeField] private float surroundingRadius;
    private List<BossMoveController> _surroundingBosses = new List<BossMoveController>();

    private void Start()
    {
        _surroundingBosses.Clear();
        
        foreach (var boss in bosses)
        {
            if (enableStartBoss)
                boss.IsActive = boss.Type == startBoss;
            else 
                boss.IsActive = false;
            if (!boss.IsActive && boss != surroundingTarget)
            {
                AddToSurroundingBosses(boss);
            }
        }
    }

    private void Update()
    {
        surroundingPivot.Rotate(0, 0, surroundingMoveSpeed * Time.deltaTime);
    }

    public void AddToSurroundingBosses(BossMoveController boss)
    {
        _surroundingBosses.Add(boss);
        UpdateSurroundingBossPositions();
    }
    
    public void RemoveFromSurroundingBosses(BossMoveController boss)
    {
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