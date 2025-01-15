using System;
using UnityEngine;

public class PlayerFootTrigger : MonoBehaviour
{
    [SerializeField] private PlayerDamageable playerDamageable;
    [SerializeField] private StatusEffect playerStatusEffect;

    private float _acidStayTimer;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("AcidPool"))
        {
            _acidStayTimer += Time.deltaTime;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("AcidPool"))
        {
            _acidStayTimer = 0;
        }
    }

    private void Update()
    {
        if (playerStatusEffect.HasEffect(Effect.Invulnerable))
        {
            _acidStayTimer = 0;
        }
        
        if (_acidStayTimer >= DataManager.Instance.acidHurtInterval)
        {
            playerDamageable.TakeDamage(DataManager.Instance.acidDamage, true);
            _acidStayTimer = 0;
        }
    }
}