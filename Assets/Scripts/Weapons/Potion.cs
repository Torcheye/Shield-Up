using UnityEngine;
using UnityEngine.UI;

public class Potion : Weapon
{
    [SerializeField] private Image chargeImage;
    
    private int _chargeCount;
    private int _maxChargeCount;
    
    private void Start()
    {
        _maxChargeCount = DataManager.Instance.weaponsConfig.GetPotionCharge(Level);
        _chargeCount = 0;
    }
    
    private void Update()
    {
        chargeImage.fillAmount = (float)_chargeCount / _maxChargeCount;
        
        if (_chargeCount >= _maxChargeCount)
        {
            playerDamageable.Heal(1);
            _chargeCount = 0;
            ringController.RemoveWeapon(gameObject);
        }
    }
    
    public void Charge()
    {
        _chargeCount++;
    }
}