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
    [SerializeField] Image hostImage;
    [SerializeField] GameObject ReadyPanel;
    public Button kickButton;

    public Player Player { get; private set; }
    public bool Ready = false;

    void Start()
    {
        hostImage.gameObject.SetActive(false);
        kickButton.gameObject.SetActive(false);
        ReadyPanel.gameObject.SetActive(false);
    }

    public void SetPlayerInfo(Player _player)
    {
        _player.NickName = GameManager.Instance.PlayerData.Nickname.ToString();
        playerNameText.text = _player.NickName;
        Player = _player;

        if (_player.CustomProperties.ContainsKey("IsReady") == false)
        {
            _player.CustomProperties.Add("IsReady", false);
        }

        int result = -1;
        if (_player.CustomProperties.ContainsKey("IsReady"))
        {
            // result = (int)_player.CustomProperties["IsReady"];
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
}
