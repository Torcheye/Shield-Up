using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    [SerializeField] private float pickUpTime;
    [SerializeField] private RingController ringController;
    [SerializeField] private Transform pickupDestination;

    private void Start()
    {
        UIManager.Instance.UpdatePlayerXp(DataManager.Instance.playerXp, DataManager.Instance.xpToNextLevel);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("XpPickup"))
        {
            var transform1 = other.attachedRigidbody.transform;
            transform1.DOMove(pickupDestination.position, pickUpTime).SetEase(Ease.InBack);
            StartCoroutine(PickUpXp(transform1.gameObject));
        }
    }
    
    private IEnumerator PickUpXp(GameObject xp)
    {
        yield return new WaitForSeconds(pickUpTime);
        
        XpPickupFactory.DestroyItem(xp);
        DataManager.Instance.playerXp++;
        UIManager.Instance.UpdatePlayerXp(DataManager.Instance.playerXp, DataManager.Instance.xpToNextLevel);
        
        if (DataManager.Instance.playerXp >= DataManager.Instance.xpToNextLevel)
        {
            DataManager.Instance.playerXp = 0;
            DataManager.Instance.xpToNextLevel++;
            UIManager.Instance.UpdatePlayerXp(DataManager.Instance.playerXp, DataManager.Instance.xpToNextLevel);
            DataManager.Instance.IsGamePaused = true;
            UIManager.Instance.OpenUpgradeScreen();
        }
    }
}