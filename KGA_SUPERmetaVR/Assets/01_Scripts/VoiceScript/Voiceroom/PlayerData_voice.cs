using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData_voice : MonoBehaviour
{
    //[HideInInspector]
    public string userID;
    //[HideInInspector]
    public int actNumber;
    //[HideInInspector]
    public string Nickname; 

    private void Start()
    {
        Player player = PhotonNetwork.PlayerList[PhotonNetwork.PlayerList.Length - 1];
        SetPlayerData(player.UserId, player.ActorNumber, player.NickName);
    }

    public void SetPlayerData(string _userID, int _actNumber, string _Nickname)
    {
        userID = _userID;
        actNumber = _actNumber;
        Nickname = _Nickname;
    }
}