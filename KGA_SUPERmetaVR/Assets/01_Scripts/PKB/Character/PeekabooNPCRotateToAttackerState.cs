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
        Vector3 direction = myFSM.MyCharacter.Attacker.transform.position - transform.position;
        Quaternion targetQuaternion = Quaternion.LookRotation(direction);
        StartCoroutine(myFSM.RotateCoroutine(targetQuaternion, 1f, PEEKABOOCHARACTERSTATE.NPCPEEKABOO));
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {

    }
}