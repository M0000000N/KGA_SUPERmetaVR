using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class testLobby : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Ver.1_Lobby");
    }
}