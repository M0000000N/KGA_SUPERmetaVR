using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class PeekabooNPC : PeekabooCharacter
{
    public bool IsMoving { get; private set; }

    [SerializeField]
    private PeekabooNPCMove myMove; 

    private void Awake()
    {
        BaseInitialize();
    }
    protected override void Initialize()
    {
        IsMoving = false;
    }

    [PunRPC]
    private void ChangeMyInteractState(bool _state)
    {
        IsInteracting = _state;
    }

    private void Update()
    {
        if (IsMoving)
        {
            if (myMove.CheckArrival(transform.position))
            {
                IsMoving = false;
            }
        }
        else
        {
            Action action = myMove.SetNextDestination;
            StartCoroutine(WaitForSetNextDestination(1f, action));
        }

        myFSM.UpdateFSM();

         
    }

    private void OnTriggerStay(Collider _other)
    {
        if (_other.tag == "PC" || _other.tag == "NPC")
        {
            if (CheckMyFieldOfView(_other.transform.position))
            {
                IsLookingSomeone = true;
                LookingTarget = _other.gameObject;
                myFSM.ChangeState(PEEKABOOCHARACTERSTATE.ROTATETOSOMEONE);
            }
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        if (CheckTarget(_other.gameObject))
        {
            IsLookingSomeone = false;
            LookingTarget = null;
            myFSM.ChangeState(PEEKABOOCHARACTERSTATE.IDLE);
        }
    }

    public override void TakeDamage(GameObject _attacker)
    {
        if (IsInteracting == false)
        {
            photonView.RPC("ChangeMyInteractState", RpcTarget.All, true);
            Attacker = _attacker;
            myFSM.ChangeState(PEEKABOOCHARACTERSTATE.NPCLAUGHT);
        }
        else
        {
            Debug.Log("공격 대상은 현재 다른 캐릭터와 상호작용중입니다!");
        }
    }

    public IEnumerator WaitForSetNextDestination(float _time, Action _method)
    {
        yield return new WaitForSeconds(_time);

        _method.Invoke();
        IsMoving = true;
    }



}