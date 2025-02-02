using System.Collections;
using Sirenix.OdinInspector;
using Spine.Unity;
using TorcheyeUtility;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public float EnhancedAttackTime => enhancedAttackTime;
    
    [SerializeField] protected BossMoveController moveController;
    [SerializeField, ShowIf(nameof(autoAttack))] protected float normalAttackInterval;
    [SerializeField] protected float enhancedAttackTime;
    [SerializeField] protected bool autoAttack = true;
    [SerializeField] protected SkeletonAnimation skeletonAnimation;

    private bool _loopNormalAttack = true;
    private bool _isDoingEnhancedAttack;
    protected IEnumerator _doAttackCoroutine;
    protected float _normalAttackInterval;

    protected virtual void Start()
    {
        _normalAttackInterval = normalAttackInterval;
        if (autoAttack)
        {
            _doAttackCoroutine = DoAttack();
            StartCoroutine(_doAttackCoroutine);
        }
    }
    
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    
    public void ResetAutoAttack()
    {
        if (_doAttackCoroutine != null)
        {
            StopCoroutine(_doAttackCoroutine);
        }
        if (autoAttack)
        {
            _doAttackCoroutine = DoAttack();
            StartCoroutine(_doAttackCoroutine);
        }
    }

    private IEnumerator DoAttack()
    {
        while (gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(_normalAttackInterval);
            if (_loopNormalAttack && moveController.IsActive)
            {
                Attack();
                AudioManager.Instance.PlaySoundEffect(AudioManager.SoundEffect.BossShoot);
            }
        }
    }
    
    public bool StartEnhancedAttack()
    {
        if (_isDoingEnhancedAttack || !moveController.IsActive)
        {
            return false;
        }
        StartCoroutine(DoEnhancedAttack());
        return true;
    }
    
    public virtual void OnSetInactive()
    {
    }
    
    private IEnumerator DoEnhancedAttack()
    {
        _loopNormalAttack = false;
        moveController.DoMove = false;
        _isDoingEnhancedAttack = true;
        AudioManager.Instance.PlaySoundEffect(AudioManager.SoundEffect.BossEnhanced);
        EnhancedAttack();
        yield return new WaitForSeconds(enhancedAttackTime);
        _loopNormalAttack = true;
        moveController.DoMove = true;
        _isDoingEnhancedAttack = false;
    }

    public virtual void Attack() { }

    public virtual void EnhancedAttack() { }
}