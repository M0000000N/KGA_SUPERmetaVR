using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1.0.4";

    [Header("타이틀 화면")]
    [SerializeField] TMP_InputField nickname;
    [SerializeField] TextMeshProUGUI logText;

    [SerializeField] Button randomJoinButton;
    [SerializeField] Button createRoomButton;

    [Header("스크롤 뷰")]
    [SerializeField] Button PlusButton;

    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Transform roomBtnParent;
    [SerializeField] RoomButton roomBtnPref;

    private List<RoomButton> roomButtons = new List<RoomButton>();

    [Header("방만들기 팝업")]
    [SerializeField] GameObject createRoomPopUp;
    [SerializeField] TMP_InputField createRoomName;
    [SerializeField] Button createRoomButtonInPannel;

    int index = 0; // 들어간 사람 숫자가 들어올 것

    private static readonly RoomOptions RandomRoomOptions = new RoomOptions()
    {
        MaxPlayers = 3
    };

    private void Awake()
    {
        createRoomName = createRoomPopUp.GetComponentInChildren<TMP_InputField>();
        createRoomPopUp.SetActive(false);

        // 버튼 이벤트 메소드 연결
        randomJoinButton.onClick.AddListener(OnClickRandomJoinButton);
        createRoomButton.onClick.AddListener(OnClickCreateRoomButton);
        createRoomButtonInPannel.onClick.AddListener(OnClickcreateRoomButtonInPannel);

        PhotonNetwork.GameVersion = gameVersion;

        // 마스터 서버 연결시도
        PhotonNetwork.ConnectUsingSettings();

        deactivateJoinButton("접속중");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList) // 룸 지웠을 때
            {
                int index = roomButtons.FindIndex(x => x.RoomInfo.Name == info.Name);
                if(index != -1)
                {
                    Destroy(roomButtons[index].gameObject);
                    roomButtons.RemoveAt(index);
                }
            }
            else // 룸 추가했을 때
            {
                RoomButton listing = (RoomButton)Instantiate(roomBtnPref, roomBtnParent);
                if (listing != null)
                {
                    listing.SetRoomInfo(info);
                }
            }
        }
    }

    private void deactivateJoinButton(string message)
    {
        randomJoinButton.interactable = false;
        logText.text = message;
    }

    private void activeJoinButton()
    {
        randomJoinButton.interactable = true;
        // randomJoinButtonText.text = "입장하기";
    }

    public override void OnConnectedToMaster()
    {
        activeJoinButton();
        logText.text = "마스터에 서버 접속됨";

        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause) // ConnectUsingSettings()에 연결이 끊겼을 때 호출되는 콜백함수다.
    {
        deactivateJoinButton("연결이 끊김. 재접속 시도 중..");

        PhotonNetwork.ConnectUsingSettings();
    }

    private void OnClickRandomJoinButton()
    {
        if (nickname.text.Length == 0)
        {
            logText.text = "닉네임을 입력하세요";
            return;
        }

        // 마스터 서버에 접속중이라면 룸 접속 실행
        if (PhotonNetwork.IsConnected)
        {
            Data data = FindObjectOfType<Data>();
            data.Nickname = nickname.text;


            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            deactivateJoinButton("연결이 끊김. 재접속 시도 중..");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void OnClickCreateRoomButton()
    {
        if (nickname.text.Length == 0)
        {
            logText.text = "닉네임을 입력하세요";
            return;
        }

        createRoomPopUp.SetActive(true);
    }

    private void OnClickcreateRoomButtonInPannel()
    {
        if (PhotonNetwork.IsConnected == false) return;
        if (createRoomName.text.Length == 0)
        {
            logText.text = "방 이름을 입력하세요";
            return;
        }

        PhotonNetwork.JoinOrCreateRoom(createRoomName.text, RandomRoomOptions, TypedLobby.Default);

        createRoomPopUp.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        logText.text = "방에 입장함";

        PhotonNetwork.LoadLevel("01_Main");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        logText.text = "빈 방이 없음, 새로운 방 생성..";

        OnClickCreateRoomButton();
    }
}
