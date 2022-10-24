using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooPCPeekabooState : PeekabooCharacterState
{
    protected override void Initialize()
    {

    }

    public override void OnEnter()
    {
        myFSM.MyAnimator.SetInteger("State", (int)PEEKABOOPCANIMATIONSTATE.PEEKABOO);

        PEEKABOOCHARACTERSTATE stateKey;
        if (myFSM.MyCharacter.AttackTarget.tag == "PC")
        {
            stateKey = PEEKABOOCHARACTERSTATE.IDLE;
        }
        else
        {
            stateKey = PEEKABOOCHARACTERSTATE.PCSUPRISED;
        }

        StartCoroutine(myFSM.WaitForNextBehaviourCoroutine(1f, stateKey));
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {
        StopAllCoroutines();
    }
}