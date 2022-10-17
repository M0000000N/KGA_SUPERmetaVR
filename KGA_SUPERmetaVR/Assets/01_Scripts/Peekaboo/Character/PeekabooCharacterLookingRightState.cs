using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooCharacterLookingRightState : PeekabooCharacterState
{
    private float minTimeToLookingRight;
    private float maxTimeToLookingRight;

    private float timeToLookingRight;

    protected override void Initialize()
    {
        int index = (int)PEEKABOOCHARACTERBEHAVIOURDATA.MIN_SEEINGTIME_RIGHT;
        minTimeToLookingRight = StaticData.GetPeekabooCharacterBehaviourData(index).VALUE;
        index = (int)PEEKABOOCHARACTERBEHAVIOURDATA.MAX_SEEINGTIME_RIGHT;
        maxTimeToLookingRight = StaticData.GetPeekabooCharacterBehaviourData(index).VALUE;
    }

    public override void OnEnter()
    {
        timeToLookingRight = Random.Range(minTimeToLookingRight, maxTimeToLookingRight);

        StartCoroutine(myFSM.WaitForNextBehaviourCoroutine(timeToLookingRight, PEEKABOOCHARACTERSTATE.RIGHTTOFRONT));
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {
        StopAllCoroutines();
    }
}