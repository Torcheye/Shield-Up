using System;
using UnityEngine;
using UnityEngine.Splines;

public class EyeMoveController : BossMoveController
{
    [SerializeField] private SplineAnimate splineAnimate;

    protected override void Awake()
    {
        base.Awake();
        
        splineAnimate.MaxSpeed = moveSpeed;
    }

    private void Start()
    {
        splineAnimate.Play();
    }
}