using UnityEngine;
using UnityEngine.Splines;

public class EyeMoveController : BossMoveController
{
    [SerializeField] private SplineAnimate splineAnimate;

    protected override void Start()
    {
        base.Start();
        splineAnimate.MaxSpeed = moveSpeed;
        
        if (DoMove)
            splineAnimate.Play();
    }

    private void Update()
    {
        if (DoMove && !splineAnimate.IsPlaying)
            splineAnimate.Play();
        else if (!DoMove && splineAnimate.IsPlaying)
            splineAnimate.Pause();
    }
}