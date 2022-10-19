using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class LobbyManager : SingletonBehaviour<LobbyManager>
{
    public List<PKB_PlayRoomUI> roomNameList = new List<PKB_PlayRoomUI>();
    bool[] isEmptyRoomList = new bool[10000];


    private void Awake()
    {
        // 마스터 서버 연결시도
        PhotonNetwork.ConnectUsingSettings();
        isEmptyRoomList[1] = true;
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
            else // TODO : 이게 왜 필요한지 확인
            {
                PKB_PlayRoomUI playRoom = new PKB_PlayRoomUI();
                playRoom.SetRoomInfo(room);
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause) // ConnectUsingSettings()에 연결이 끊겼을 때 호출되는 콜백함수다.
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom(string _password)
    {
        if (PhotonNetwork.IsConnected == false) return;

        RoomOptions roomOptions = new RoomOptions()
        {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = 14
        };

        string roomName = SetRoomName();
        bool isInRoom = false;

        roomOptions.CustomRoomProperties = new Hashtable()
        {
            { "RoomName", roomName },
            { "Password", _password },
            { "IsInRoom",  isInRoom }
        };

        roomOptions.CustomRoomPropertiesForLobby = new string[]
        {
            "RoomName",
            "Password",
            "IsInRoom"
        };

        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Peekaboo_WaitingRoomUIManager.Instance.PlayRoomUI.gameObject.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // 빈 방이 없음, 참여가능 방 없음 팝업
        // [To Do] db에서 텍스트 받아오기
        Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "현재 참여가능한 방이 없습니다.\n잠시 후 다시 시도해주세요.", "확인");

        switch (returnCode) // TODO : 나중에 데이터로 빼야함
        {
            case -1:
                // 서버에 문제가 발생했습니다. 재생산을 시도하고 Exit Game에 문의하십시오.
                Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "서버에 문제가 발생했습니다. 다시 시도해 주세요", "확인");
                break;
            case 32765:
                // 게임이 꽉 찼습니다. 참가가 완료되기 전에 일부 플레이어가 방에 참가한 경우에는 거의 발생하지 않습니다.
                Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "게임이 꽉 찼습니다.", "확인");
                break;
            case 32764:
                // 게임이 종료되어 참가할 수 없습니다. 다른 게임에 참여하세요.
                Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "게임이 종료되어 참가할 수 없습니다. 다른 게임에 참여하세요.", "확인");
                break;
            case 32760:
                // 무작위 매치메이킹은 닫히거나 꽉 차지 않은 방이 있는 경우에만 성공합니다. 몇 초 후에 반복하거나 새 방을 만드십시오.
                Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "방에 들어갈 수 없습니다.", "확인");
                break;
            default:
                // 아무튼 방에 참가하지 못 했다.
                Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "오류가 발생했습니다.", "확인");
                break;
        }

        Debug.Log("조인랜덤룸 실패 리턴 코드 : " +returnCode);
    }

    public string SetRoomName() // TODO : 
    {
        for (int i = 1; i <= 10000; i++)
        {
            if(i==10000)
            {
                Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "현재 방을 생성할 수 없습니다. 잠시 후 다시 시도헤해주세요", "확인");
            }
            if(isEmptyRoomList[i]) // 빈방
            {
                isEmptyRoomList[i] = false;
                roomNameList.Add(new PKB_PlayRoomUI());
                
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
