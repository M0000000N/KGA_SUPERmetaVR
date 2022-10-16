using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooNPCLaughtState : PeekabooCharacterState
{
    protected override void Initialize()
    {

    }

    public override void OnEnter()
    {
        myFSM.MyAnimator.SetInteger("State", (int)PEEKABOONPCANIMATIONSTATE.LAUGHT);
        StartCoroutine(myFSM.WaitForNextBehaviourCoroutine(1f, PEEKABOOCHARACTERSTATE.NPCROTATETOATTACKER));
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {

    }
}