using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class LobbyManager : SingletonBehaviour<LobbyManager>
{
    private string gameVersion = "1.0.4";

    //[Header("타이틀 화면")]
    //[SerializeField] TMP_InputField nickname;
    //[SerializeField] TextMeshProUGUI logText;

    //[SerializeField] Button randomJoinButton;
    //[SerializeField] Button createRoomButton;

    //[Header("스크롤 뷰")]
    //[SerializeField] Button PlusButton;

    //[SerializeField] ScrollRect scrollRect;
    //[SerializeField] Transform roomBtnParent;
    //[SerializeField] RoomButton roomBtnPref;

    //private List<RoomButton> roomButtons = new List<RoomButton>();

    int index = 0; // 들어간 사람 숫자가 들어올 것

    //private Dictionary<string>x
    public List<PlayRoomUI> roomNameList = new List<PlayRoomUI>();
    bool[] isEmptyRoomList = new bool[10000];

    public readonly RoomOptions RoomOptions = new RoomOptions()
    {
        IsOpen = true,
        IsVisible = true,
        MaxPlayers = 14
    };

    private void Awake()
    {
        // 버튼 이벤트 메소드 연결
        //randomJoinButton.onClick.AddListener(OnClickRandomJoinButton);
        //createRoomButton.onClick.AddListener(OnClickCreateRoomButton);
        //createRoomButtonInPannel.onClick.AddListener(OnClickcreateRoomButtonInPannel);

        //PhotonNetwork.GameVersion = gameVersion;

        // 마스터 서버 연결시도
        PhotonNetwork.ConnectUsingSettings();
        isEmptyRoomList[1] = true;

        //deactivateJoinButton("접속중");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            if (room.RemovedFromList) // 룸 지웠을 때
            {
                int index = roomNameList.FindIndex(x => x.RoomInfo.Name == room.Name);
                if (index != -1)
                {
                    isEmptyRoomList[index] = true;
                }
            }
            else
            {
                PlayRoomUI playRoom = new PlayRoomUI();
                playRoom.SetRoomInfo(room);
            }
        }
    }

    //private void deactivateJoinButton(string message)
    //{
    //    randomJoinButton.interactable = false;
    //    logText.text = message;
    //}

    //private void activeJoinButton()
    //{
    //    randomJoinButton.interactable = true;
    //    // randomJoinButtonText.text = "입장하기";
    //}

    public override void OnConnectedToMaster()
    {
        //activeJoinButton();
        //logText.text = "마스터에 서버 접속됨";

        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause) // ConnectUsingSettings()에 연결이 끊겼을 때 호출되는 콜백함수다.
    {
        //deactivateJoinButton("연결이 끊김. 재접속 시도 중..");

        PhotonNetwork.ConnectUsingSettings();
    }

    //private void OnClickRandomJoinButton()
    //{
    //    if (nickname.text.Length == 0)
    //    {
    //        logText.text = "닉네임을 입력하세요";
    //        return;
    //    }

    //    // 마스터 서버에 접속중이라면 룸 접속 실행
    //    if (PhotonNetwork.IsConnected)
    //    {
    //        Data data = FindObjectOfType<Data>();
    //        data.Nickname = nickname.text;


    //        PhotonNetwork.JoinRandomRoom();
    //    }
    //    else
    //    {
    //        deactivateJoinButton("연결이 끊김. 재접속 시도 중..");
    //        PhotonNetwork.ConnectUsingSettings();
    //    }
    //}

    //private void OnClickCreateRoomButton()
    //{
    //    if (nickname.text.Length == 0)
    //    {
    //        logText.text = "닉네임을 입력하세요";
    //        return;
    //    }

    //    createRoomPopUp.SetActive(true);
    //}

    public void CreateRoom(string _roomName)
    {
        if (PhotonNetwork.IsConnected == false) return;
        // [To do]
        // PhotonNetwork.NickName = GameManager.Instance.PlayerData.Nickname;

        PhotonNetwork.JoinOrCreateRoom(_roomName, RoomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        // logText.text = "방에 입장함";

        // PhotonNetwork.LoadLevel("01_Main");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // logText.text = "빈 방이 없음, 새로운 방 생성..";

        // OnClickCreateRoomButton();
    }

    public string SetRoomName()
    {
        for (int i = 1; i <= 9999; i++)
        {
            if(isEmptyRoomList[i]) // 빈방
            {
                isEmptyRoomList[i] = false;
                roomNameList.Add(new PlayRoomUI());
                
                return System.String.Format("{0:0000}", i);
            }
            else
            {
                return System.String.Format("{0:0000}", i);
            }
        }
        return "0";
    }
}
