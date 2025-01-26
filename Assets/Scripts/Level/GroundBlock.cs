using DG.Tweening;
using UnityEngine;

public class GroundBlock : MonoBehaviour
{
    [SerializeField] private bool isBreakable;
    [SerializeField] private int hp;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color[] damageColors;
    [SerializeField] private float damageColorFlashDuration;
    [SerializeField] private GameObject colliderObject;
    
    private int _currentHp;
    private Tween _damageColorTween;
    private float _regenTimer;
    private Transform _acidPool;
    private static readonly int HitEffectBlend = Shader.PropertyToID("_HitEffectBlend");

    private void Awake()
    {
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
                //spriteRenderer.sprite = damageSprites[0];
            }
        }
    }
    
    public void AttachAcidPool(Transform acidPool)
    {
        acidPool.SetParent(transform);
        _acidPool = acidPool;
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
            
            if (_acidPool != null)
            {
                AcidPoolFactory.DestroyItem(_acidPool.gameObject);
                _acidPool = null;
            }
            return false;
        }
        else
        {
            FlashDamageColor();
            //spriteRenderer.sprite = damageSprites[hp - _currentHp];
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