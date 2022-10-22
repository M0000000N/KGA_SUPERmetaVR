using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PeekabooPC : PeekabooCharacter
{
    [SerializeField]
    private GameObject tempTarget;

    private void Awake()
    {
        BaseInitialize();
    }

    protected override void Initialize()
    {

    }

    private void Update()
    {
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

    public void Attack()
    {
        if (IsInteracting == false)
        {
            IsInteracting = true;
            AttackTarget = tempTarget;
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
        else
        {
            Debug.Log("공격 대상은 현재 다른 캐릭터와 상호작용중입니다!");
        }
    }
}