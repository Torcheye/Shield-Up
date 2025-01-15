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
    protected bool doMove;

    protected virtual void Start()
    {
        var bossConfig = DataManager.Instance.bossConfig;
        
        moveSpeed = bossConfig.GetBossMoveSpeed(bossType);
        doMove = true;
    }

    protected virtual void OnSetMove() { }
}