using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayer : MonoBehaviour
{
    private LobbyPlayerFSM myFSM;

    private void Awake()
    {
        myFSM = GetComponent<LobbyPlayerFSM>();
    }

    private void Update()
    {
        myFSM.UpdateFSM();
    }
}