using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooCharacterLookingLeftState : PeekabooCharacterState
{
    private float minTimeToLookingLeft;
    private float maxTimeToLookingLeft;

    private float timeToLookingLeft;

    protected override void Initialize()
    {
        int index = (int)PEEKABOOCHARACTERBEHAVIOURDATA.MIN_SEEINGTIME_LEFT;
        minTimeToLookingLeft = StaticData.GetPeekabooCharacterBehaviourData(index).VALUE;
        index = (int)PEEKABOOCHARACTERBEHAVIOURDATA.MAX_SEEINGTIME_LEFT;
        maxTimeToLookingLeft = StaticData.GetPeekabooCharacterBehaviourData(index).VALUE;
    }

    public override void OnEnter()
    {
        timeToLookingLeft = Random.Range(minTimeToLookingLeft, maxTimeToLookingLeft);

        StartCoroutine(myFSM.WaitForNextBehaviourCoroutine(timeToLookingLeft, PEEKABOOCHARACTERSTATE.ROTATETOFRONT));
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {

    }
}