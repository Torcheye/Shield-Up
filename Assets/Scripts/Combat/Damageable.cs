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
    
    protected bool IsPlayer;
    private Tween _damageColorTween;
    private float _damageCooldown;
    private float _damageColorFlashValue;
    
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

    public bool TakeDamage(int dmg, bool isHostile)
    {
        if (_damageCooldown > 0 || isHostile != IsPlayer || statusEffect.HasEffect(Effect.Invulnerable))
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
            StartCoroutine(FlashDamageColor());
            OnTakeDamage(dmg);
        }

        return true;
    }

    protected virtual void OnTakeDamage(int dmg) { }
    
    protected virtual void Die()
    {
        gameObject.SetActive(false);
    }
    
    private IEnumerator FlashDamageColor()
    {
        if (_damageColorTween != null)
        {
            _damageColorTween.Kill();
        }
        
        if (meshRenderer == null)
        {
            yield break;
        }
        
        meshRenderer.material.SetColor(FillColor, DataManager.Instance.damageColor);
        _damageColorFlashValue = 1;
        yield return new WaitForSeconds(DataManager.Instance.damageColorLastDuration);
        DOTween.To(() => _damageColorFlashValue, x => _damageColorFlashValue = x, 0, DataManager.Instance.damageColorFadeDuration)
            .SetEase(Ease.OutBounce)
            .OnUpdate(() => meshRenderer.material.SetFloat(FillPhase, _damageColorFlashValue));
        yield return new WaitForSeconds(DataManager.Instance.damageColorFadeDuration);
        meshRenderer.material.SetFloat(FillPhase, 0);
    }
}