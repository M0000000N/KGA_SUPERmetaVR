using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

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

    // 테스트 코드
    public readonly RoomOptions RoomOptions = new RoomOptions()
    {
        IsOpen = true,
        IsVisible = true,
        MaxPlayers = 14
    };

    // 빠른매칭 버튼
    public void OnClickRandomJoinButton()
    {
        findingRoomImage.SetActive(true);

        if (PhotonNetwork.IsConnected)
        {
            // 빠른 매칭
            PhotonNetwork.JoinRandomRoom();

            findingRoomImage.SetActive(false);
            gameObject.SetActive(false);
            // PhotonNetwork.JoinOrCreateRoom("meta", RoomOptions, TypedLobby.Default);
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

    public void OnClickXButton()
    {
        findingRoomImage.SetActive(false);
        // 포톤 PhotonNetwork.JoinRandomRoom() 중에 멈출 수 있는 방안 모색
        // PhotonNetwork.LeaveRoom();
    }
}
