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

    Quaternion initialQuaternion;
    private float waitTimeToNextBehaviour;
    private float elapsedTime;

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
        Debug.Log("Idle ���� ����!");
        myFSM.MyAnimator.SetInteger("State", (int)PEEKABOOCHARACTERSTATE.IDLE);
        myFSM.MyCharacter.SetMyInteractingState(false);
        waitTimeToNextBehaviour = Random.Range(minTimeToNextBehaviour, maxTimeToNextBehaviour);
        initialQuaternion = transform.rotation;
        elapsedTime = 0f;
    }

    public override void OnUpdate()
    {
        elapsedTime += Time.deltaTime;
        if (waitTimeToNextBehaviour <= elapsedTime)
        {
            myFSM.ChangeState(PEEKABOOCHARACTERSTATE.FRONTTOLEFT);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(initialQuaternion, myFSM.MyBodyRotation, elapsedTime * minTimeToNextBehaviour);
        }
    }

    public override void OnExit()
    {
        StopAllCoroutines();
    }
}