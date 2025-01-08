using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool isPlayer;

    protected int hp;
    private Tween _damageColorTween;
    private float _damageCooldown;
    
    protected virtual void Start()
    {
        hp = maxHealth;
    }

    private void Update()
    {
        if (_damageCooldown > 0)
        {
            _damageCooldown -= Time.deltaTime;
        }
    }

    public bool TakeDamage(int dmg, bool isHostile)
    {
        if (_damageCooldown > 0 || isHostile != isPlayer)
        {
            return false;
        }
        
        hp -= dmg;
        _damageCooldown = DataManager.Instance.damageCooldown;
        Debug.Log($"{gameObject.name} takes {dmg} damage. Current HP: {hp}");
        
        if (hp <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(FlashDamageColor());
            OnTakeDamage(dmg);
        }

        return true;
    }

    protected virtual void OnTakeDamage(int dmg) { }
    
    protected virtual void Die()
    {
        Destroy(gameObject);
    }
    
    private IEnumerator FlashDamageColor()
    {
        if (_damageColorTween != null)
        {
            _damageColorTween.Kill();
        }
        
        spriteRenderer.color = DataManager.Instance.damageColor;
        yield return new WaitForSeconds(DataManager.Instance.damageColorLastDuration);
        _damageColorTween = spriteRenderer.DOColor(Color.white, DataManager.Instance.damageColorFadeDuration);
    }
}