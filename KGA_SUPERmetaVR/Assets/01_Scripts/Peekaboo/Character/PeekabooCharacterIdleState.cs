using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooCharacterIdleState : PeekabooCharacterState
{
    // -----------------------------------
    #region DB 추가 요청할 부분
    private float minTimeToNextBehaviour;
    private float maxTimeToNextBehaviour;
    #endregion
    // -----------------------------------

    private float waitTimeToNextBehaviour;

    protected override void Initialize()
    {
        // -----------------------------------
        #region DB 수정 후 수정 할 코드 구문
        minTimeToNextBehaviour = 0f;
        maxTimeToNextBehaviour = 13f;
        #endregion
        // -----------------------------------
    }

    public override void OnEnter()
    {
        myFSM.MyAnimator.SetInteger("State", (int)PEEKABOOCHARACTERSTATE.IDLE);
        waitTimeToNextBehaviour = Random.Range(minTimeToNextBehaviour, maxTimeToNextBehaviour);

        StartCoroutine(myFSM.WaitForNextBehaviourCoroutine(waitTimeToNextBehaviour, PEEKABOOCHARACTERSTATE.FRONTTOLEFT));
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {
        StopAllCoroutines();
    }
}