using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooPCRotateToTargetState : PeekabooCharacterState
{
    protected override void Initialize()
    {

    }

    public override void OnEnter()
    {
        Vector3 direction = myFSM.MyCharacter.AttackTarget.transform.position - transform.position;
        Quaternion targetQuaternion = Quaternion.LookRotation(direction);
        StartCoroutine(myFSM.RotateCoroutine(targetQuaternion, 1f, PEEKABOOCHARACTERSTATE.PCPEEKABOO));
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {

    }
}