using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD")]
    [SerializeField] private Image playerHpBar;
    [SerializeField] private PlayerStatusEffectUI[] playerStatusEffectUIs;

    [Header("Effects")] 
    [SerializeField] private Image blindEffect;
    [SerializeField] private Vector2 blindEffectOuterRadius;

    private static readonly int PlayerScreenPos = Shader.PropertyToID("_PlayerScreenPos");
    private static readonly int OuterRadius = Shader.PropertyToID("_OuterRadius");

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

    public void UpdateBlindEffect(Vector4 screenPos, float progress)
    {
        if (progress <= 0)
        {
            blindEffect.gameObject.SetActive(false);
            return;
        }
        
        blindEffect.gameObject.SetActive(true);
        var mat = blindEffect.material;
        mat.SetVector(PlayerScreenPos, screenPos);
        var outerRadius = Mathf.Lerp(blindEffectOuterRadius.x, blindEffectOuterRadius.y, progress);
        mat.SetFloat(OuterRadius, outerRadius);
    }

    public void UpdatePlayerHp(int currentHp, int maxHp)
    {
        playerHpBar.fillAmount = (float) currentHp / maxHp;
    }
    
    public void UpdatePlayerStatusEffects(int effectIndex, float progress)
    {
        playerStatusEffectUIs[effectIndex].UpdateStatusEffect(progress);
    }
}