using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Shield : Weapon
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Image cooldownImage;
    [SerializeField] private float deflectScale;
    [SerializeField] private float deflectScaleDuration;
    [SerializeField] private Collider2D deflectTrigger;
    
    private int _deflectCount;
    private float _shieldCooldownTimer;
    private float _shieldCooldown;

    private void Start()
    {
        _deflectCount = DataManager.Instance.weaponsConfig.GetShieldBlock(Level);
        _shieldCooldown = DataManager.Instance.weaponsConfig.shieldCooldown;
        _shieldCooldownTimer = _shieldCooldown;
    }

    private void Update()
    {
        if (_shieldCooldownTimer < _shieldCooldown)
        {
            _shieldCooldownTimer += Time.deltaTime;
            cooldownImage.fillAmount = _shieldCooldownTimer / _shieldCooldown;
        }
        else
        {
            cooldownImage.fillAmount = 0;
        }

        spriteRenderer.enabled = _shieldCooldownTimer >= _shieldCooldown;

        var flipX = ringController.transform.position.x < transform.position.x ? -1 : 1;
        var t = levelObjects[Level - 1].transform;
        t.localScale = new Vector3(flipX * Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
    }

    public bool Deflect()
    {
        if (_shieldCooldownTimer < _shieldCooldown)
            return false;
        
        _deflectCount--;
        StartCoroutine(DoScaleAnimation());
        
        return true;
    }
    
    private IEnumerator DoScaleAnimation()
    {
        deflectTrigger.enabled = false;
        transform.DOPunchScale(Vector3.one * deflectScale, deflectScaleDuration, 3, 0);
        yield return new WaitForSeconds(deflectScaleDuration);
        transform.localScale = Vector3.one;
        deflectTrigger.enabled = true;
        
        if (_deflectCount <= 0)
        {
            _shieldCooldownTimer = 0;
            _deflectCount = DataManager.Instance.weaponsConfig.GetShieldBlock(Level);
        }
    }
}