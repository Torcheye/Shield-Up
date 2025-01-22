using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] private GameObject line;
    [SerializeField] private Transform progressLine;
    [SerializeField] private Collider2D colliderLine;
    
    [SerializeField] private Transform progressCircle;
    [SerializeField] private GameObject circle;
    [SerializeField] private Collider2D colliderCircle;
    
    private bool _dealtDamage;
    private bool _hasEffect;
    private Effect _effect;
    private float _effectDuration;
    private int _damage;
    private bool _doDamage;

    private void Awake()
    {
        progressCircle.localScale = Vector3.zero;
        line.SetActive(false);
        circle.SetActive(false);
        colliderCircle.enabled = false;
        colliderLine.enabled = false;
    }
    
    public void InitializeLineNoDamage(float warningTime, float duration, Vector2 direction, float length, float width)
    {
        StartCoroutine(DoInitializeLineNoDamage(warningTime, duration, direction, length, width));
    }
    
    public void InitializeLineDamage(float warningTime, float duration, Vector2 direction, float length, float width, int damage, bool hasEffect, Effect effect, float effectDuration)
    {
        StartCoroutine(DoInitializeLineDamage(warningTime, duration, direction, length, width, damage, hasEffect, effect, effectDuration));
    }
    
    public void InitializeCircle(bool doDamage, float warningTime, float duration, float radius, int damage, bool hasEffect, Effect effect, float effectDuration)
    {
        StartCoroutine(DoInitializeCircle(doDamage, warningTime, duration, radius, damage, hasEffect, effect, effectDuration));
    }
    
    public IEnumerator DoInitializeLineNoDamage(float warningTime, float duration, Vector2 direction, float length, float width)
    {
        _doDamage = false;
        
        line.SetActive(true);
        circle.SetActive(false);
        colliderLine.enabled = false;

        line.transform.localScale = new Vector3(length, width / 2, 1);
        line.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, direction));
        progressLine.localScale = new Vector3(1, 0, 1);
        progressLine.DOScaleY(1, warningTime).SetEase(Ease.Linear);

        yield return new WaitForSeconds(warningTime);
        
        colliderLine.enabled = true;
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
    
    public IEnumerator DoInitializeLineDamage(float warningTime, float duration, Vector2 direction, float length, float width, int damage, bool hasEffect, Effect effect, float effectDuration)
    {
        _doDamage = true;
        _hasEffect = hasEffect;
        _effect = effect;
        _effectDuration = effectDuration;
        _damage = damage;
        
        line.SetActive(true);
        circle.SetActive(false);
        colliderLine.enabled = false;

        line.transform.localScale = new Vector3(length, width / 2, 1);
        line.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, direction));
        progressLine.localScale = new Vector3(1, 0, 1);
        progressLine.DOScaleY(1, warningTime).SetEase(Ease.Linear);

        yield return new WaitForSeconds(warningTime);
        
        colliderLine.enabled = true;
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }

    public IEnumerator DoInitializeCircle(bool doDamage, float warningTime, float duration, float radius, int damage, bool hasEffect, Effect effect, float effectDuration)
    {
        _doDamage = doDamage;
        _hasEffect = hasEffect;
        _effect = effect;
        _effectDuration = effectDuration;
        _damage = damage;
        
        circle.SetActive(true);
        line.SetActive(false);
        colliderCircle.enabled = false;

        circle.transform.localScale = radius * Vector3.one;
        progressCircle.localScale = Vector3.zero;
        progressCircle.DOScale(Vector3.one, warningTime).SetEase(Ease.Linear);

        yield return new WaitForSeconds(warningTime);
        
        colliderCircle.enabled = true;
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Damageable"))
        {
            if (_dealtDamage || !_doDamage) return;
            
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
            if (!_doDamage) return;
            
            var groundBlockParent = other.transform.parent;
            var groundBlock = groundBlockParent.GetComponent<GroundBlock>();
            groundBlock.TakeHit();
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}