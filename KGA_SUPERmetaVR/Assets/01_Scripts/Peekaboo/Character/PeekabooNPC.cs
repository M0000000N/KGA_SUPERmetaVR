using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PeekabooNPC : PeekabooCharacter
{
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

    public override void TakeDamage(GameObject _attacker)
    {
        if (IsInteracting == false)
        {
            IsInteracting = true;
            Attacker = _attacker;
            myFSM.ChangeState(PEEKABOOCHARACTERSTATE.NPCLAUGHT);
        }
    }
}