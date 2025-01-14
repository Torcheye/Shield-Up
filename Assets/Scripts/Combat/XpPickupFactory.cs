using System.Collections.Generic;
using UnityEngine;

public class XpPickupFactory : MonoBehaviour
{
    public static XpPickupFactory Instance { get; private set; }
    public GameObject xpPickupPrefab;
    public Transform xpPickupParent;
    public int maxXpCount = 30;
    
    private List<GameObject> _xpPickups = new List<GameObject>();
    
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        for (int i = 0; i < maxXpCount; i++)
        {
            var xpPickup = Instantiate(xpPickupPrefab, xpPickupParent);
            xpPickup.SetActive(false);
            _xpPickups.Add(xpPickup);
        }
    }
    
    public GameObject GetXp(Vector3 position)
    {
        foreach (var xpPickup in _xpPickups)
        {
            if (!xpPickup.activeInHierarchy)
            {
                xpPickup.transform.position = position;
                xpPickup.SetActive(true);
                return xpPickup;
            }
        }

        return null;
    }
    
    public static void DestroyXp(GameObject xpPickup)
    {
        xpPickup.SetActive(false);
    }
}