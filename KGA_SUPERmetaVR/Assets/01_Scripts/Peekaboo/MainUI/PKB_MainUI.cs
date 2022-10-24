using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Linq;

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

    [Header("커스텀 캐릭터")]
    [SerializeField] GameObject[] customCharacter;

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
        RefreshUI();
    }

    public override void OnConnectedToMaster()
    {
        exitButton.interactable = true;
        findRoomButton.interactable = true;
        customizingButton.interactable = true;
        randomJoinButton.interactable = true;
        createRoomButton.interactable = true;
        settingButton.interactable = true;
        PKB_MainUIManager.Instance.Fade(true);
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

            for (int i = 0; i < LobbyManager.Instance.NowRooms.Count; i++)
            {
                if (LobbyManager.Instance.NowRooms[i].CustomProperties["Password"] == null)
                {
                    string roomName = LobbyManager.Instance.NowRooms.ElementAt(i).CustomProperties.Values.ElementAt(0).ToString();
                    if (PhotonNetwork.JoinRoom(roomName))
                    {
                        findingRoomImage.SetActive(false);
                        return;
                    }
                }
            }
            PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "현재 입장할 수 있는 방이 없습니다.", "확인");
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

    public void ChangeCustomCharacter()
    {
        for (int i = 0; i < customCharacter.Length; i++)
        {
            customCharacter[i].SetActive(false);
        }
        customCharacter[GameManager.Instance.PlayerData.PlayerPeekabooData.SelectCharacter].SetActive(true);
    }

    public void RefreshUI()
    {
        ChangeCustomCharacter();
    }
}
