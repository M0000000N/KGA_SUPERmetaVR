using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class testLobby : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.IsConnected)
        {
            RoomOptions roomOptions = new RoomOptions()
            {
                IsOpen = true,
                IsVisible = true,
                MaxPlayers = 20,
                BroadcastPropsChangeToAll = true
            };

            roomOptions.CustomRoomProperties = new Hashtable()
            {
                { "RoomName", "0" },
                // { "IsInRoom",  isInRoom } 추후 튕길 때 사용
            };

            roomOptions.CustomRoomPropertiesForLobby = new string[]
            {
                "RoomName",
                // "IsInRoom"
            };
            PhotonNetwork.JoinOrCreateRoom("0", roomOptions, TypedLobby.Default);
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Ver.1_Lobby");
    }
}