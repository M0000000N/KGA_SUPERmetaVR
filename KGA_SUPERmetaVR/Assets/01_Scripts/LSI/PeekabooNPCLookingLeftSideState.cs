using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooNPCLookingLeftSideState : PeekabooNPCState
{
    private float viewAngleHalf;
    private Vector3 rotateVector3;
    private Quaternion initialQuaternion;
    private Quaternion targetQuaternion;

    public override void OnEnter()
    {
        viewAngleHalf = myFSM.getMyData.ViewAngle / 2;
        rotateVector3 = new Vector3(0f, -viewAngleHalf, 0f);
        initialQuaternion = transform.rotation;
        targetQuaternion = transform.rotation * Quaternion.Euler(rotateVector3);

        StartCoroutine(RotateToTargetQuaternionCoroutine(targetQuaternion));
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {
        StopAllCoroutines();
    }

    private IEnumerator RotateToTargetQuaternionCoroutine(Quaternion _target)
    {
        Debug.Log("좌측 시야각으로 이동중!");
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

    private IEnumerator RotateToInitialQuaternionCoroutine(Quaternion _target)
    {
        Debug.Log("다시 원래 시야각으로 돌아가야지~");
        Quaternion initialQuaternion = transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime <= myFSM.getMyData.LeftAngleReachTime)
        {
            yield return null;

            elapsedTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(initialQuaternion, _target, elapsedTime / myFSM.getMyData.LeftAngleReachTime);
        }

        transform.rotation = targetQuaternion;

        myFSM.ChangeState(PEEKABOONPCSTATE.LOOKINGRIGHTSIDE);
    }

    private IEnumerator WaitForReturnQuaternionCoroutine(float _waitTime)
    {
        Debug.Log("좌측 쳐다봐야지...");
        yield return new WaitForSeconds(_waitTime);

        targetQuaternion = initialQuaternion;
        StartCoroutine(RotateToInitialQuaternionCoroutine(targetQuaternion));
    }
}