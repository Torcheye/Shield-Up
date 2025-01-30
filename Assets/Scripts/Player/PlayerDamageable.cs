using System.Collections;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerDamageable : Damageable
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float damageColorFlashDuration;
    [SerializeField] private CinemachineImpulseSource impulseSource;
    private static readonly int HitEffectBlend = Shader.PropertyToID("_HitEffectBlend");
    private Tween _damageColorTween;
    
    private void OnEnable()
    {
        isPlayer = true;
    }

    protected override void Start()
    {
        base.Start();
        Hp = DataManager.Instance.playerMaxHp;
        maxHp = Hp;
        UIManager.Instance.UpdatePlayerHp(Hp, DataManager.Instance.playerMaxHp);
    }

    protected override void OnTakeDamage(int dmg)
    {
        UIManager.Instance.UpdatePlayerHp(Hp, DataManager.Instance.playerMaxHp);
        StartCoroutine(FlashDamageColorPlayer(dmg > 0));
    }
    
    private IEnumerator FlashDamageColorPlayer(bool hasDamage)
    {
        if (_damageColorTween != null && _damageColorTween.IsActive())
        {
            _damageColorTween.Kill();
        }

        spriteRenderer.material.SetFloat(HitEffectBlend, 1);
        _damageColorTween = DOTween.To(value => spriteRenderer.material.SetFloat(HitEffectBlend, value), 
            1, 0, damageColorFlashDuration).SetEase(Ease.OutBounce);

        // hit stop
        if (hasDamage)
        {
            Time.timeScale = DataManager.Instance.hitStopTimeScale;
            impulseSource.GenerateImpulse();
            yield return new WaitForSecondsRealtime(DataManager.Instance.hitStopDuration);
            Time.timeScale = 1;
        }
        
        yield return new WaitForSeconds(damageColorFlashDuration);
        spriteRenderer.material.SetFloat(HitEffectBlend, 0);
    }
    
    protected override void OnHeal(int amount)
    {
        UIManager.Instance.UpdatePlayerHp(Hp, DataManager.Instance.playerMaxHp);
    }
}