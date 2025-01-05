using System.Collections;
using DG.Tweening;
using UnityEngine;

public class NinjaDashSkill : SkillBase
{
    public float dashDistance;
    public float dashDuration;
    public float cooldown;
    
    private float _cooldownTimer;
    private Tween _dashTween;

    protected override void OnUpdate(float deltaTime)
    {
        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= deltaTime;
            // TODO: Update UI cooldown
        }
    }

    protected override void OnSecondaryActionStart()
    {
        if (_cooldownTimer <= 0)
        {
            BeginDash();
            StartCoroutine(WaitForDashEnd());
        }
    }

    private void BeginDash()
    {
        if (_dashTween != null)
        {
            _dashTween.Kill();
        }
        
        var mousePos = GeneralInput.Instance.GetMousePosition();
        var direction = (mousePos - playerController.playerTransform.position).normalized;
        var destination = playerController.playerTransform.position + direction * dashDistance;
        _dashTween = playerController.playerTransform.DOMove(destination, dashDuration);
        
        playerController.IsInvincible = true;
    }

    private IEnumerator WaitForDashEnd()
    {
        yield return new WaitForSeconds(dashDuration);
        
        playerController.IsInvincible = false;
        _cooldownTimer = cooldown;
    }
}