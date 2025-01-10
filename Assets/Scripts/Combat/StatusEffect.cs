using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    private readonly Dictionary<Effect, EffectTimer> _effects = new();

    private void Awake()
    {
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
            UIManager.Instance.UpdatePlayerStatusEffects((int) effect.Key, effect.Value.GetProgress());
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