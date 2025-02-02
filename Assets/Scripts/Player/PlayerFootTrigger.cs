using System;
using UnityEngine;

public class PlayerFootTrigger : MonoBehaviour
{
    [SerializeField] private PlayerDamageable playerDamageable;
    [SerializeField] private StatusEffect playerStatusEffect;

    private float _acidStayTimer;
    private float _mouthStayTimer;

    private void Awake()
    {
        _acidStayTimer = 0;
        _mouthStayTimer = 0;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("AcidPool"))
        {
            _acidStayTimer += Time.deltaTime;
        }
        
        if (other.CompareTag("MouthEnhancedAttack"))
        {
            _mouthStayTimer += Time.deltaTime;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("AcidPool"))
        {
            _acidStayTimer = 0;
        }
        
        if (other.CompareTag("MouthEnhancedAttack"))
        {
            _mouthStayTimer = 0;
        }
    }

    private void Update()
    {
        if (playerStatusEffect.HasEffect(Effect.Invulnerable))
        {
            _acidStayTimer = 0;
            _mouthStayTimer = 0;
        }
        
        if (_acidStayTimer >= DataManager.Instance.acidHurtInterval)
        {
            playerDamageable.TakeDamage(DataManager.Instance.acidDamage, true);
            _acidStayTimer = 0;
        }
        
        if (_mouthStayTimer >= DataManager.Instance.bossConfig.mouthEnhancedAttackInterval)
        {
            playerDamageable.TakeDamage(DataManager.Instance.bossConfig.mouthEnhancedAttackDamage, true);
            _mouthStayTimer = 0;
        }
    }
}