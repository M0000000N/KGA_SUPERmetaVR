using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public override void TakeDamage(GameObject _attacker)
    {
        if (IsInteracting == false)
        {
            IsInteracting = true;
            Attacker = _attacker;
            myFSM.ChangeState(PEEKABOOCHARACTERSTATE.PCROTATETOATTACKER);
        }
    }
}