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
    [SerializeField] private GameObject bubble;
    
    private Tween _vignetteTween;
    private Vignette _vignette;
    
    private IEnumerator DoShowBubble(float time)
    {
        bubble.SetActive(true);
        yield return new WaitForSeconds(time);
        bubble.SetActive(false);
    }
    
    public void ShowBubble(float time)
    {
        StartCoroutine(DoShowBubble(time));
    }

    protected override void Die()
    {
        base.Die();
        
        UIManager.Instance.ShowGameOverScreen();
    }

    private void OnEnable()
    {
        isPlayer = true;
        hitVolume.profile.TryGet(out _vignette);
        bubble.SetActive(false);
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
            StartCoroutine(DoVignette(DataManager.Instance.playerHitColor, 
                DataManager.Instance.damageColorLastDuration, DataManager.Instance.damageColorFadeDuration));
        }
    }

    private IEnumerator DoHitStop()
    {
        Time.timeScale = DataManager.Instance.hitStopTimeScale;
        impulseSource.GenerateImpulse();
        yield return new WaitForSecondsRealtime(DataManager.Instance.hitStopDuration);
        Time.timeScale = 1;
    }

    private IEnumerator DoVignette(Color color, float lastTime, float fadeTime)
    {
        if (_vignetteTween != null && _vignetteTween.IsActive())
        {
            _vignetteTween.Kill();
        }
        
        _vignette.color.value = color;
        _vignette.intensity.value = 0.5f;
        yield return new WaitForSeconds(lastTime);
        _vignetteTween = DOTween.To(value => _vignette.intensity.value = value, .5f, 0, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        _vignette.intensity.value = 0;
    }

    protected override void OnHeal(int amount)
    {
        UIManager.Instance.UpdatePlayerHp(Hp, DataManager.Instance.playerMaxHp);
        StartCoroutine(DoVignette(DataManager.Instance.healColor, 
            DataManager.Instance.healColorLastDuration, DataManager.Instance.healColorFadeDuration));
    }
}