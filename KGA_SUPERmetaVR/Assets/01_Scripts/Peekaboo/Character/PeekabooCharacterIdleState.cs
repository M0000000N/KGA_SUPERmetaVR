using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooCharacterIdleState : PeekabooCharacterState
{
    // -----------------------------------
    #region DB �߰� ��û�� �κ�
    private float minTimeToNextBehaviour;
    private float maxTimeToNextBehaviour;
    #endregion
    // -----------------------------------

    private float waitTimeToNextBehaviour;

    protected override void Initialize()
    {
        // -----------------------------------
        #region DB ���� �� ���� �� �ڵ� ����
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