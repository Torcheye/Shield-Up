using UnityEngine;

public class BossMoveController : MonoBehaviour
{
    [SerializeField] private BossType bossType;
    
    protected float moveSpeed;

    protected virtual void Start()
    {
        var bossConfig = DataManager.Instance.bossConfig;
        if (!bossConfig.GetBossEnable(bossType))
        {
            gameObject.SetActive(false);
        }
        
        moveSpeed = bossConfig.GetBossMoveSpeed(bossType);
    }
}