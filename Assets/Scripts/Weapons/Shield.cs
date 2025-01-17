using UnityEngine;
using UnityEngine.UI;

public class Shield : Weapon
{
    [SerializeField] private Image cooldownImage;
    [SerializeField] private Transform cooldownCanvas;
    
    private int _deflectCount;
    private float _cooldownTimer;
    private float _deflectCooldown;

    private void Start()
    {
        _deflectCount = DataManager.Instance.weaponsConfig.GetShieldBlock(Level);
        _deflectCooldown = DataManager.Instance.weaponsConfig.shieldCooldown;
    }
    
    private void Update()
    {
        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
            cooldownImage.fillAmount = 1 - _cooldownTimer / _deflectCooldown;
        }
        else 
        {
            cooldownImage.fillAmount = 0;
        }
        cooldownCanvas.rotation = Quaternion.identity;
    }

    public bool Deflect()
    {
        if (_cooldownTimer > 0)
            return false;
        
        _deflectCount--;
        
        if (_deflectCount <= 0)
        {
            _cooldownTimer = _deflectCooldown;
            _deflectCount = DataManager.Instance.weaponsConfig.GetShieldBlock(Level);
        }
        
        return true;
    }
}