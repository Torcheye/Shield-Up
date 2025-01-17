using System.Collections.Generic;
using UnityEngine;

public class ItemFactory<T> : MonoBehaviour 
{
    public static ItemFactory<T>  Instance { get; private set; }
    public GameObject itemPrefab;
    public Transform itemParent;
    public int maxItemCount;
    
    private List<GameObject> _items = new List<GameObject>();
    
    private void Awake()
    {
        Instance = this;
        
        for (int i = 0; i < maxItemCount; i++)
        {
            var item = Instantiate(itemPrefab, itemParent);
            item.SetActive(false);
            _items.Add(item);
        }
    }
    
    public GameObject SpawnItem(Vector2 position)
    {
        foreach (var item in _items)
        {
            if (!item.activeInHierarchy)
            {
                item.transform.position = position;
                item.SetActive(true);
                return item;
            }
        }

        return null;
    }
    
    public static void DestroyItem(GameObject item)
    {
        item.SetActive(false);
        item.transform.parent = Instance.itemParent;
    }
}