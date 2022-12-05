using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PeekabooPC : PeekabooCharacter
{
    [SerializeField]
    private LayserPointer layser; 

    private void Awake()
    {
        BaseInitialize();
    }

    protected override void Initialize()
    {

    }

    private void Start()
    {
        peekabooTextObject.SetActive(false);
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
        if (IsLookingSomeone == false && IsInteracting == false)
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
        if (IsInteracting == false)
        {
            if (CheckTarget(_other.gameObject))
            {
                IsLookingSomeone = false;
                LookingTarget = null;
                myFSM.ChangeState(PEEKABOOCHARACTERSTATE.IDLE);
            }
        }
    }

    public void Attack(GameObject _attackTarget)
    {
        if (IsInteracting == false)
        {
            photonView.RPC("ChangeMyInteractState", RpcTarget.All, true);
            AttackTarget = _attackTarget;
            myFSM.ChangeState(PEEKABOOCHARACTERSTATE.PCROTATETOTARGET);
            //if (PhotonNetwork.IsMasterClient)
            //{
            //    photonView.RPC("ChangeMyInteractState", RpcTarget.All, true);
            //    AttackTarget = _attackTarget;
            //    myFSM.ChangeState(PEEKABOOCHARACTERSTATE.PCROTATETOTARGET);
            //}
            //else
            //{
            //    photonView.RPC("AttackRPC", RpcTarget.MasterClient, _attackTarget.GetPhotonView().ViewID);
            //}
        }
    }

    [PunRPC]
    private void ChangeMyInteractState(bool _state)
    {
        IsInteracting = _state;
    }

    [PunRPC]
    private void AppearPeekaboo()
    {
        StartCoroutine(FadeOutPeekaboo());
    }

    [PunRPC]
    private void AttackRPC(int _photonViewNumber)
    {
        photonView.RPC("ChangeMyInteractState", RpcTarget.All, true);
        AttackTarget = PhotonView.Find(_photonViewNumber).gameObject;
        myFSM.ChangeState(PEEKABOOCHARACTERSTATE.PCROTATETOTARGET);
    }

    [PunRPC]
    private void TakeDamageRPC(int _attackerViewNumber)
    {
        photonView.RPC("ChangeMyInteractState", Photon.Pun.RpcTarget.All, true);
        photonView.RPC("AppearPeekaboo", RpcTarget.All);
        Attacker = PhotonView.Find(_attackerViewNumber).gameObject;
        myFSM.ChangeState(PEEKABOOCHARACTERSTATE.PCROTATETOATTACKER);
    }

    public override void TakeDamage(GameObject _attacker)
    {
        if (IsInteracting == false)
        {
            int targetViewNumber = _attacker.GetPhotonView().ViewID;
            photonView.RPC("TakeDamageRPC", photonView.Owner, targetViewNumber);
            //if (PhotonNetwork.IsMasterClient)
            //{
            //    photonView.RPC("ChangeMyInteractState", Photon.Pun.RpcTarget.All, true);
            //    photonView.RPC("AppearPeekaboo", RpcTarget.All);
            //    Attacker = _attacker;
            //    myFSM.ChangeState(PEEKABOOCHARACTERSTATE.PCROTATETOATTACKER);
            //}
            //else
            //{
            //    int targetViewNumber = _attacker.GetPhotonView().ViewID;
            //    photonView.RPC("TakeDamageRPC", RpcTarget.MasterClient, targetViewNumber);
            //}
        }
    }
}