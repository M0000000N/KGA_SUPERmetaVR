using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVoiceData : MonoBehaviourPun
{
   
    //[HideInInspector]
    public string userID;
    //[HideInInspector]
    public int actNumber;
    //[HideInInspector]
    public int interestGroup;

    private void Start()
    {
        Player player = PhotonNetwork.PlayerList[PhotonNetwork.PlayerList.Length - 1];
        SetPlayerData(player.UserId, player.ActorNumber);
    }

    public void SetPlayerData(string _userID, int _actNumber)
    {
        userID = _userID;
        actNumber = _actNumber;
    }
}
