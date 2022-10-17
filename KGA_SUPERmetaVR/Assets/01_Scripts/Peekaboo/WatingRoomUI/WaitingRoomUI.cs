using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class WaitingRoomUI : MonoBehaviourPunCallbacks
{
    [Header("DB에서 가져올 것")]
    [SerializeField] TextMeshProUGUI coin;
    [SerializeField] TextMeshProUGUI nickname;

    [Header("버튼")]
    [SerializeField] Button exitButton;
    [SerializeField] Button findRoomButton;
    [SerializeField] Button customizingButton;
    [SerializeField] Button randomJoinButton;
    [SerializeField] Button createRoomButton;
    [SerializeField] Button settingButton;

    [Header("방 찾기")]
    [SerializeField] GameObject findingRoomImage;

    private void Awake()
    {
        coin.text = GameManager.Instance.PlayerData.Coin.ToString();
        nickname.text = GameManager.Instance.PlayerData.Nickname.ToString();
        findingRoomImage.SetActive(false);
    }

    private void Start()
    {
        exitButton.onClick.AddListener(OnClickExitButton);
        findRoomButton.onClick.AddListener(OnClickFindRoomButton);
        customizingButton.onClick.AddListener(OnClickCustomizingButton);
        randomJoinButton.onClick.AddListener(OnClickRandomJoinButton);
        createRoomButton.onClick.AddListener(OnClickCreateRoomButton);
        settingButton.onClick.AddListener(OnClickSettingButton);
    }

    public void OnClickExitButton()
    {
        Peekaboo_WaitingRoomUIManager.Instance.ExitUI.gameObject.SetActive(true);
    }

    public void OnClickFindRoomButton()
    {
        Peekaboo_WaitingRoomUIManager.Instance.FindRoomUI.gameObject.SetActive(true);
    }

    public void OnClickCustomizingButton()
    {
        Peekaboo_WaitingRoomUIManager.Instance.CustomizingUI.gameObject.SetActive(true);
    }

    public void OnClickCreateRoomButton()
    {
        Peekaboo_WaitingRoomUIManager.Instance.CreateRoomUI.gameObject.SetActive(true);
    }

    public void OnClickRandomJoinButton()
    {
        findingRoomImage.SetActive(true);

        if (PhotonNetwork.IsConnected)
        {
            Hashtable myHashtable = new Hashtable() {
            { "isPrivate", false },
            { "password", null } };
            PhotonNetwork.JoinRandomRoom(myHashtable, 0);

            findingRoomImage.SetActive(false);
            gameObject.SetActive(false);
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void OnClickSettingButton()
    {
        Peekaboo_WaitingRoomUIManager.Instance.SettingUI.gameObject.SetActive(true);
    }
}
