using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooNPCLookingLeftState : PeekabooNPCState
{
    [SerializeField]
    private float viewAngle;

    private float viewAngleHalf;
    public float fromFrontToLeftRotateMinTime;
    public float fromFrontToLeftRotateMaxTime;
    public float minTimeToLookingLeft;
    public float maxTimeToLookingLeft;
    public float fromLeftToFrontRotateMinTime;
    public float fromLeftToFrontRotateMaxTime;
    public float minTimeToLookingFront;
    public float maxTimeToLookingFront;
    private Vector3 eulerAngleToRotate;
    private Quaternion targetQuaternion;
    private Quaternion initialQuaternion;
    public float fromFrontToLeftRotateTime;
    public float timeToLookingLeft;
    public float fromLeftToFrontRotateTime;
    public float timeToLookingFront;
    private float elapsedTime;

    private void Awake()
    {
        viewAngleHalf = viewAngle / 2;
        fromFrontToLeftRotateMinTime = StaticData.GetNPCData(1).VALUE;
        fromFrontToLeftRotateMaxTime = StaticData.GetNPCData(2).VALUE;
        minTimeToLookingLeft = StaticData.GetNPCData(11).VALUE;
        maxTimeToLookingLeft = StaticData.GetNPCData(12).VALUE;
        fromLeftToFrontRotateMinTime = StaticData.GetNPCData(3).VALUE;
        fromLeftToFrontRotateMaxTime = StaticData.GetNPCData(4).VALUE;
        minTimeToLookingFront = StaticData.GetNPCData(9).VALUE;
        maxTimeToLookingFront = StaticData.GetNPCData(10).VALUE;
    }

    public override void OnEnter()
    {
        eulerAngleToRotate = new Vector3(0f, -viewAngleHalf, 0f);
        targetQuaternion = transform.rotation * Quaternion.Euler(eulerAngleToRotate);
        fromFrontToLeftRotateTime = Random.Range(fromFrontToLeftRotateMinTime, fromFrontToLeftRotateMaxTime);
        timeToLookingLeft = Random.Range(minTimeToLookingLeft, maxTimeToLookingLeft);
        fromLeftToFrontRotateTime = Random.Range(fromLeftToFrontRotateMinTime, fromLeftToFrontRotateMaxTime);
        timeToLookingFront = Random.Range(minTimeToLookingFront, maxTimeToLookingFront);

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

        while (elapsedTime <= fromFrontToLeftRotateTime)
        {
            elapsedTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(initialQuaternion, _target, elapsedTime / fromFrontToLeftRotateTime);

            yield return null;
        }

        transform.rotation = _target;

        StartCoroutine(WaitForReturnQuaternionCoroutine(timeToLookingLeft));
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

        while (elapsedTime <= fromLeftToFrontRotateTime)
        {
            elapsedTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(initialQuaternion, _target, elapsedTime / fromLeftToFrontRotateTime);

            yield return null;
        }

        transform.rotation = _target;

        yield return new WaitForSeconds(timeToLookingFront);

        myFSM.ChangeState(PEEKABOONPCSTATE.LOOKINGRIGHT);
    }
}