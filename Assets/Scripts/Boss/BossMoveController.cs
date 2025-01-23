using UnityEngine;

public class BossMoveController : MonoBehaviour
{
    public bool DoMove
    {
        get => doMove;
        set
        {
            doMove = value;
            OnSetMove();
        }
    }
    
    [SerializeField] private BossType bossType;
    
    protected float moveSpeed;
    protected bool doMove = true;

    protected virtual void Start()
    {
        var bossConfig = DataManager.Instance.bossConfig;
        
        moveSpeed = bossConfig.GetBossMoveSpeed(bossType);
    }

    protected virtual void OnSetMove() { }
}