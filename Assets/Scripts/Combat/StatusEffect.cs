using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    [SerializeField] private bool isPlayer;
    
    [Header("Slow"), ShowIf(nameof(isPlayer))]
    [SerializeField] private PlayerController playerController;

    [Header("Copycat"), ShowIf(nameof(isPlayer))] 
    [SerializeField] private RingController copySourceRing;
    [ShowIf(nameof(isPlayer))]
    [SerializeField] private RingController copyTargetRing;
    
    private readonly Dictionary<Effect, EffectTimer> _effects = new();
    private Camera _camera;
    private bool _copyActivated;

    private void Awake()
    {
        _camera = Camera.main;
        _effects.Clear();
        foreach (Effect effect in Enum.GetValues(typeof(Effect)))
        {
            _effects[effect] = new EffectTimer();
        }
    }

    private void Update()
    {
        foreach (var effect in _effects)
        {
            if (effect.Value.IsAlive())
            {
                effect.Value.TimeLeft -= Time.deltaTime;
            }
            else
            {
                effect.Value.TimeLeft = 0;
            }
            
            if (isPlayer)
            {
                UpdatePlayerStatusEffect(effect.Key, effect.Value);
            }
        }
    }
    
    private void UpdatePlayerStatusEffect(Effect effect, EffectTimer timer)
    {
        UIManager.Instance.UpdatePlayerStatusEffects((int) effect, timer.GetProgress());
                
        switch (effect)
        {
            case Effect.Blind:
            {
                var screenPos = _camera.WorldToScreenPoint(transform.position) / new Vector2(Screen.width, Screen.height);
                UIManager.Instance.UpdateBlindEffect(screenPos, timer.GetProgress());
                break;
            }
            case Effect.Copycat when !_copyActivated && timer.IsAlive():
                copySourceRing.CopyOver(copyTargetRing);
                _copyActivated = true;
                break;
            case Effect.Copycat:
            {
                if (_copyActivated && !timer.IsAlive())
                {
                    copyTargetRing.ClearAll();
                    _copyActivated = false;
                }

                break;
            }
            case Effect.Slow:
                playerController.SetSlowMultiplier(timer.GetProgress() > 0);
                break;
            case Effect.Invulnerable:
                break;
            case Effect.Dizzy:
                playerController.SetDizzyMultiplier(timer.GetProgress() > 0);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(effect), effect, null);
        }
    }

    public void ApplyEffect(Effect effect, float duration)
    {
        _effects[effect].Start(duration);
    }
    
    public bool HasEffect(Effect effect)
    {
        return _effects[effect].IsAlive();
    }
}

public class EffectTimer
{
    public float TimeLeft;
    public float Duration;
    
    public void Start(float duration)
    {
        TimeLeft = duration;
        Duration = duration;
    }
    
    public bool IsAlive()
    {
        return TimeLeft > 0;
    }
    
    public float GetProgress()
    {
        if (Duration == 0)
        {
            return 0;
        }
        return TimeLeft / Duration;
    }
}