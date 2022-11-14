using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooCharacterLookingSomeoneState : PeekabooCharacterState
{
    private GameObject target;
    private Quaternion initialQuaternion;
    private Vector3 directionToTarget;
    private Quaternion targetQuaternion;
    private float elapsedTime;

    protected override void Initialize()
    {

    }

    public override void OnEnter()
    {
        target = myFSM.MyCharacter.LookingTarget;
        initialQuaternion = transform.rotation;
        elapsedTime = 0f;
    }

    public override void OnUpdate()
    {
        directionToTarget = target.transform.position - transform.position;
        targetQuaternion = Quaternion.LookRotation(directionToTarget);

        elapsedTime += Time.deltaTime;
        transform.rotation = Quaternion.Lerp(initialQuaternion, targetQuaternion, elapsedTime);
    }

    public override void OnExit()
    {
        StopAllCoroutines();
    }
}