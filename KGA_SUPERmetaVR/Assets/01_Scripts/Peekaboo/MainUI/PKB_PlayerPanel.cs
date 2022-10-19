using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class PKB_PlayerPanel : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI playerNameText;
    [SerializeField] Image hostImage;
    [SerializeField] GameObject ReadyPanel;
    public Button kickButton;
    

    void Start()
    {
        hostImage.gameObject.SetActive(false);
        kickButton.gameObject.SetActive(false);
        ReadyPanel.gameObject.SetActive(false);
    }

    public void SetPlayerInfo(Player _player)
    {
        playerNameText.text = GameManager.Instance.PlayerData.Nickname.ToString();
        _player.NickName = playerNameText.text;
    }
}
