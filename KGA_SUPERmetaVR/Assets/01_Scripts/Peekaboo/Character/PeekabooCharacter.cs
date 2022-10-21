using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class PeekabooCharacter : MonoBehaviourPun
{
    public bool IsLookingSomeone { get; protected set; }
    public bool IsInteracting { get; protected set; }
    public float ViewAngleHalf { get; protected set; }
    public GameObject LookingTarget { get; protected set; }
    public GameObject Attacker { get; protected set; }
    public GameObject AttackTarget { get; protected set; }

    protected PeekabooCharacterFSM myFSM;

    protected void BaseInitialize()
    {
        IsLookingSomeone = false;
        IsInteracting = false;
        ViewAngleHalf = 70f;

        myFSM = GetComponent<PeekabooCharacterFSM>();

        Initialize();
    }

    protected abstract void Initialize();

    protected bool CheckMyFieldOfView(Vector3 _targetPosition)
    {
        Vector3 direction = _targetPosition - transform.position;
        float angleToTarget = Vector3.Angle(transform.forward, direction);

        if (angleToTarget <= ViewAngleHalf)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected bool CheckTarget(GameObject _target)
    {
        if (_target == LookingTarget)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetMyInteractingState(bool _state)
    {
        photonView.RPC("ChangeMyInteractState", RpcTarget.All, false);
    }

    public abstract void TakeDamage(GameObject _attacker);
}