using System.Collections.Generic;
using UnityEngine;

public class MouthSuctionTrigger : MonoBehaviour
{
    private List<Rigidbody2D> _objectsInTrigger = new List<Rigidbody2D>();
    public List<Rigidbody2D> ObjectsInTrigger => _objectsInTrigger;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody.CompareTag("Player") || other.CompareTag("XpPickup"))
        {
            _objectsInTrigger.Add(other.attachedRigidbody);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("XpPickup"))
        {
            _objectsInTrigger.Remove(other.attachedRigidbody);
        }
    }
}