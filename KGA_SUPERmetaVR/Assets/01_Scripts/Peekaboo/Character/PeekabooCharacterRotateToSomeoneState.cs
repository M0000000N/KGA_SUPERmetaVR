using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooCharacterRotateToSomeoneState : PeekabooCharacterState
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

        transform.rotation = Quaternion.Lerp(initialQuaternion, targetQuaternion, Time.deltaTime);
    }

    public override void OnExit()
    {
        StopAllCoroutines();
    }
}