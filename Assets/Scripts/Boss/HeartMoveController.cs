using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartMoveController : BossMoveController
{
    private enum State
    {
        Normal,
        ReturnToCenter,
        Regen
    }
    
    [SerializeField] private Bounds normalMoveBounds;
    [SerializeField] private float switchYLine;

    [SerializeField] private int regenCount;
    [SerializeField] private float regenInterval;
    [SerializeField] private float postRegenTime;
    [SerializeField] private Damageable bossDamageable;
    [SerializeField] private BossStateManager bossStateManager;

    private Vector2 _target;
    private State _state = State.Normal;

    public bool CanTakeDamage()
    {
        return _state == State.Normal;
    }

    private void Awake()
    {
        IsActive = true;
    }

    public void OnHpIsZero()
    {
        bossStateManager.IncreaseNextBossActiveCountAndResetTimer();
        SwitchState(State.ReturnToCenter);
    }

    private IEnumerator DoRegen()
    {
        var regenAmount = DataManager.Instance.bossConfig.GetBossHp(BossType.Heart) / regenCount;
        for (var i = 0; i < regenCount; i++)
        {
            bossDamageable.Heal(regenAmount);
            yield return new WaitForSeconds(regenInterval);
        }
        
        yield return new WaitForSeconds(postRegenTime);
        SwitchState(State.Normal);
        bossStateManager.ResumeBossRotation();
    }

    private void Update()
    {
        if (_state == State.Normal)
        {
            var playerPos = DataManager.Instance.playerTransform.position;
            _target = transform.position;
            _target.y = playerPos.y > switchYLine ? normalMoveBounds.min.y : normalMoveBounds.max.y;
            _target.x = playerPos.x > normalMoveBounds.center.x ? normalMoveBounds.min.x : normalMoveBounds.max.x;
            if (!normalMoveBounds.Contains(transform.position))
                _target = normalMoveBounds.center;
        }
        else if (_state == State.ReturnToCenter)
        {
            if ((transform.position - normalMoveBounds.center).sqrMagnitude < 0.1f)
            {
                SwitchState(State.Regen);
            }
        }
        
        transform.position = Vector2.MoveTowards(transform.position, _target, moveSpeed * Time.deltaTime);
    }
    
    private void SwitchState(State state)
    {
        Debug.Log($"Switch state to {state}");
        _state = state;
        switch (_state)
        {
            case State.Normal:
                break;
            case State.ReturnToCenter:
                _target = normalMoveBounds.center;
                if ((transform.position - normalMoveBounds.center).sqrMagnitude < 0.1f)
                {
                    _state = State.Regen;
                }
                break;
            case State.Regen:
                _target = normalMoveBounds.center;
                StartCoroutine(DoRegen());
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(normalMoveBounds.center, normalMoveBounds.size);
    }
#endif
}