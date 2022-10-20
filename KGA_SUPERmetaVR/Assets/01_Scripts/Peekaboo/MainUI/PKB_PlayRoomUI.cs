#define 호스트판단
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PKB_PlayRoomUI : MonoBehaviourPunCallbacks, IPunObservable
{
    public RoomInfo RoomInfo { get; private set; }
    public int MinPlayercount;

    [Header("PlayRoom")]
    [SerializeField] TextMeshProUGUI RoomNameText;
    [SerializeField] Button gameStartButton;
    private TextMeshProUGUI gameStartButtonText;
    [SerializeField] Button exitRoomButton;
    [SerializeField] TextMeshProUGUI RoomTypeText;

    [Header("ExitRoom")]
    [SerializeField] GameObject exitRoomUI;
    [SerializeField] Button YesExitRoomButton;
    [SerializeField] Button NoExitRoomButton;

    [Header("PlayerList")]
    [SerializeField] PKB_PlayerPanel playerPanel;

    bool playerIsReady;
    bool hostIsReady;
    bool isGameStart;

    private void Awake()
    {
        gameStartButton.onClick.AddListener(OnClickStartButton);
        exitRoomButton.onClick.AddListener(OnClickExitButton);
        exitRoomUI.gameObject.SetActive(false);
        YesExitRoomButton.onClick.AddListener(OnClickYesExitRoomButton);
        NoExitRoomButton.onClick.AddListener(OnClickNoExitRoomButton);

        gameStartButtonText = gameStartButton.GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        playerIsReady = false;
        hostIsReady = false;
        isGameStart = false;

        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                gameStartButtonText.text = "게임시작";
            }
            else
            {
                gameStartButtonText.text = "준비";
            }
        }
        else
        {
            gameStartButtonText.text = "대기중";

        }
    }
    private void Update()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                gameStartButtonText.text = "게임시작";

                if (PhotonNetwork.PlayerList.Length > MinPlayercount) // TODO : 전부 준비완료가 됐는지?
                {
                    hostIsReady = true;
                }
                else
                {
                    hostIsReady = false;
                }
                // playerPanel.kickButton.gameObject.SetActive(true);
            }
            else
            {
                gameStartButton.interactable = true;
                // playerPanel.kickButton.gameObject.SetActive(false);
            }
        }
        else
        {
            gameStartButton.interactable = false;
        }

        if (isGameStart)
        {
        }
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 직렬화 -> 서버에게 데이터를 보내는 것
        if (stream.IsWriting)
        {
            stream.SendNext(isGameStart);
        }
        else // 역직렬화 -> 서버로부터 데이터를 받은 것
        {
            isGameStart = (bool)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void OnClickStartButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (hostIsReady)
            {
                isGameStart = true;
            PhotonNetwork.LoadLevel("Peekaboo_InGame");
            }
            else
            {
                PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "모든 플레이어가 준비완료 상태가 아닙니다.", "확인");
            }
        }
        else
        {
            if (playerIsReady)
            {
                gameStartButtonText.text = "준비";
                playerIsReady = false;
            }
            else
            {
                gameStartButtonText.text = "준비완료";
                playerIsReady = true;
            }
        }
    }

    public void SetRoomInfo(RoomOptions _roomOptions)
    {
        // 방 이름
        RoomNameText.text = "# " + System.String.Format("{0:0000}", int.Parse(PhotonNetwork.CurrentRoom.CustomProperties["RoomName"].ToString()));

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
