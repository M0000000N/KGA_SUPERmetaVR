using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using System.Linq;

public class PKB_MainUI : MonoBehaviourPunCallbacks
{
    [Header("DB에서 가져올 것")]
    [SerializeField] TextMeshProUGUI nickname;

    [Header("버튼")]
    [SerializeField] Button findRoomButton;
    [SerializeField] Button customizingButton;
    [SerializeField] Button randomJoinButton;
    [SerializeField] Button createRoomButton;
    [SerializeField] Button settingButton;
    [SerializeField] Button exitButton;

    [Header("방 찾기")]
    [SerializeField] GameObject findingRoomImage;

    [Header("커스텀 캐릭터")]
    [SerializeField] GameObject[] customCharacter;

    private void Awake()
    {
        nickname.text = GameManager.Instance.PlayerData.Nickname.ToString();
        findingRoomImage.SetActive(false);
    }

    private void Start()
    {
        findRoomButton.onClick.AddListener(OnClickFindRoomButton);
        customizingButton.onClick.AddListener(OnClickCustomizingButton);
        randomJoinButton.onClick.AddListener(OnClickRandomJoinButton);
        createRoomButton.onClick.AddListener(OnClickCreateRoomButton);
        settingButton.onClick.AddListener(OnClickSettingButton);
        exitButton.onClick.AddListener(OnClickExitButton);

        findRoomButton.interactable = true;
        customizingButton.interactable = true;
        randomJoinButton.interactable = true;
        createRoomButton.interactable = true;
        settingButton.interactable = true;
        exitButton.interactable = true;
        PKB_MainUIManager.Instance.Fade(true);
        RefreshUI();
    }

    public void OnClickFindRoomButton()
    {
        SoundManager.Instance.PlaySE("popup_open.wav");
        PKB_MainUIManager.Instance.FindRoomUI.gameObject.SetActive(true);
    }

    public void OnClickCustomizingButton()
    {
        SoundManager.Instance.PlaySE("popup_open.wav");
        PKB_MainUIManager.Instance.CustomizingUI.gameObject.SetActive(true);
    }

    public void OnClickRandomJoinButton()
    {
        SoundManager.Instance.PlaySE("popup_open.wav");
        if (!PhotonNetwork.IsConnected) return;
        findingRoomImage.SetActive(true);

        for (int i = 0; i < LobbyManager.Instance.NowRooms.Count; i++)
        {
            if (LobbyManager.Instance.NowRooms[i].CustomProperties["RoomName"].ToString().Contains("00")) continue;
            if (LobbyManager.Instance.NowRooms[i].CustomProperties["Password"] == null)
            {
                string roomName = LobbyManager.Instance.NowRooms.ElementAt(i).CustomProperties.Values.ElementAt(0).ToString();
                PhotonNetwork.JoinRoom(roomName);
                findingRoomImage.SetActive(false);
                return;
            }
        }
        PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "현재 입장할 수 있는 방이 없습니다.", "확인");
        findingRoomImage.SetActive(false);
    }

    public void OnClickCreateRoomButton()
    {
        SoundManager.Instance.PlaySE("popup_open.wav");
        PKB_MainUIManager.Instance.CreateRoomUI.gameObject.SetActive(true);
    }

    public void OnClickSettingButton()
    {
        SoundManager.Instance.PlaySE("popup_open.wav");
        PKB_MainUIManager.Instance.SettingUI.gameObject.SetActive(true);
    }

    public void OnClickExitButton()
    {
        SoundManager.Instance.PlaySE("popup_open.wav");
        PKB_MainUIManager.Instance.ExitUI.gameObject.SetActive(true);
    }

    public void ChangeCustomCharacter()
    {
        SoundManager.Instance.PlaySE("popup_open.wav");
        for (int i = 0; i < customCharacter.Length; i++)
        {
            customCharacter[i].SetActive(false);
        }
        customCharacter[GameManager.Instance.PlayerData.PeekabooData.SelectCharacter].SetActive(true);
    }

    public void RefreshUI()
    {
        ChangeCustomCharacter();
    }
}
