using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    private static readonly int FillPhase = Shader.PropertyToID("_FillPhase");
    private static readonly int FillColor = Shader.PropertyToID("_FillColor");
    public int Hp { get; protected set; }
    
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private StatusEffect statusEffect;
    
    protected bool isPlayer;
    private Tween _damageColorTween;
    private float _damageCooldown;
    private float _damageColorFlashValue;
    protected int maxHp;
    
    public void ApplyEffect(Effect effect, float duration)
    {
        statusEffect.ApplyEffect(effect, duration);
    }
    
    protected virtual void Start() { }

    private void Update()
    {
        if (_damageCooldown > 0)
        {
            _damageCooldown -= Time.deltaTime;
        }
    }
    
    public void Heal(int amount)
    {
        Hp += amount;
        Debug.Log($"{gameObject.name} heals {amount} HP. Current HP: {Hp}");
        
        if (Hp > maxHp)
        {
            Hp = maxHp;
        }
        
        StartCoroutine(FlashColor(
            DataManager.Instance.healColor, DataManager.Instance.healColorLastDuration, DataManager.Instance.healColorFadeDuration));
        OnHeal(amount);
    }

    public bool TakeDamage(int dmg, bool isHostile)
    {
        if (_damageCooldown > 0 || isHostile != isPlayer || statusEffect.HasEffect(Effect.Invulnerable) || !TryTakeDamage())
        {
            return false;
        }
        
        Hp -= dmg;
        _damageCooldown = DataManager.Instance.damageCooldown;
        Debug.Log($"{gameObject.name} takes {dmg} damage. Current HP: {Hp}");
        
        if (Hp <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(FlashColor(
                DataManager.Instance.damageColor, DataManager.Instance.damageColorLastDuration, DataManager.Instance.damageColorFadeDuration));
            OnTakeDamage(dmg);
        }

        return true;
    }

    protected virtual bool TryTakeDamage() { return true; }

    protected virtual void OnTakeDamage(int dmg) { }
    
    protected virtual void OnHeal(int amount) { }
    
    protected virtual void Die()
    {
        gameObject.SetActive(false);
    }
    
    private IEnumerator FlashColor(Color color, float lastDuration, float fadeDuration)
    {
        if (_damageColorTween != null)
        {
            _damageColorTween.Kill();
        }
        
        if (meshRenderer == null)
        {
            yield break;
        }
        
        meshRenderer.material.SetColor(FillColor, color);
        meshRenderer.material.SetFloat(FillPhase, 1);
        _damageColorFlashValue = 1;
        yield return new WaitForSeconds(lastDuration);
        DOTween.To(() => _damageColorFlashValue, x => _damageColorFlashValue = x, 0, fadeDuration)
            .SetEase(Ease.OutBounce)
            .OnUpdate(() => meshRenderer.material.SetFloat(FillPhase, _damageColorFlashValue));
        yield return new WaitForSeconds(fadeDuration);
        meshRenderer.material.SetFloat(FillPhase, 0);
    }
}