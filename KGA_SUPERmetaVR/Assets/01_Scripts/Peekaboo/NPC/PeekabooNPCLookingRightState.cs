using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooNPCLookingRightState : PeekabooNPCState
{
    [SerializeField]
    private float viewAngle;

    private float viewAngleHalf;
    public float fromFrontToRightRotateMinTime;
    public float fromFrontToRightRotateMaxTime;
    public float minTimeToLookingRight;
    public float maxTimeToLookingRight;
    public float fromRightToFrontRotateMinTime;
    public float fromRightToFrontRotateMaxTime;
    private Vector3 eulerAngleToRotate;
    private Quaternion targetQuaternion;
    private Quaternion initialQuaternion;
    public float fromFrontToRightRotateTime;
    public float timeToLookingRight;
    public float fromRightToFrontRotateTime;
    private float elapsedTime;

    private void Awake()
    {
        viewAngleHalf = viewAngle / 2;
        fromFrontToRightRotateMinTime = StaticData.GetNPCData(5).VALUE;
        fromFrontToRightRotateMaxTime = StaticData.GetNPCData(6).VALUE;
        minTimeToLookingRight = StaticData.GetNPCData(13).VALUE;
        maxTimeToLookingRight = StaticData.GetNPCData(14).VALUE;
        fromRightToFrontRotateMinTime = StaticData.GetNPCData(7).VALUE;
        fromRightToFrontRotateMaxTime = StaticData.GetNPCData(8).VALUE;
    }

    public override void OnEnter()
    {
        eulerAngleToRotate = new Vector3(0f, viewAngleHalf, 0f);
        targetQuaternion = transform.rotation * Quaternion.Euler(eulerAngleToRotate);
        fromFrontToRightRotateTime = Random.Range(fromFrontToRightRotateMinTime, fromFrontToRightRotateMaxTime);
        timeToLookingRight = Random.Range(minTimeToLookingRight, maxTimeToLookingRight);
        fromRightToFrontRotateTime = Random.Range(fromRightToFrontRotateMinTime, fromRightToFrontRotateMaxTime);

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
        initialQuaternion = transform.rotation;
        elapsedTime = 0f;

        while (elapsedTime <= fromFrontToRightRotateTime)
        {
            elapsedTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(initialQuaternion, _target, elapsedTime / fromFrontToRightRotateTime);

            yield return null;
        }

        transform.rotation = _target;

        StartCoroutine(WaitForReturnQuaternionCoroutine(timeToLookingRight));
    }

    private IEnumerator WaitForReturnQuaternionCoroutine(float _waitTime)
    {
        yield return new WaitForSeconds(_waitTime);

        targetQuaternion = initialQuaternion;
        StartCoroutine(RotateToInitialQuaternionCoroutine(targetQuaternion));
    }

    private IEnumerator RotateToInitialQuaternionCoroutine(Quaternion _target)
    {
        initialQuaternion = transform.rotation;
        elapsedTime = 0f;

        while (elapsedTime <= fromRightToFrontRotateTime)
        {
            elapsedTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(initialQuaternion, _target, elapsedTime / fromRightToFrontRotateTime);

            yield return null;
        }

        transform.rotation = _target;

        myFSM.ChangeState(PEEKABOONPCSTATE.IDLE);
    }
}