using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BossStateManager : MonoBehaviour
{
    public List<BossMoveController> bosses;
    
    [Header("Surrounding Movement")]
    [SerializeField] private BossMoveController surroundingTarget;
    [SerializeField] private Transform surroundingPivot;
    [SerializeField] private float surroundingMoveSpeed;
    [SerializeField] private float surroundingRadius;
    private readonly List<BossMoveController> _surroundingBosses = new();

    private float _bossRotationTimer = 0;
    private int _nextBossRotationIndex = 0;
    
    private void Start()
    {
        _surroundingBosses.Clear();
        SetBossActive(DataManager.Instance.bossRotationOrderFTUE[_nextBossRotationIndex]);
        _nextBossRotationIndex++;
    }
    
    [Button]
    public void SetBossActive(BossType activeBoss)
    {
        Debug.Log($"Set boss active: {activeBoss}");
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
        UpdateSurroundingBossPositions();
        
        if (_bossRotationTimer < DataManager.Instance.bossRotationTime)
        {
            _bossRotationTimer += Time.deltaTime;
        }
        else
        {
            var currentActiveBoss = bosses.Find(boss => boss.IsActive);
            var canSetInactive = currentActiveBoss.CanSetInactive;
            var nextBossType = DataManager.Instance.bossRotationOrderFTUE[_nextBossRotationIndex];
            if (canSetInactive)
            {
                SetBossActive(nextBossType);
                _bossRotationTimer = 0;
                _nextBossRotationIndex = (_nextBossRotationIndex + 1) % DataManager.Instance.bossRotationOrderFTUE.Count;
            }
        }
    }

    private void AddToSurroundingBosses(BossMoveController boss)
    {
        if (_surroundingBosses.Contains(boss))
        {
            Debug.Log($"{boss} is already in surrounding bosses");
            return;
        }
        _surroundingBosses.Add(boss);
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