using System.Collections;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerDamageable : Damageable
{
    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] private Volume hitVolume;
    
    private Tween _vignetteTween;
    private Vignette _vignette;
    
    private void OnEnable()
    {
        isPlayer = true;
        hitVolume.profile.TryGet(out _vignette);
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
        if (dmg > 0)
        {
            StartCoroutine(DoHitStop());
            StartCoroutine(DoVignette(DataManager.Instance.playerHitColor, DataManager.Instance.playerVignetteLastDuration));
        }
    }

    private IEnumerator DoHitStop()
    {
        Time.timeScale = DataManager.Instance.hitStopTimeScale;
        impulseSource.GenerateImpulse();
        yield return new WaitForSecondsRealtime(DataManager.Instance.hitStopDuration);
        Time.timeScale = 1;
    }

    private IEnumerator DoVignette(Color color, float time)
    {
        if (_vignetteTween != null && _vignetteTween.IsActive())
        {
            _vignetteTween.Kill();
        }
        
        _vignette.color.value = color;
        _vignetteTween = DOTween.To(value => _vignette.intensity.value = value, .5f, 0, time);
        yield return new WaitForSeconds(time);
        _vignette.intensity.value = 0;
    }

    protected override void OnHeal(int amount)
    {
        UIManager.Instance.UpdatePlayerHp(Hp, DataManager.Instance.playerMaxHp);
        StartCoroutine(DoVignette(DataManager.Instance.healColor, DataManager.Instance.playerVignetteLastDuration));
    }
}