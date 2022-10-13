using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class WaitingRoomUI : MonoBehaviourPunCallbacks
{
    [Header("DB���� ������ ��")]
    [SerializeField] TextMeshProUGUI coin;
    [SerializeField] TextMeshProUGUI nickname;

    [Header("��ư")]
    [SerializeField] Button exitButton;
    [SerializeField] Button findRoomButton;
    [SerializeField] Button customizingButton;
    [SerializeField] Button randomJoinButton;
    [SerializeField] Button createRoomButton;
    [SerializeField] Button settingButton;

    [Header("�� ã��")]
    [SerializeField] GameObject findingRoomImage;
    private Button XButton;



    private void Awake()
    {
        coin.text = GameManager.Instance.PlayerData.Coin.ToString();
        nickname.text = GameManager.Instance.PlayerData.Nickname.ToString();
        findingRoomImage.SetActive(false);

        XButton = findingRoomImage.GetComponentInChildren<Button>();
    }

    private void Start()
    {
        exitButton.onClick.AddListener(OnClickExitButton);
        findRoomButton.onClick.AddListener(OnClickFindRoomButton);
        customizingButton.onClick.AddListener(OnClickCustomizingButton);
        randomJoinButton.onClick.AddListener(OnClickRandomJoinButton);
        createRoomButton.onClick.AddListener(OnClickCreateRoomButton);
        XButton.onClick.AddListener(OnClickXButton);
        settingButton.onClick.AddListener(OnClickSettingButton);
    }

    public void OnClickExitButton()
    {
        Peekaboo_WatingRoomUIManager.Instance.ExitUI.gameObject.SetActive(true);
    }

    public void OnClickFindRoomButton()
    {
        Peekaboo_WatingRoomUIManager.Instance.FindRoomUI.gameObject.SetActive(true);
    }

    public void OnClickCustomizingButton()
    {
        Peekaboo_WatingRoomUIManager.Instance.CustomizingUI.gameObject.SetActive(true);
    }

    public void OnClickCreateRoomButton()
    {
        Peekaboo_WatingRoomUIManager.Instance.CreateRoomUI.gameObject.SetActive(true);
    }

    // �׽�Ʈ �ڵ�
    public readonly RoomOptions RoomOptions = new RoomOptions()
    {
        IsOpen = true,
        IsVisible = true,
        MaxPlayers = 14
    };

    public void OnClickRandomJoinButton()
    {
        findingRoomImage.SetActive(true);

        //�׽�Ʈ �ڵ�
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinOrCreateRoom("meta", RoomOptions, TypedLobby.Default);

            PhotonNetwork.LoadLevel("01_Main");
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void OnClickSettingButton()
    {
        Peekaboo_WatingRoomUIManager.Instance.SettingUI.gameObject.SetActive(true);
    }

    public void OnClickXButton()
    {
        findingRoomImage.SetActive(false);
        // ���� PhotonNetwork.JoinRandomRoom() �߿� ���� �� �ִ� ��� ���
        //PhotonNetwork.LeaveRoom();
    }
}
