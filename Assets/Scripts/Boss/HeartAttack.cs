﻿using System.Collections;
using DG.Tweening;
using UnityEngine;

public class HeartAttack : BossAttack
{
    [SerializeField] private float propelDuration;
    [SerializeField] private float propelPower;
    [SerializeField] private float propelRadius;
    [SerializeField] private ObjectRangeTrigger propulsionTrigger;
    [SerializeField] private SpriteRenderer propulsionSprite;
    
    [Header("Boost")]
    [SerializeField] private float normalAttackBoostIntervalMultiplier;
    
    private float _propulsionSpriteStartAlpha;

    private void Awake()
    {
        _propulsionSpriteStartAlpha = propulsionSprite.color.a;
        propulsionTrigger.gameObject.SetActive(false);
    }
    
    protected override void Start()
    {
        base.Start();
        
        DataManager.Instance.OnBossAttackBoostEnable.AddListener(() =>
        {
            _normalAttackInterval = normalAttackInterval * normalAttackBoostIntervalMultiplier;
            ResetAutoAttack();
        });
        
        DataManager.Instance.OnBossAttackBoostDisable.AddListener(() =>
        {
            _normalAttackInterval = normalAttackInterval;
        });
    }

    public override void EnhancedAttack()
    {
        DataManager.Instance.EnableBossAttackBoost();
    }
    
    public override void Attack()
    {
        StartCoroutine(DoPropel());
    }

    private IEnumerator DoPropel()
    {
        if (moveController.DoMove)
        {
            propulsionTrigger.gameObject.SetActive(true);
            propulsionSprite.DOFade(_propulsionSpriteStartAlpha, 0);
            propulsionTrigger.transform.localScale = Vector3.zero;
            propulsionTrigger.transform.DOScale(Vector3.one * propelRadius, propelDuration);
            propulsionSprite.DOFade(0, propelDuration);
        }
        
        yield return new WaitForSeconds(propelDuration);
        
        propulsionTrigger.transform.localScale = Vector3.zero;
        propulsionTrigger.gameObject.SetActive(false);
    }
    
    private void FixedUpdate()
    {
        foreach (var obj in propulsionTrigger.objectsInTriggerStay)
        {
            var dir = (obj.transform.position - transform.position).normalized;
            obj.AddForce(dir * propelPower);
        }
    }
}