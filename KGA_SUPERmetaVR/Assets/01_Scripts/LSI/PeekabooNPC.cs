using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PeekabooNPC : MonoBehaviour, IDamageable
{
    public bool IsLookingSomeone { get; private set; }
    public bool IsInteracting { get; private set; }

    private PeekabooNPCMove myMove;
    private PeekabooNPCFieldOfView myView;
    private PeekabooNPCFSM myFSM;
    private SphereCollider myCollider;
    private GameObject lookingTarget;

    private void Awake()
    {
        IsLookingSomeone = false;
        IsInteracting = false;

        myMove = GetComponent<PeekabooNPCMove>();
        myView = GetComponent<PeekabooNPCFieldOfView>();
        myFSM = GetComponent<PeekabooNPCFSM>();
        myCollider = GetComponent<SphereCollider>();
        lookingTarget = null;
    }

    private void Update()
    {

    }

    private void OnTriggerStay(Collider _other)
    {
        if (myView.CheckView(_other.transform.position))
        {
            myCollider.enabled = false;
            IsLookingSomeone = true;
            lookingTarget = _other.GetComponent<GameObject>();
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other == lookingTarget)
        {
            myCollider.enabled = true;
            IsLookingSomeone = false;
            lookingTarget = null;
        }
    }

    public void TakeDamage(GameObject _attacker)
    {
        IsInteracting = true;
        myFSM.SetCounterAttackTarget(_attacker);
        myFSM.ChangeState(PEEKABOONPCSTATE.TAKEDAMAGE);
    }
}