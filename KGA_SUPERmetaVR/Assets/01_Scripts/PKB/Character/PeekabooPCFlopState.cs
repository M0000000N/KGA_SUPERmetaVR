using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooPCFlopState : PeekabooCharacterState
{
    protected override void Initialize()
    {

    }

    public override void OnEnter()
    {
        myFSM.MyAnimator.SetInteger("State", (int)PEEKABOOPCANIMATIONSTATE.FLOP);

        StartCoroutine(myFSM.WaitForNextBehaviourCoroutine(1f, PEEKABOOCHARACTERSTATE.PCLOSINGSOUL));
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {

    }
}