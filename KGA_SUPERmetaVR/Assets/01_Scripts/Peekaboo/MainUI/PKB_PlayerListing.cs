using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class PKB_PlayerListing : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI playerNameText;
    [SerializeField] GameObject readyPanel;
    // public Button KickButton;

    public Player Player { get; private set; }

    public void SetPlayerInfo(Player _player)
    {
        playerNameText.text = _player.NickName;
        Player = _player;

        if (_player.CustomProperties.ContainsKey("IsReady") == false)
        {
            _player.CustomProperties.Add("IsReady", false);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer != null && targetPlayer == Player)
        {
            if (changedProps.ContainsKey("IsReady"))
            {
                SetPlayerInfo(targetPlayer);
            }
        }
    }

    public void ActiveReadyPanel(bool _isActive)
    {
        readyPanel.gameObject.SetActive(_isActive);
    }
}
