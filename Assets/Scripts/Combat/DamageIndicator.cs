using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    
    [SerializeField] private Transform progress;
    [SerializeField] private Collider2D col;
    
    private bool _dealtDamage;
    private bool _hasEffect;
    private Effect _effect;
    private float _effectDuration;
    private int _damage;

    private void Awake()
    {
        progress.localScale = Vector3.zero;
        col.enabled = false;
    }

    public IEnumerator Initialize(Vector2 pos, float warningTime, float duration, float radius, int damage, bool hasEffect, Effect effect, float effectDuration)
    {
        _hasEffect = hasEffect;
        _effect = effect;
        _effectDuration = effectDuration;
        _damage = damage;
        transform.position = pos;
        col.enabled = false;
        transform.localScale = radius * Vector3.one;
        progress.localScale = Vector3.zero;
        progress.DOScale(Vector3.one, warningTime).SetEase(Ease.Linear);
        yield return new WaitForSeconds(warningTime);
        
        col.enabled = true;
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Damageable"))
        {
            if (_dealtDamage || !col.enabled) return;
            
            var damageable = other.GetComponentInParent<Damageable>();
            bool hit = damageable.TakeDamage(_damage, true);
            if (hit && _hasEffect)
            {
                if (_hasEffect)
                    damageable.ApplyEffect(_effect, _effectDuration);
                _dealtDamage = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (!col.enabled) return;
            
            var groundBlockParent = other.transform.parent;
            var groundBlock = groundBlockParent.GetComponent<GroundBlock>();
            groundBlock.TakeHit();
        }
    }
}