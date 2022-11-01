using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooPCLosingSoulState : PeekabooCharacterState
{
    protected override void Initialize()
    {

    }

    public override void OnEnter()
    {
        myFSM.MyAnimator.SetInteger("State", (int)PEEKABOOPCANIMATIONSTATE.LOSINGSOUL);

        if (myFSM.MyCharacter.Attacker == null)
        {
            StartCoroutine(myFSM.WaitForNextBehaviourCoroutine(2f, PEEKABOOCHARACTERSTATE.IDLE));
        }
        else
        {
            StartCoroutine(myFSM.WaitForNextBehaviourCoroutine(2f, PEEKABOOCHARACTERSTATE.PCDIE));
        }
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {

    }
}