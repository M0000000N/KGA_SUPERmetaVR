using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public abstract class PeekabooCharacter : MonoBehaviourPun
{
    public bool IsLookingSomeone { get; protected set; }
    public bool IsInteracting { get; protected set; }
    public float ViewAngleHalf { get; protected set; }
    public GameObject LookingTarget { get; protected set; }
    public GameObject Attacker { get; protected set; }
    public GameObject AttackTarget { get; protected set; }

    [SerializeField]
    protected GameObject peekabooTextObject;

    protected PeekabooCharacterFSM myFSM;

    //테스트용 
    public PeekabooCharacterFSM MyFSM { get { return myFSM;} set { myFSM = value; } }

    protected void BaseInitialize()
    {
        IsLookingSomeone = false;
        IsInteracting = false;
        ViewAngleHalf = 70f;

        myFSM = GetComponent<PeekabooCharacterFSM>();

        Initialize();
    }

    protected abstract void Initialize();

    protected bool CheckMyFieldOfView(Vector3 _targetPosition)
    {
        Vector3 direction = _targetPosition - transform.position;
        float angleToTarget = Vector3.Angle(transform.forward, direction);

        if (angleToTarget <= ViewAngleHalf)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected bool CheckTarget(GameObject _target)
    {
        if (_target == LookingTarget)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetMyInteractingState(bool _state)
    {
        photonView.RPC("ChangeMyInteractState", RpcTarget.All, false);
    }

    public abstract void TakeDamage(GameObject _attacker);

    protected IEnumerator FadeOutPeekaboo()
    {
        peekabooTextObject.SetActive(true);
        for (float f = 1f; f > 0; f -= 0.02f)
        {
            Color c = peekabooTextObject.GetComponent<Image>().color;
            c.a = f;
            peekabooTextObject.GetComponent<Image>().color = c;
            yield return null;
        }
    }
}