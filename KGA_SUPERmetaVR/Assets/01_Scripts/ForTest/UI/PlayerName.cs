using Photon.Realtime;
using Photon.Pun;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerName : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI playerText;

    public Player Player { get; private set; }

    public void SetPlayerInfo(Player _player)
    {
        Player = _player;
        playerText.text = _player.NickName;
    }
}
