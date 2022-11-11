using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class LobbyPlayerState : MonoBehaviourPun
{
    protected LobbyPlayerFSM myFSM;

    public void Initialize(LobbyPlayerFSM _fsm)
    {
        myFSM = _fsm;
    }

    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
}