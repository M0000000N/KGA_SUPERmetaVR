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
            Debug.Log("공격 대상이 PC입니다!");
            stateKey = PEEKABOOCHARACTERSTATE.IDLE;
        }
        else
        {
            Debug.Log("공격 대상이 NPC입니다!");
            stateKey = PEEKABOOCHARACTERSTATE.PCSUPRISED;
        }

        StartCoroutine(myFSM.WaitForNextBehaviourCoroutine(1f, stateKey));
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {

    }
}