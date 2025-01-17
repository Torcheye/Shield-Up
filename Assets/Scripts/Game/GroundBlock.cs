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
    
    private int _currentHp;
    private Tween _damageColorTween;

    private void Awake()
    {
        _currentHp = hp;
    }

    /// returns whether the block still exists
    public bool TakeHit()
    {
        if (!isBreakable)
            return true;
        
        _currentHp--;
        
        if (_currentHp <= 0)
        {
            Destroy(gameObject);
            return false;
        }
        else
        {
            FlashDamageColor();
            spriteRenderer.sprite = damageSprites[hp - _currentHp - 1];
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