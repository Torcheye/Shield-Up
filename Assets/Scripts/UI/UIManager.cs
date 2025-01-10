using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Image playerHpBar;
    
    [SerializeField] private StatusEffect playerStatusEffect;
    [SerializeField] private TMP_Text playerStatusEffectText;

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

    private void Update()
    {
        UpdatePlayerStatusEffectText();
    }

    public void UpdatePlayerHp(int currentHp, int maxHp)
    {
        playerHpBar.fillAmount = (float) currentHp / maxHp;
    }
    
    public void UpdatePlayerStatusEffectText()
    {
        var effects = playerStatusEffect.Effects;
        playerStatusEffectText.text = "";
        foreach (var effect in effects)
        {
            playerStatusEffectText.text += $"{effect}\n";
        }
    }
}