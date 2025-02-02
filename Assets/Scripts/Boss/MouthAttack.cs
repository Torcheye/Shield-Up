using System.Collections;
using Spine.Unity;
using TorcheyeUtility;
using UnityEngine;

public class MouthAttack : BossAttack
{
    [SerializeField] private BulletConfig acidBullet;
    [SerializeField] private Vector2 normalLaunchPower;
    [SerializeField] private int burstAmount;
    [SerializeField] private float burstRandomAngle;
    [SerializeField] private float burstRandomPower;
    [SerializeField] private float burstUpPower;
    [SerializeField] private float burstInterval;
    
    [Header("Suction")]
    [SerializeField] private float suctionPower;
    [SerializeField] private float suctionDuration;
    [SerializeField] private float suctionRampUpTime;
    [SerializeField] private ObjectRangeTrigger suctionTrigger;
    [SerializeField] private ParticleSystem suctionParticles;
    [SerializeField] private GameObject enhancedTrigger;
    
    [Header("Animation")]
    [SerializeField, SpineAnimation] private string attackAnimation;
    [SerializeField, SpineAnimation] private string idleAnimation;
    [SerializeField, SpineAnimation] private string enhancedAnimation;
    
    [Header("Boost")]
    [SerializeField] private float normalAttackBoostIntervalMultiplier;
    [SerializeField] private int burstAmountBoost;
    [SerializeField] private float burstIntervalBoostMult;

    private bool _isSucking;
    private float _suctionTimer;
    private int _burstAmount;
    private float _burstInterval;
    private Coroutine _suctionCoroutine;
    private Coroutine _burstCoroutine;

    protected override void Start()
    {
        base.Start();
        
        _burstAmount = burstAmount;
        _burstInterval = burstInterval;
        enhancedTrigger.SetActive(false);
        
        DataManager.Instance.OnBossAttackBoostEnable.AddListener(() =>
        {
            _normalAttackInterval = normalAttackInterval * normalAttackBoostIntervalMultiplier;
            _burstAmount = burstAmount + burstAmountBoost;
            _burstInterval = burstInterval * burstIntervalBoostMult;
            ResetAutoAttack();
        });
        
        DataManager.Instance.OnBossAttackBoostDisable.AddListener(() =>
        {
            _normalAttackInterval = normalAttackInterval;
            _burstAmount = burstAmount;
            _burstInterval = burstInterval;
        });
    }

    public override void OnSetInactive()
    {
        base.OnSetInactive();
        
        _isSucking = false;
        suctionParticles.Stop();
        enhancedTrigger.SetActive(false);
        DataManager.Instance.ToggleShake(false);
        if (_suctionCoroutine != null)
            StopCoroutine(_suctionCoroutine);
        if (_burstCoroutine != null)
            StopCoroutine(_burstCoroutine);
    }

    public override void Attack()
    {
        var playerPos = DataManager.Instance.playerTransform.position;
        var x = (playerPos.x - transform.position.x) * normalLaunchPower.x;
        var y = (playerPos.y - transform.position.y) * normalLaunchPower.y;
        var dir = new Vector2(x, y);
        BulletFactory.Instance.SpawnBullet(acidBullet, transform.position, dir, true, transform);
        
        skeletonAnimation.AnimationState.SetAnimation(0, attackAnimation, false);
        skeletonAnimation.AnimationState.AddAnimation(0, idleAnimation, true, 0);
    }

    public override void EnhancedAttack()
    {
        _suctionCoroutine = StartCoroutine(DoSuction());
    }
    
    private IEnumerator DoSuction()
    {
        DataManager.Instance.ToggleShake(true);
        enhancedTrigger.SetActive(true);
        moveController.CanSetInactive = false;
        _isSucking = true;
        suctionParticles.Play();
        skeletonAnimation.AnimationState.SetAnimation(0, enhancedAnimation, true);
        
        yield return new WaitForSeconds(suctionDuration);
        _isSucking = false;
        suctionParticles.Stop();
        skeletonAnimation.AnimationState.SetAnimation(0, idleAnimation, true);
        enhancedTrigger.SetActive(false);
        
        _burstCoroutine = StartCoroutine(ShootBulletBurst());
    }
    
    private IEnumerator ShootBulletBurst()
    {
        for (int i = 0; i < _burstAmount; i++)
        {
            var randomAngle = Random.Range(-burstRandomAngle, burstRandomAngle);
            Vector2 randomDir = Quaternion.Euler(0, 0, randomAngle) * Vector3.up * burstUpPower;
            randomDir += new Vector2(Random.Range(-burstRandomPower, burstRandomPower), Random.Range(-burstRandomPower, burstRandomPower));
            BulletFactory.Instance.SpawnBullet(acidBullet, transform.position, randomDir, true, transform);
            AudioManager.Instance.PlaySoundEffect(AudioManager.SoundEffect.BossShoot, .5f);
            yield return new WaitForSeconds(_burstInterval);
        }
        moveController.CanSetInactive = true;
        DataManager.Instance.ToggleShake(false);
    }

    private void Update()
    {
        if (_isSucking)
        {
            _suctionTimer += Time.deltaTime;
        }
        else
        {
            _suctionTimer = 0;
        }
    }

    private void FixedUpdate()
    {
        if (_isSucking)
        {
            foreach (var rb in suctionTrigger.objectsInTriggerStay)
            {
                var suctionDir = ((Vector2)transform.position - rb.position).normalized;
                var rampUp = Mathf.Clamp01(_suctionTimer / suctionRampUpTime);
                rb.AddForce(rampUp * suctionPower * rb.gravityScale * suctionDir);
            }
        }
    }
}