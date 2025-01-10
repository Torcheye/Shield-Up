using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    private readonly List<Effect> _effects = new List<Effect>();
    
    public List<Effect> Effects => _effects;
    
    public void AddEffect(Effect effect, float duration)
    {
        if (_effects.Contains(effect))
        {
            Debug.LogWarning($"Effect {effect} already exists");
            return;
        }
        _effects.Add(effect);
        StartCoroutine(RemoveEffectAfterDuration(effect, duration));
    }
    
    private IEnumerator RemoveEffectAfterDuration(Effect effect, float duration)
    {
        yield return new WaitForSeconds(duration);
        RemoveEffect(effect);
    }
    
    public void RemoveEffect(Effect effect)
    {
        if (!_effects.Contains(effect))
        {
            Debug.LogWarning($"Effect {effect} does not exist");
            return;
        }
        _effects.Remove(effect);
    }
    
    public bool HasEffect(Effect effect)
    {
        return _effects.Contains(effect);
    }
}