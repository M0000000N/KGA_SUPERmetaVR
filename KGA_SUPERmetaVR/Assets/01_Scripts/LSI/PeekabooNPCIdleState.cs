using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooNPCIdleState : PeekabooNPCState
{
    [SerializeField]
    private float routineStartMinTime;
    [SerializeField]
    private float routineStartMaxTime;

    private float waitTimeForNextAnimation;
    private float elapsedTime;

    public override void OnEnter()
    {
        waitTimeForNextAnimation = Random.Range(routineStartMinTime, routineStartMaxTime);
        elapsedTime = 0f;
    }

    public override void OnUpdate()
    {
        if (myFSM.myNPC.IsLookingSomeone == false)
        {
            elapsedTime += Time.deltaTime;
        }

        if (waitTimeForNextAnimation <= elapsedTime)
        {
            myFSM.ChangeState(PEEKABOONPCSTATE.LOOKINGLEFT);
        }
    }

    public override void OnExit()
    {

    }
}