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
    [SerializeField] GameObject ReadyPanel;
    public Button kickButton;

    public Player Player { get; private set; }
    public bool Ready = false;

    void Start()
    {
        // 방 준비상태 동기화를 위해 주석처리
        //kickButton.gameObject.SetActive(false);
        //ReadyPanel.gameObject.SetActive(false);
    }

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
        ReadyPanel.gameObject.SetActive(_isActive);
    }
}
