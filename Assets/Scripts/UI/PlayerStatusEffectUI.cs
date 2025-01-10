using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusEffectUI : MonoBehaviour
{
    [SerializeField] private Image countdownBar;
    
    public void UpdateStatusEffect(float progress)
    {
        if (progress <= 0)
        {
            gameObject.SetActive(false);
            return;
        }
        
        gameObject.SetActive(true);
        countdownBar.fillAmount = progress;
    }
}