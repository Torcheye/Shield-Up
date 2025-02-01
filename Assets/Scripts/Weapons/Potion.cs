using UnityEngine;
using UnityEngine.UI;

public class Potion : Weapon
{
    [SerializeField] private Image[] chargeImages;
    
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
            playerDamageable.Heal(1);
            _chargeCount = 0;
            ringController.RemoveWeapon(slotIndex);
        }
    }
    
    public void Charge()
    {
        _chargeCount++;
    }
}