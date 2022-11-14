using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooNPCPeekabooState : PeekabooCharacterState
{
    protected override void Initialize()
    {

    }

    public override void OnEnter()
    {
        myFSM.MyAnimator.SetInteger("State", (int)PEEKABOONPCANIMATIONSTATE.PEEKABOO);
        StartCoroutine(myFSM.WaitForNextBehaviourCoroutine(1f, PEEKABOOCHARACTERSTATE.NPCDIE));
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {

    }
}