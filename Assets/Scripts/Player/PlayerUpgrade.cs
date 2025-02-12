﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TorcheyeUtility;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    [SerializeField] private float pickUpTime;
    [SerializeField] private RingController ringController;
    [SerializeField] private Transform pickupDestination;
    
    private List<GameObject> _pickedUpXps;

    private void Start()
    {
        UIManager.Instance.UpdatePlayerXp(DataManager.Instance.playerXp, DataManager.Instance.XpToNextLevel);
        _pickedUpXps = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("XpPickup") && !_pickedUpXps.Contains(other.attachedRigidbody.gameObject))
        {
            var transform1 = other.attachedRigidbody.transform;
            _pickedUpXps.Add(other.attachedRigidbody.gameObject);
            transform1.DOMove(pickupDestination.position, pickUpTime).SetEase(Ease.InBack);
            StartCoroutine(PickUpXp(transform1.gameObject));
        }
    }
    
    private IEnumerator PickUpXp(GameObject xp)
    {
        yield return new WaitForSeconds(pickUpTime);
        AudioManager.Instance.PlaySoundEffect(AudioManager.SoundEffect.CollectXp);
        yield return null;
        
        XpPickupFactory.DestroyItem(xp);
        _pickedUpXps.Remove(xp);
        DataManager.Instance.playerXp++;
        UIManager.Instance.UpdatePlayerXp(DataManager.Instance.playerXp, DataManager.Instance.XpToNextLevel);
        
        if (DataManager.Instance.playerXp >= DataManager.Instance.XpToNextLevel)
        {
            DataManager.Instance.playerXp = 0;
            
            DataManager.Instance.playerLevel++;
            
            UIManager.Instance.UpdatePlayerXp(DataManager.Instance.playerXp, DataManager.Instance.XpToNextLevel);
            DataManager.Instance.IsGamePaused = true;
            UIManager.Instance.OpenUpgradeScreen();
        }
    }
}