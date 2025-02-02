using System.Collections;
using DG.Tweening;
using TorcheyeUtility;
using UnityEngine;

public class GroundBlock : MonoBehaviour
{
    [SerializeField] private bool isBreakable;
    [SerializeField] private int hp;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] damageSprites;
    [SerializeField] private float damageColorFlashDuration;
    [SerializeField] private GameObject colliderObject;
    [SerializeField] private GameObject acidPool;
    
    private int _currentHp;
    private Tween _damageColorTween;
    private float _regenTimer;
    private static readonly int HitEffectBlend = Shader.PropertyToID("_HitEffectBlend");

    private void Awake()
    {
        acidPool.SetActive(false);
        _currentHp = hp;
    }

    private void Update()
    {
        if (!isBreakable)
            return;
        
        if (_regenTimer > 0)
        {
            _regenTimer -= Time.deltaTime;
            if (_regenTimer <= 0)
            {
                colliderObject.SetActive(true);
                spriteRenderer.material.DisableKeyword("OUTBASE_ON");
                _currentHp = hp;
                spriteRenderer.sprite = damageSprites[0];
            }
        }
    }
    
    public void AttachAcidPool(float time)
    {
        AudioManager.Instance.PlaySoundEffect(AudioManager.SoundEffect.AcidPoolSpawn);
        StopAllCoroutines();
        StartCoroutine(DoSpawnAcidPool(time));
    }
    
    private IEnumerator DoSpawnAcidPool(float time)
    {
        acidPool.SetActive(true);
        yield return new WaitForSeconds(time);
        acidPool.SetActive(false);
    }

    /// returns whether the block still exists
    public bool TakeHit()
    {
        if (!isBreakable)
            return true;
        
        _currentHp--;
        if (_currentHp <= 0)
        {
            colliderObject.SetActive(false);
            spriteRenderer.material.EnableKeyword("OUTBASE_ON");
            _regenTimer = DataManager.Instance.breakableGroundRegenTime;
            
            StopAllCoroutines();
            acidPool.SetActive(false);
            AudioManager.Instance.PlaySoundEffect(AudioManager.SoundEffect.GroundBlockBreak);
            return false;
        }
        else
        {
            FlashDamageColor();
            AudioManager.Instance.PlaySoundEffect(AudioManager.SoundEffect.GroundHit);
            spriteRenderer.sprite = damageSprites[hp - _currentHp];
            return true;
        }
    }
    
    private void FlashDamageColor()
    {
        if (_damageColorTween != null && _damageColorTween.IsActive())
        {
            _damageColorTween.Kill();
        }

        spriteRenderer.material.SetFloat(HitEffectBlend, 1);
        _damageColorTween = DOTween.To(value => spriteRenderer.material.SetFloat(HitEffectBlend, value), 1, 0, damageColorFlashDuration).SetEase(Ease.OutBounce); 
        _damageColorTween.onComplete += () =>
        {
            spriteRenderer.material.SetFloat(HitEffectBlend, 0);
        };
    }
}