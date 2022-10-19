using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PeekabooPC : PeekabooCharacter
{
    [SerializeField]
    private GameObject attackTarget;

    private void Awake()
    {
        BaseInitialize();
    }

    protected override void Initialize()
    {

    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            myFSM.UpdateFSM(); 
        }
    }

    private void OnTriggerStay(Collider _other)
    {
        if (photonView.IsMine && IsInteracting == false)
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
    }

    private void OnTriggerExit(Collider _other)
    {
        if (photonView.IsMine && IsInteracting == false)
        {
            if (CheckTarget(_other.gameObject))
            {
                IsLookingSomeone = false;
                LookingTarget = null;
                myFSM.ChangeState(PEEKABOOCHARACTERSTATE.IDLE);
            }
        }
    }

    public void Attack()
    {
        if (IsInteracting == false)
        {
            photonView.RPC("ChangeMyInteractState", RpcTarget.All, true);
            AttackTarget = attackTarget;
            // AttackTarget에 레이캐스트 쏴서 맞은 대상 캐릭터 저장 필요
            myFSM.ChangeState(PEEKABOOCHARACTERSTATE.PCROTATETOTARGET);
        }
    }

    [PunRPC]
    private void ChangeMyInteractState(bool _state)
    {
        IsInteracting = _state;
    }

    public override void TakeDamage(GameObject _attacker)
    {
        if (IsInteracting == false)
        {
            photonView.RPC("ChangeMyInteractState", Photon.Pun.RpcTarget.All, true);
            Attacker = _attacker;
            myFSM.ChangeState(PEEKABOOCHARACTERSTATE.PCROTATETOATTACKER);
        }
    }
}