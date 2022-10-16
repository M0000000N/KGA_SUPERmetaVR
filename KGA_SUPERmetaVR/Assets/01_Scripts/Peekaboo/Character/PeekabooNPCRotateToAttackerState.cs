using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooNPCRotateToAttackerState : PeekabooCharacterState
{
    protected override void Initialize()
    {

    }

    public override void OnEnter()
    {
        Vector3 targetDirection = myFSM.MyCharacter.Attacker.transform.position - transform.position;
        float rotateAngleY = Quaternion.FromToRotation(Vector3.forward, targetDirection).eulerAngles.y;
        Quaternion rotateQuaternion = Quaternion.Euler(new Vector3(0f, rotateAngleY, 0f));
        Quaternion targetQuaternion = transform.rotation * rotateQuaternion;
        StartCoroutine(myFSM.RotateCoroutine(targetQuaternion, 1f, PEEKABOOCHARACTERSTATE.NPCPEEKABOO));
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {

    }
}