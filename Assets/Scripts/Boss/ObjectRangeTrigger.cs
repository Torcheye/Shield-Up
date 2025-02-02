using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRangeTrigger : MonoBehaviour
{
    public bool checkPlayer;
    public List<string> tagsToCheck;
    public List<Rigidbody2D> objectsInTriggerStay = new List<Rigidbody2D>();
    public List<Rigidbody2D> objectsInTriggerEnter = new List<Rigidbody2D>();

    private void Awake()
    {
        objectsInTriggerStay.Clear();
    }

    private void OnValidate()
    {
        if (checkPlayer && !tagsToCheck.Contains("Player"))
        {
            tagsToCheck.Add("Player");
        }
        else if (!checkPlayer && tagsToCheck.Contains("Player"))
        {
            tagsToCheck.Remove("Player");
        }
    }

    private void LateUpdate()
    {
        objectsInTriggerEnter.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var rb = other.attachedRigidbody;
        if (rb != null && tagsToCheck.Contains(rb.tag) && !objectsInTriggerStay.Contains(rb))
        {
            objectsInTriggerStay.Add(rb);
            objectsInTriggerEnter.Add(rb);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        var rb = other.attachedRigidbody;
        if (rb != null && objectsInTriggerStay.Contains(rb) && tagsToCheck.Contains(rb.tag))
        {
            objectsInTriggerStay.Remove(rb);
        }
    }
}