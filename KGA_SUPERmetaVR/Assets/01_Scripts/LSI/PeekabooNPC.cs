using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PeekabooNPC : MonoBehaviour
{
    [SerializeField]
    private float waitTimeForNextMove;

    public bool IsLookingSomeone { get; private set; }
    public bool IsInteracting { get; private set; }

    private PeekabooNPCMove myMove;
    private PeekabooNPCFieldOfView myView;
    private PeekabooNPCFSM myFSM;
    private SphereCollider myCollider;
    private GameObject lookingTarget;
    private float elapsedTime;

    private void Awake()
    {
        IsLookingSomeone = false;
        IsInteracting = false;

        myMove = GetComponent<PeekabooNPCMove>();
        myView = GetComponent<PeekabooNPCFieldOfView>();
        myFSM = GetComponent<PeekabooNPCFSM>();
        myCollider = GetComponent<SphereCollider>();
        lookingTarget = null;
        elapsedTime = 0f;
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            elapsedTime += Time.deltaTime;
            if (waitTimeForNextMove <= elapsedTime)
            {
                myMove.SetNextDestination();
                elapsedTime = 0f;
            }
            if (IsLookingSomeone)
            {
                transform.LookAt(lookingTarget.transform);
            }

            myFSM.UpdateFSM();
        }
    }

    private void OnTriggerStay(Collider _other)
    {
        if (IsLookingSomeone == false && _other.tag == "NPC")
        {
            if (myView.CheckView(_other.transform.position))
            {
                IsLookingSomeone = true;
                if (myFSM.nowStateKey != PEEKABOONPCSTATE.IDLE)
                {
                    myFSM.ChangeState(PEEKABOONPCSTATE.IDLE);
                }
                lookingTarget = _other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.gameObject == lookingTarget)
        {
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