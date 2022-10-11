using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooNPCLookingLeftSideState : PeekabooNPCState
{
    private float viewAngleHalf;
    private Vector3 rotateVector3;
    private Quaternion initializeQuaternion;
    private Quaternion targetQuaternion;

    public override void OnEnter()
    {
        viewAngleHalf = myFSM.getMyData.ViewAngle / 2;
        rotateVector3 = new Vector3(0f, -viewAngleHalf, 0f);
        initializeQuaternion = transform.rotation;
        targetQuaternion = transform.rotation * Quaternion.Euler(rotateVector3);

        StartCoroutine(RotateToTargetQuaternionCoroutine(targetQuaternion));
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {

    }

    private IEnumerator RotateToTargetQuaternionCoroutine(Quaternion _target)
    {
        Quaternion initialQuaternion = transform.rotation;
        float  elapsedTime = 0f;

        while (elapsedTime <= myFSM.getMyData.LeftAngleReachTime)
        {
            yield return null;

            elapsedTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(initialQuaternion, _target, elapsedTime / myFSM.getMyData.LeftAngleReachTime);
        }

        transform.rotation = targetQuaternion;

        StartCoroutine(WaitForReturnQuaternionCoroutine(myFSM.getMyData.LookingLeftSideTime));
    }



    private IEnumerator WaitForReturnQuaternionCoroutine(float _waitTime)
    {
        yield return new WaitForSeconds(_waitTime);

        targetQuaternion = initializeQuaternion;
        StartCoroutine(RotateToTargetQuaternionCoroutine(targetQuaternion));
    }
}