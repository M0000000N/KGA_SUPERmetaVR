 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LobbyPlayerCulling : MonoBehaviourPun
{

    [SerializeField]
    private GameObject LobbyPlayerModeling;

    private void Awake()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) return;
    }

    private void Start()
    {
        if (photonView.IsMine == true && PhotonNetwork.IsConnected == true)
            CullingModeling();
    }

    public void CullingModeling()
    {
        LobbyPlayerModeling.SetActive(false);
    }
}
