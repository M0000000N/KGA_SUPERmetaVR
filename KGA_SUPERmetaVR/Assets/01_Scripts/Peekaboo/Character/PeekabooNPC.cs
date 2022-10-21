using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum PEEKABOONPCMOVESTATE
{
    WAITFORNEXTDESTINATION,
    STARTMOVE,
    MOVING,
    INTERACTING,
}

public class PeekabooNPC : PeekabooCharacter
{
    public PEEKABOONPCMOVESTATE MoveState { get; private set; }

    [SerializeField]
    private PeekabooNPCMove myMove;

    public delegate void SetDestination();
    private SetDestination method;

    private void Awake()
    {
        BaseInitialize();
    }

    protected override void Initialize()
    {
        MoveState = PEEKABOONPCMOVESTATE.WAITFORNEXTDESTINATION;
        method = myMove.SetNextDestination;
        StartCoroutine(WaitForSetNextDestination(1f, method));
    }

    [PunRPC]
    private void ChangeMyInteractState(bool _state)
    {
        IsInteracting = _state;
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && IsInteracting == false)
        {
            switch (MoveState)
            {
                case PEEKABOONPCMOVESTATE.WAITFORNEXTDESTINATION:
                    break;

                case PEEKABOONPCMOVESTATE.STARTMOVE:

                    break;

                case PEEKABOONPCMOVESTATE.MOVING:
                    if (myMove.CheckArrival(transform.position))
                    {
                        MoveState = PEEKABOONPCMOVESTATE.WAITFORNEXTDESTINATION;
                        StartCoroutine(WaitForSetNextDestination(1f, method));
                    }
                    break;
            }

            myFSM.UpdateFSM();
        }
    }

    private void OnTriggerStay(Collider _other)
    {
        if (PhotonNetwork.IsMasterClient && IsInteracting == false)
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
        if (PhotonNetwork.IsMasterClient && IsInteracting == false)
        {
            if (CheckTarget(_other.gameObject))
            {
                IsLookingSomeone = false;
                LookingTarget = null;
                myFSM.ChangeState(PEEKABOOCHARACTERSTATE.IDLE);
            }
        }
    }

    [PunRPC]
    public void TakeDamageRPC(int _attackerViewNumber)
    {
        photonView.RPC("ChangeMyInteractState", RpcTarget.All, true);
        Attacker = PhotonView.Find(_attackerViewNumber).gameObject;
        myFSM.ChangeState(PEEKABOOCHARACTERSTATE.NPCLAUGHT);
    }

    public override void TakeDamage(GameObject _attacker)
    {
        if (IsInteracting == false)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("마스터 클라이언트라서 NPC Take Damage 실행!");
                photonView.RPC("ChangeMyInteractState", RpcTarget.All, true);
                Attacker = _attacker;
                myFSM.ChangeState(PEEKABOOCHARACTERSTATE.NPCLAUGHT);
            }
            else
            {
                Debug.Log("마스터 클라이언트가 아니라서 NPC TakeDamage RPC 보냄!");
                int targetViewNumber = _attacker.GetPhotonView().ViewID;
                photonView.RPC("TakeDamageRPC", RpcTarget.MasterClient, targetViewNumber);
            }
        }
    }

    public IEnumerator WaitForSetNextDestination(float _time, SetDestination _method)
    {
        yield return new WaitForSeconds(_time);

        _method();
        MoveState = PEEKABOONPCMOVESTATE.MOVING;
    }
}