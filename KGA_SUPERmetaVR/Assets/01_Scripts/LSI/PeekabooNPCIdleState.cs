using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooNPCIdleState : PeekabooNPCState
{
    private float waitTimeForNextAnimationRoutine;
    private float elapsedTime;

    public override void OnEnter()
    {
        Debug.Log("Idle 상태 돌입!");
        float minTime = myFSM.getMyData.MinWaitTimeForNextAnimation;
        float maxTime = myFSM.getMyData.MaxWaitTimeForNextAnimation;
        waitTimeForNextAnimationRoutine = Random.Range(minTime, maxTime);

        elapsedTime = 0f;
    }

    public override void OnUpdate()
    {
        if (waitTimeForNextAnimationRoutine <= elapsedTime)
        {
            Debug.Log("시간 경과 ! : " + waitTimeForNextAnimationRoutine);

            if (CheckProbability(myFSM.getMyData.AnimationRoutineStartProbability))
            {
                myFSM.ChangeState(PEEKABOONPCSTATE.LOOKINGLEFTSIDE);
            }
            else
            {
                Debug.Log("실패! Idle 상태로 돌아갑니다!");
                myFSM.ChangeState(PEEKABOONPCSTATE.IDLE);
            }
        }

        elapsedTime += Time.deltaTime;
    }

    public override void OnExit()
    {

    }

    private bool CheckProbability(int _startProbability)
    {
        int randomValue = Random.Range(1, 101);
        if (randomValue <= _startProbability)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}