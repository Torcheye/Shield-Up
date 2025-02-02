using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossStateManager : MonoBehaviour
{
    public List<BossMoveController> aliveBosses;
    
    [Header("Surrounding Movement")]
    [SerializeField] private BossMoveController heart;
    [SerializeField] private Transform surroundingPivot;
    [SerializeField] private float surroundingMoveSpeed;
    [SerializeField] private float surroundingRadius;
    private readonly List<BossMoveController> _surroundingBosses = new();

    private float _bossRotationTimer = 0;
    private int _nextBossRotationIndex = 0;
    private int _nextBossActiveCount = 1;
    private bool _enableRotationTimer = true;
    
    public void RemoveBoss(BossMoveController boss)
    {
        aliveBosses.Remove(boss);
        RemoveFromSurroundingBosses(boss);
        
        if (aliveBosses.Count == 0)
        {
            UIManager.Instance.ShowGameSuccessScreen();
        }
    }
    
    private IEnumerator Start()
    {
        _surroundingBosses.Clear();
        SetBossActive(new List<BossType>() { DataManager.Instance.bossRotationOrderFTUE[0] });
        yield return null;
        SetBossActive(new List<BossType>() { DataManager.Instance.bossRotationOrderFTUE[0] });
        _nextBossRotationIndex = 1;
    }

    /// returns whether the max boss count is reached
    public bool IncreaseNextBossActiveCountAndResetTimer()
    {
        if (_nextBossActiveCount < aliveBosses.Count)
        {
            _nextBossActiveCount++;
        }
        else
        {
            return true;
        }
        _enableRotationTimer = false;
        _bossRotationTimer = 0;
        SetBossActive(new List<BossType>());
        return false;
    }

    public void ResumeBossRotation()
    {
        _enableRotationTimer = true;
        _bossRotationTimer = DataManager.Instance.bossRotationTime;
    }
    
    private void SetBossActive(IList<BossType> activeBosses)
    {
        foreach (var boss in aliveBosses)
        {
            if (activeBosses.Contains(boss.Type))
            {
                boss.IsActive = true;
                RemoveFromSurroundingBosses(boss);
            }
            else
            {
                boss.IsActive = false;
                AddToSurroundingBosses(boss);
            }
        }
    }

    private IList<BossType> GetNextRotationBosses()
    {
        var activeBosses = aliveBosses.FindAll(boss => boss.IsActive).Select(boss => boss.Type).ToList();
        var inactiveBosses = aliveBosses.FindAll(boss => !boss.IsActive).Select(boss => boss.Type).ToList();
        var nextBosses = new List<BossType>();

        for (int i = 0; i < _nextBossActiveCount; i++)
        {
            if (inactiveBosses.Count > 0)
            {
                var nextBoss = inactiveBosses[Random.Range(0, inactiveBosses.Count)];
                nextBosses.Add(nextBoss);
                inactiveBosses.Remove(nextBoss);
            }
            else
            {
                var nextBoss = activeBosses[Random.Range(0, activeBosses.Count)];
                nextBosses.Add(nextBoss);
                activeBosses.Remove(nextBoss);
            }
        }
        
        return nextBosses;
    }

    private void Update()
    {
        surroundingPivot.Rotate(0, 0, surroundingMoveSpeed * Time.deltaTime);
        UpdateSurroundingBossPositions();

        if (!_enableRotationTimer)
            return;
        
        if (_bossRotationTimer < DataManager.Instance.bossRotationTime)
        {
            _bossRotationTimer += Time.deltaTime;
        }
        else
        {
            if (_nextBossActiveCount == 1)
            {
                var nextBossType = DataManager.Instance.bossRotationOrderFTUE[_nextBossRotationIndex];
                SetBossActive(new List<BossType> { nextBossType });
                _nextBossRotationIndex = (_nextBossRotationIndex + 1) % DataManager.Instance.bossRotationOrderFTUE.Count;
            }
            else
            {
                SetBossActive(GetNextRotationBosses());
            }
            _bossRotationTimer = 0;
        }
    }

    private void AddToSurroundingBosses(BossMoveController boss)
    {
        if (_surroundingBosses.Contains(boss))
            return;
        
        _surroundingBosses.Add(boss);
    }
    
    private void RemoveFromSurroundingBosses(BossMoveController boss)
    {
        if (!_surroundingBosses.Contains(boss))
            return;
        
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