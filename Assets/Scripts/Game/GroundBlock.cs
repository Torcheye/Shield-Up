using System;
using DG.Tweening;
using UnityEngine;

public class GroundBlock : MonoBehaviour
{
    [SerializeField] private bool isBreakable;
    [SerializeField] private int hp;
    [SerializeField] private SpriteRenderer damageFlashRenderer;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color damageColor;
    [SerializeField] private Sprite[] damageSprites;
    [SerializeField] private float damageColorFlashDuration;
    [SerializeField] private GameObject mainObject;
    [SerializeField] private GameObject regenObject;
    
    private int _currentHp;
    private Tween _damageColorTween;
    private float _regenTimer;

    private void Awake()
    {
        _currentHp = hp;
    }

    private void Update()
    {
        if (!isBreakable)
            return;
        
        if (!mainObject.activeSelf)
        {
            _regenTimer -= Time.deltaTime;
            if (_regenTimer <= 0)
            {
                mainObject.SetActive(true);
                regenObject.SetActive(false);
                _currentHp = hp;
                spriteRenderer.sprite = damageSprites[0];
            }
        }
    }
    
    public void AttachAcidPool(Transform acidPool)
    {
        acidPool.SetParent(mainObject.transform);
    }

    /// returns whether the block still exists
    public bool TakeHit()
    {
        if (!isBreakable)
            return true;
        
        _currentHp--;
        
        if (_currentHp <= 0)
        {
            mainObject.SetActive(false);
            regenObject.SetActive(true);
            _regenTimer = DataManager.Instance.breakableGroundRegenTime;
            return false;
        }
        else
        {
            FlashDamageColor();
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
        
        damageFlashRenderer.color = damageColor;
        _damageColorTween = damageFlashRenderer.DOColor(new Color(0,0,0,0), damageColorFlashDuration).SetEase(Ease.OutBounce);
        _damageColorTween.onComplete += () =>
        {
            damageFlashRenderer.color = new Color(0,0,0,0);
        };
    }
}