using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooCharacterFrontToLeftState : PeekabooCharacterState
{
    private float minTimeToRotateLeft;
    private float maxTimeToRotateLeft;

    private float rotateTime;

    protected override void Initialize()
    {
        int index = (int)PEEKABOOCHARACTERBEHAVIOURDATA.FRONT_TO_LEFT_MIN_TIME;
        minTimeToRotateLeft = StaticData.GetPeekabooCharacterBehaviourData(index).VALUE;
        index = (int)PEEKABOOCHARACTERBEHAVIOURDATA.FRONT_TO_LEFT_MAX_TIME;
        maxTimeToRotateLeft = StaticData.GetPeekabooCharacterBehaviourData(index).VALUE;
    }

    public override void OnEnter()
    {
        rotateTime = Random.Range(minTimeToRotateLeft, maxTimeToRotateLeft);

        Quaternion rotateQuaternion = Quaternion.Euler(new Vector3(0f, -myFSM.ViewAngleHalf, 0f));
        Quaternion targetQuaternion = myFSM.MyBodyRotation * rotateQuaternion;

        StartCoroutine(myFSM.RotateCoroutine(targetQuaternion, rotateTime, PEEKABOOCHARACTERSTATE.LOOKINGLEFT));
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {
        StopAllCoroutines();
    }
}