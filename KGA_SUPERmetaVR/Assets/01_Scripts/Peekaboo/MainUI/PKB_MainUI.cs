using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PKB_MainUI : MonoBehaviourPunCallbacks
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
        createRoomButton.onClick.AddListener(OnClickCreateRoomButton);
        randomJoinButton.onClick.AddListener(OnClickRandomJoinButton);
        settingButton.onClick.AddListener(OnClickSettingButton);

        exitButton.interactable = false;
        findRoomButton.interactable = false;
        customizingButton.interactable = false;
        randomJoinButton.interactable = false;
        createRoomButton.interactable = false;
        settingButton.interactable = false;
    }
    public override void OnConnectedToMaster()
    {
        exitButton.interactable = true;
        findRoomButton.interactable = true;
        customizingButton.interactable = true;
        randomJoinButton.interactable = true;
        createRoomButton.interactable = true;
        settingButton.interactable = true;
    }

    public void OnClickExitButton()
    {
        PKB_MainUIManager.Instance.ExitUI.gameObject.SetActive(true);
    }

    public void OnClickFindRoomButton()
    {
        PKB_MainUIManager.Instance.FindRoomUI.gameObject.SetActive(true);
    }

    public void OnClickCustomizingButton()
    {
        PKB_MainUIManager.Instance.CustomizingUI.gameObject.SetActive(true);
    }

    public void OnClickCreateRoomButton()
    {
        PKB_MainUIManager.Instance.CreateRoomUI.gameObject.SetActive(true);
    }

    public void OnClickRandomJoinButton()
    {
        findingRoomImage.SetActive(true);

        if (PhotonNetwork.IsConnected)
        {
            Hashtable myHashtable = new Hashtable() {
            { "Password", null } };
            PhotonNetwork.JoinRandomRoom(myHashtable, 0);

            findingRoomImage.SetActive(false);
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void OnClickSettingButton()
    {
        PKB_MainUIManager.Instance.SettingUI.gameObject.SetActive(true);
    }
}
