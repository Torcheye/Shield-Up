using UnityEngine;
using UnityEngine.Splines;

public class EyeMoveController : BossMoveController
{
    [SerializeField] private SplineAnimate splineAnimate;

    protected override void Start()
    {
        base.Start();
        splineAnimate.MaxSpeed = moveSpeed;
        
        if (doMove)
            splineAnimate.Play();
    }
    
    protected override void OnSetMove()
    {
        if (doMove)
            splineAnimate.Play();
        else
            splineAnimate.Pause();
    }
}