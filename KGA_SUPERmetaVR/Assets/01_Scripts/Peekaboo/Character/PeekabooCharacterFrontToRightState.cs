using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooCharacterFrontToRightState : PeekabooCharacterState
{
    private float minTimeToRotateRight;
    private float maxTimeToRotateRight;

    private float rotateTime;

    protected override void Initialize()
    {
        int index = (int)PEEKABOOCHARACTERBEHAVIOURDATA.FRONT_TO_RIGHT_MIN_TIME;
        minTimeToRotateRight = StaticData.GetPeekabooCharacterBehaviourData(index).VALUE;
        index = (int)PEEKABOOCHARACTERBEHAVIOURDATA.FRONT_TO_RIGHT_MAX_TIME;
        maxTimeToRotateRight = StaticData.GetPeekabooCharacterBehaviourData(index).VALUE;
    }

    public override void OnEnter()
    {
        rotateTime = Random.Range(minTimeToRotateRight, maxTimeToRotateRight);

        Quaternion rotateQuaternion = Quaternion.Euler(new Vector3(0f, myFSM.ViewAngleHalf, 0f));
        Quaternion targetQuaternion = Quaternion.Euler(myFSM.ForwardDirectionOfBody) * rotateQuaternion;

        StartCoroutine(myFSM.RotateCoroutine(targetQuaternion, rotateTime, PEEKABOOCHARACTERSTATE.LOOKINGRIGHT));
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {
        StopAllCoroutines();
    }
}