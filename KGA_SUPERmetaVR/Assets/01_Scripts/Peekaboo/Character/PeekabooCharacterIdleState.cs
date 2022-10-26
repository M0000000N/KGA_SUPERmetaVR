using Photon.Pun;
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

    Quaternion initialQuaternion;
    private float waitTimeToNextBehaviour;
    private float elapsedTime;

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
        myFSM.MyCharacter.SetMyInteractingState(false);
        waitTimeToNextBehaviour = Random.Range(minTimeToNextBehaviour, maxTimeToNextBehaviour);
        initialQuaternion = transform.rotation;
        elapsedTime = 0f;
    }

    public override void OnUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("마스터라 실행중~");
            elapsedTime += Time.deltaTime;
        }

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