using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int RingIndex => _ringIndex;
    public WeaponType type;
    
    private int _ringIndex = -1;
    
    public void SetRingIndex(int ringIndex)
    {
        _ringIndex = ringIndex;
        
        if (_ringIndex == -1)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}