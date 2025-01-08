﻿using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Image playerHpBar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void UpdatePlayerHp(int currentHp, int maxHp)
    {
        playerHpBar.fillAmount = (float) currentHp / maxHp;
    }
}