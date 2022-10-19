using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooCharacterLeftToFrontState : PeekabooCharacterState
{
    private float minTimeToRotateFront;
    private float maxTimeToRotateFront;

    private float rotateTime;

    protected override void Initialize()
    {
        int index = (int)PEEKABOOCHARACTERBEHAVIOURDATA.LEFT_TO_FRONT_MIN_TIME;
        minTimeToRotateFront = StaticData.GetPeekabooCharacterBehaviourData(index).VALUE;
        index = (int)PEEKABOOCHARACTERBEHAVIOURDATA.LEFT_TO_FRONT_MAX_TIME;
        maxTimeToRotateFront = StaticData.GetPeekabooCharacterBehaviourData(index).VALUE;
    }

    public override void OnEnter()
    {
        rotateTime = Random.Range(minTimeToRotateFront, maxTimeToRotateFront);

        Quaternion targetQuaternion = myFSM.MyBodyRotation;

        StartCoroutine(myFSM.RotateCoroutine(targetQuaternion, rotateTime, PEEKABOOCHARACTERSTATE.FRONTTORIGHT));
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {
        StopAllCoroutines();
    }
}