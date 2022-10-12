using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PeekabooNPCData", menuName = "Data/PeekabooNPCData")]
public class PeekabooNPCData : ScriptableObject
{
    public float ViewAngle { get { return viewAngle; } }
    public float LeftAngleReachTime { get { return leftAngleReachTime; } }
    public float RightAngleReachTime { get { return rightAngleReachTime; } }
    public float LookingLeftSideTime { get { return lookingLeftSideTime; } }
    public float LookingRightSideTime { get { return lookingRightSideTime; } }
    public float MinWaitTimeForNextAnimation { get { return minWaitTimeForNextAnimationRoutine; } }
    public float MaxWaitTimeForNextAnimation { get { return maxWaitTimeForNextAnimationRoutine; } }
    public int AnimationRoutineStartProbability { get { return animationRoutineStartProbability; } }
    public float RotateToCounterAttackTarget { get { return rotateToCounterAttackTarget; } }

    [SerializeField]
    private float viewAngle;
    [SerializeField]
    private float leftAngleReachTime;
    [SerializeField]
    private float rightAngleReachTime;
    [SerializeField]
    private float lookingLeftSideTime;
    [SerializeField]
    private float lookingRightSideTime;
    [SerializeField]
    private float minWaitTimeForNextAnimationRoutine;
    [SerializeField]
    private float maxWaitTimeForNextAnimationRoutine;
    [SerializeField]
    private int animationRoutineStartProbability;
    [SerializeField]
    private float rotateToCounterAttackTarget;
}