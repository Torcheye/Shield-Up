using System.Collections;
using DG.Tweening;
using TorcheyeUtility;
using UnityEngine;
using UnityEngine.UI;

public class Potion : Weapon
{
    [SerializeField] private Image[] chargeImages;
    [SerializeField] private Collider2D absorbTrigger;
    [SerializeField] private float absorbScale;
    [SerializeField] private float absorbScaleDuration;
    
    private int _chargeCount;
    private int _maxChargeCount;
    
    private void Start()
    {
        _maxChargeCount = DataManager.Instance.weaponsConfig.GetPotionCharge(Level);
        _chargeCount = 0;
    }
    
    private void Update()
    {
        chargeImages[Level-1].fillAmount = (float)_chargeCount / _maxChargeCount;
        
        if (_chargeCount >= _maxChargeCount)
        {
            _chargeCount = 0;
            ringController.RemoveWeapon(slotIndex, absorbScaleDuration);
            
            if (Level < 3)
            {
                playerDamageable.Heal(1);
            }
            else
            {
                playerDamageable.ShowBubble(DataManager.Instance.weaponsConfig.potionL3BubbleDuration);
                playerDamageable.ApplyEffect(Effect.Invulnerable, DataManager.Instance.weaponsConfig.potionL3BubbleDuration);
            }
        }
    }

    protected override void OnLevelChange(int newLevel)
    {
        base.OnLevelChange(newLevel);
        
        _maxChargeCount = DataManager.Instance.weaponsConfig.GetPotionCharge(Level);
    }

    public void Charge()
    {
        _chargeCount++;
        AudioManager.Instance.PlaySoundEffect(AudioManager.SoundEffect.PotionFill);
        StartCoroutine(DoScaleAnimation());
    }
    
    private IEnumerator DoScaleAnimation()
    {
        absorbTrigger.enabled = false;
        transform.DOPunchScale(Vector3.one * absorbScale, absorbScaleDuration, 3, 0);
        yield return new WaitForSeconds(absorbScaleDuration);
        transform.localScale = Vector3.one;
        absorbTrigger.enabled = true;
    }
}