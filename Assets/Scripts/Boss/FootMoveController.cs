using UnityEngine;

public class FootMoveController : BossMoveController
{
    [SerializeField] private FootAttack attack;
    protected override void OnSetIsActive()
    {
        base.OnSetIsActive();
        attack.OnSetIsActive(IsActive);
    }
}