using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooPCSuprisedState : PeekabooCharacterState
{
    protected override void Initialize()
    {

    }

    public override void OnEnter()
    {
        myFSM.MyAnimator.SetInteger("State", (int)PEEKABOOPCANIMATIONSTATE.SUPRISED);

        StartCoroutine(myFSM.WaitForNextBehaviourCoroutine(0.5f, PEEKABOOCHARACTERSTATE.PCFLOP));
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {

    }
}