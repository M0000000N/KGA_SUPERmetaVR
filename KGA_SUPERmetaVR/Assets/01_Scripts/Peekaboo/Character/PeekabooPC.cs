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
        if (photonView.IsMine && IsLookingSomeone == false && IsInteracting == false)
        {
            if (_other.tag == "Player" || _other.tag == "Enemy")
            {
                if (CheckMyFieldOfView(_other.transform.position))
                {
                    IsLookingSomeone = true;
                    LookingTarget = _other.gameObject;
                    myFSM.ChangeState(PEEKABOOCHARACTERSTATE.LOOKINGSOMEONE);
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
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("������ Ŭ���̾�Ʈ�� PC Attack ����!");
                photonView.RPC("ChangeMyInteractState", RpcTarget.All, true);
                AttackTarget = attackTarget;
                // AttackTarget�� ����ĳ��Ʈ ���� ���� ��� ĳ���� ���� �ʿ�
                myFSM.ChangeState(PEEKABOOCHARACTERSTATE.PCROTATETOTARGET);
            }
            else
            {
                Debug.Log("������ Ŭ���̾�Ʈ�� �ƴ϶� PC AttackRPC ����!");
                photonView.RPC("AttackRPC", RpcTarget.MasterClient);
            }
        }
    }

    [PunRPC]
    private void ChangeMyInteractState(bool _state)
    {
        IsInteracting = _state;
    }

    [PunRPC]
    private void AttackRPC()
    {
        photonView.RPC("ChangeMyInteractState", RpcTarget.All, true);
        AttackTarget = attackTarget;
        // AttackTarget�� ����ĳ��Ʈ ���� ���� ��� ĳ���� ���� �ʿ�
        myFSM.ChangeState(PEEKABOOCHARACTERSTATE.PCROTATETOTARGET);
    }

    [PunRPC]
    private void TakeDamageRPC(int _attackerViewNumber)
    {
        photonView.RPC("ChangeMyInteractState", Photon.Pun.RpcTarget.All, true);
        Attacker = PhotonView.Find(_attackerViewNumber).gameObject;
        myFSM.ChangeState(PEEKABOOCHARACTERSTATE.PCROTATETOATTACKER);
    }

    //public override void TakeDamage(GameObject _attacker)
    //{
    //    if (IsInteracting == false)
    //    {
    //        photonView.RPC("ChangeMyInteractState", Photon.Pun.RpcTarget.All, true);
    //        Attacker = _attacker;
    //        myFSM.ChangeState(PEEKABOOCHARACTERSTATE.PCROTATETOATTACKER);
    //    }
    //}

    public override void TakeDamage(GameObject _attacker)
    {
        if (IsInteracting == false)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("������ Ŭ���̾�Ʈ�� PC TakeDamage ����!");
                photonView.RPC("ChangeMyInteractState", Photon.Pun.RpcTarget.All, true);
                Attacker = _attacker;
                myFSM.ChangeState(PEEKABOOCHARACTERSTATE.PCROTATETOATTACKER);
            }
            else
            {
                Debug.Log("������ Ŭ���̾�Ʈ�� �ƴ϶� PC TakeDamage RPC ����!");
                int targetViewNumber = _attacker.GetPhotonView().ViewID;
                photonView.RPC("TakeDamageRPC", RpcTarget.MasterClient, targetViewNumber);
            }
        }
    }
}