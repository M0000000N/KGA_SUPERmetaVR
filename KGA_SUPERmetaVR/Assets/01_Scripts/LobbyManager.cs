using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : SingletonBehaviour<LobbyManager>
{
    public List<RoomInfo> NowRooms = new List<RoomInfo>(); // 생성된 방
    RoomOptions roomOptions;
    bool[] isRoom = new bool[10000]; // 방 이름 관련

    private void Awake()
    {
        // 마스터 서버 연결시도
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            if (room.RemovedFromList) // 룸 지웠을 때
            {
                if (NowRooms.IndexOf(room) < 0)
                {
                    int roomIndex = int.Parse(room.Name);
                    isRoom[roomIndex] = false;
                    return;
                }
                NowRooms.RemoveAt(NowRooms.IndexOf(room));
            }
            else
            {
                if (NowRooms.Contains(room) == false)
                {
                    NowRooms.Add(room);
                    isRoom[NowRooms.IndexOf(room) + 1] = true;
                }
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

    public override void OnJoinedRoom()
    {
        PKB_MainUIManager.Instance.PlayRoomUI.gameObject.SetActive(true);
        PKB_MainUIManager.Instance.PlayRoomUI.SetRoomInfo(roomOptions);
        Debug.Log($"현재인원 / 최대인원 : {PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}");
        // Debug.Log($"들어온 방 인덱스 1 : {NowRooms[1]}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        switch (returnCode) // TODO : 나중에 데이터로 빼야함
        {
            case -1:
                // 서버에 문제가 발생했습니다. 재생산을 시도하고 Exit Game에 문의하십시오.
                PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "서버에 문제가 발생했습니다. 다시 시도해 주세요", "확인");
                break;
            case 32765:
                // 게임이 꽉 찼습니다. 참가가 완료되기 전에 일부 플레이어가 방에 참가한 경우에는 거의 발생하지 않습니다.
                PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "게임이 꽉 찼습니다.", "확인");
                break;
            case 32764:
                // 게임이 종료되어 참가할 수 없습니다. 다른 게임에 참여하세요.
                PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "게임이 종료되어 참가할 수 없습니다. 다른 게임에 참여하세요.", "확인");
                break;
            case 32760:
                // 무작위 매치메이킹은 닫히거나 꽉 차지 않은 방이 있는 경우에만 성공합니다. 몇 초 후에 반복하거나 새 방을 만드십시오.
                PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "현재 참여가능한 방이 없습니다.", "확인");
                break;
            default:
                // 아무튼 방에 참가하지 못 했다.
                PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "오류가 발생했습니다.", "확인");
                break;
        }
    }

    public void CreateRoom(string _password)
    {
        if (PhotonNetwork.IsConnected)
        {
            roomOptions = new RoomOptions()
            {
                IsOpen = true,
                IsVisible = true,
                MaxPlayers = 14,
                BroadcastPropsChangeToAll = true
            };

            string roomName = SetRoomName();
            // bool isInRoom = false;

            roomOptions.CustomRoomProperties = new Hashtable()
            {
                { "RoomName", roomName },
                { "Password", _password },
                // { "IsInRoom",  isInRoom } 추후 튕길 때 사용
            };

            roomOptions.CustomRoomPropertiesForLobby = new string[]
            {
                "RoomName",
                "Password",
                // "IsInRoom"
            };
            PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    [PunRPC]
    public string SetRoomName()
    {
        for (int i = 1; i <= 10000; i++)
        {
            if (i == 10000)
            {
                // TODO : 데이터작업
                PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "현재 방을 생성할 수 없습니다. 잠시 후 다시 시도헤해주세요", "확인");
            }
            if (isRoom[i]) // 있는 방
            {
                continue;
            }
            else
            {
                isRoom[i] = true;
                return i.ToString();
            }
        }
        return "0";
    }
}