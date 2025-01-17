using UnityEngine;
using UnityEngine.UI;

public class Shield : Weapon
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Image cooldownImage;
    
    private int _deflectCount;
    private float _cooldownTimer;
    private float _deflectCooldown;

    private void Start()
    {
        _deflectCount = DataManager.Instance.weaponsConfig.GetShieldBlock(Level);
        _deflectCooldown = DataManager.Instance.weaponsConfig.shieldCooldown;
        _cooldownTimer = _deflectCooldown;
    }
    
    private void Update()
    {
        if (_cooldownTimer < _deflectCooldown)
        {
            _cooldownTimer += Time.deltaTime;
            cooldownImage.fillAmount = _cooldownTimer / _deflectCooldown;
        }
        else
        {
            cooldownImage.fillAmount = 0;
        }
        
        spriteRenderer.enabled = _cooldownTimer >= _deflectCooldown;
    }

    public bool Deflect()
    {
        if (_cooldownTimer < _deflectCooldown)
            return false;
        
        _deflectCount--;
        
        if (_deflectCount <= 0)
        {
            _cooldownTimer = 0;
            _deflectCount = DataManager.Instance.weaponsConfig.GetShieldBlock(Level);
        }
        
        return true;
    }
}