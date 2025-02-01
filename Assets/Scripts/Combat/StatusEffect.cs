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

    [Header("Bleed"), ShowIf(nameof(_isBoss))] 
    [SerializeField] private Damageable bossDamageable;
    [SerializeField, ShowIf(nameof(_isBoss))]
    private ParticleSystem bleedParticle;
    
    private readonly Dictionary<Effect, EffectTimer> _effects = new();
    private Camera _camera;
    private bool _copyActivated;
    private float _bleedTimer;
    private bool _isBoss => !isPlayer;

    private void Awake()
    {
        _camera = Camera.main;
        _effects.Clear();
        foreach (Effect effect in Enum.GetValues(typeof(Effect)))
        {
            _effects[effect] = new EffectTimer();
        }

        if (_isBoss)
        {
            bleedParticle.Stop();
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
            else
            {
                if (effect.Key == Effect.Bleed)
                {
                    UpdateBleed(effect.Value);
                }
            }
        }
    }

    private void UpdateBleed(EffectTimer timer)
    {
        if (timer.IsAlive())
        {
            _bleedTimer += Time.deltaTime;
            if (_bleedTimer >= DataManager.Instance.bleedHurtInterval)
            {
                bossDamageable.TakeDamage(DataManager.Instance.bleedDamage, false);
                _bleedTimer = 0;
            }
            
            if (!bleedParticle.isPlaying)
            {
                bleedParticle.Play();
            }
        }
        else
        {
            if (bleedParticle.isPlaying)
            {
                bleedParticle.Stop();
            }
        }
    }
    
    private void UpdatePlayerStatusEffect(Effect effect, EffectTimer timer)
    {
        if (effect == Effect.Bleed)
            return;
        
        var progress = timer.GetProgress();
        UIManager.Instance.UpdatePlayerStatusEffects((int) effect, progress);
                
        switch (effect)
        {
            case Effect.Blind:
                var screenPos = _camera.WorldToScreenPoint(transform.position) / new Vector2(Screen.width, Screen.height);
                UIManager.Instance.UpdateBlindEffect(screenPos, progress);
                break;
            case Effect.Copycat when !_copyActivated && timer.IsAlive():
                copySourceRing.CopyOver(copyTargetRing);
                _copyActivated = true;
                break;
            case Effect.Copycat:
                if (_copyActivated && !timer.IsAlive())
                {
                    copyTargetRing.ClearAll();
                    _copyActivated = false;
                }
                break;
            case Effect.Slow:
                playerController.SetSlowMultiplier(progress > 0);
                break;
            case Effect.Invulnerable:
                break;
            case Effect.Dizzy:
                playerController.SetDizzyMultiplier(progress > 0);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(effect), effect, null);
        }
    }

    public void ApplyEffect(Effect effect, float duration)
    {
        _effects[effect].Start(duration);
        
        if (effect == Effect.Bleed)
        {
            _bleedTimer = 0;
        }
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