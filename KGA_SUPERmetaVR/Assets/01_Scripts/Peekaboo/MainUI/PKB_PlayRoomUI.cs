#define 테스트용
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PKB_PlayRoomUI : MonoBehaviourPunCallbacks
{
    public RoomInfo RoomInfo { get; private set; }
    public int MinPlayercount;

    [Header("PlayRoom")]
    [SerializeField] TextMeshProUGUI RoomNameText;
    [SerializeField] Button gameStartButton;
    private TextMeshPro gameStartButtonText;
    [SerializeField] Button exitRoomButton;
    [SerializeField] TextMeshProUGUI RoomTypeText;

    [Header("ExitRoom")]
    [SerializeField] GameObject exitRoomUI;
    [SerializeField] Button YesExitRoomButton;
    [SerializeField] Button NoExitRoomButton;

    [Header("PlayerList")]
    [SerializeField] PKB_PlayerPanel playerPanel;

    Player player; // TODO : 내가 누굴까?

    private void Awake()
    {
        gameStartButton.onClick.AddListener(OnClickStartButton);
        exitRoomButton.onClick.AddListener(OnClickExitButton);
        exitRoomUI.gameObject.SetActive(false);
        YesExitRoomButton.onClick.AddListener(OnClickYesExitRoomButton);
        NoExitRoomButton.onClick.AddListener(OnClickNoExitRoomButton);

        gameStartButton.interactable = false;
        gameStartButtonText = gameStartButton.GetComponentInChildren<TextMeshPro>();
    }

    private void Update()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount > MinPlayercount)
        {
#if 테스트용
            gameStartButton.interactable = true;
#endif

#if 호스트판단
            // 호스트는 게임시작, 일반 플레이어는 준비
            if (PhotonNetwork.LocalPlayer.IsMasterClient && !player.IsLocal)
            {
                gameStartButtonText.text = "게임시작";
            }
            else
            {
                gameStartButtonText.text = "준비";
            }
#endif

        }
        else
        {
            gameStartButton.interactable = true;

        }
    }

    public void initialize()
    {
#if 테스트용
            playerPanel.kickButton.gameObject.SetActive(true);
#endif

#if 호스트판단
        if (PhotonNetwork.LocalPlayer.IsMasterClient && !player.IsLocal)
        {
            playerPanel.kickButton.gameObject.SetActive(true);
        }
        else
        {
            playerPanel.kickButton.gameObject.SetActive(false);
        }
#endif

    }

    [PunRPC]
    public void SetRoomInfo(RoomOptions _roomOptions)
    {
        // 방 이름
        RoomNameText.text = "# " + PhotonNetwork.CurrentRoom.CustomProperties["RoomName"];

        // 방 타입
        if (PhotonNetwork.CurrentRoom.CustomProperties["Password"] == null)
        {
            RoomTypeText.text = "공개방";
        }
        else
        {
            RoomTypeText.text = "비밀방";
        }

        // 게임시작버튼
        gameStartButton.GetComponentInChildren<TextMeshPro>().text = "대기중";
    }

    public void OnClickStartButton()
    {
#if 테스트용
                PhotonNetwork.LoadLevel("Peekaboo_InGame");
#endif

#if 호스트판단
        if (PhotonNetwork.LocalPlayer.IsMasterClient && !player.IsLocal)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount > MinPlayercount)
            {
                gameObject.SetActive(false);
                PhotonNetwork.LoadLevel("Peekaboo_InGame");
            }
            else
            {
                Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "모든 플레이어가 준비완료 상태가 아닙니다.", "확인");
            }
        }
        else
        {
            gameStartButtonText.text = "준비완료";

        }
#endif
    }

    public void OnClickExitButton()
    {
        exitRoomUI.gameObject.SetActive(true);
    }

    public void OnClickYesExitRoomButton()
    {
        PhotonNetwork.LeaveRoom();

        exitRoomUI.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void OnClickNoExitRoomButton()
    {
        exitRoomUI.gameObject.SetActive(false);
    }

#if 추방
    public void kickPlayerButton()
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Add("isKicked", true);
        // player.SetCustomProperties(hashtable);
    }

    public void refreshPlayerNameList()
    {
        // destroyAllPlayerNameRecord();
        Dictionary<int, Player> lists = PhotonNetwork.CurrentRoom.Players;

        if (lists == null || lists.Count == 0)
        {
            return;
        }

        foreach (int key in lists.Keys)
        {
            Player player = lists[key];
            // GameObject obj = Instantiate(playerNameRecord);
            // PlayerNameRecord pnr = obj.GetComponent<PlayerNameRecord>();
            // pnr.setPlayer(player);
            // obj.transform.SetParent(scrollRect.content.transform, false);
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == PhotonNetwork.LocalPlayer)
        {
            // isKicked property가 존재할경우
            if (changedProps["isKicked"] != null)
            {
                // 이게 true인 경우에만 진행
                if ((bool)changedProps["isKicked"])
                {
                    string[] _removeProperties = new string[1];
                    _removeProperties[0] = "isKicked";
                    PhotonNetwork.RemovePlayerCustomProperties(_removeProperties);
                    PhotonNetwork.LeaveRoom();
                }
            }
        }
    }
#endif
}
