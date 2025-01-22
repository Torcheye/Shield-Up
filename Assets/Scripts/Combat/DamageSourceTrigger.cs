using UnityEngine;

public class DamageSourceTrigger : MonoBehaviour
{
    private bool _hasEffect;
    private Effect _effect;
    private float _effectDuration;
    private int _damage;
    
    public void Initialize(int damage, bool hasEffect, Effect effect, float effectDuration, float radius)
    {
        _damage = damage;
        _hasEffect = hasEffect;
        _effect = effect;
        _effectDuration = effectDuration;
        transform.localScale = new Vector3(radius, radius, 1);
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Damageable"))
        {
            if (!enabled) return;
            
            var damageable = other.GetComponentInParent<Damageable>();
            bool hit = damageable.TakeDamage(_damage, true);
            if (hit && _hasEffect)
            {
                if (_hasEffect)
                    damageable.ApplyEffect(_effect, _effectDuration);
            }
        }
    }
}