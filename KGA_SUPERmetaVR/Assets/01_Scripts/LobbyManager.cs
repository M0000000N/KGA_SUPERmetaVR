using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : SingletonBehaviour<LobbyManager>
{
    public List<RoomInfo> NowRooms = new List<RoomInfo>(); // ������ ��
    RoomOptions roomOptions;
    bool[] isRoom = new bool[10000]; // �� �̸� ����

    private void Awake()
    {
        // ������ ���� ����õ�
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            if (room.RemovedFromList) // �� ������ ��
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

    public override void OnDisconnected(DisconnectCause cause) // ConnectUsingSettings()�� ������ ������ �� ȣ��Ǵ� �ݹ��Լ���.
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedRoom()
    {
        PKB_MainUIManager.Instance.PlayRoomUI.gameObject.SetActive(true);
        PKB_MainUIManager.Instance.PlayRoomUI.SetRoomInfo(roomOptions);
        Debug.Log($"�����ο� / �ִ��ο� : {PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}");
        // Debug.Log($"���� �� �ε��� 1 : {NowRooms[1]}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        switch (returnCode) // TODO : ���߿� �����ͷ� ������
        {
            case -1:
                // ������ ������ �߻��߽��ϴ�. ������� �õ��ϰ� Exit Game�� �����Ͻʽÿ�.
                PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "������ ������ �߻��߽��ϴ�. �ٽ� �õ��� �ּ���", "Ȯ��");
                break;
            case 32765:
                // ������ �� á���ϴ�. ������ �Ϸ�Ǳ� ���� �Ϻ� �÷��̾ �濡 ������ ��쿡�� ���� �߻����� �ʽ��ϴ�.
                PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "������ �� á���ϴ�.", "Ȯ��");
                break;
            case 32764:
                // ������ ����Ǿ� ������ �� �����ϴ�. �ٸ� ���ӿ� �����ϼ���.
                PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "������ ����Ǿ� ������ �� �����ϴ�. �ٸ� ���ӿ� �����ϼ���.", "Ȯ��");
                break;
            case 32760:
                // ������ ��ġ����ŷ�� �����ų� �� ���� ���� ���� �ִ� ��쿡�� �����մϴ�. �� �� �Ŀ� �ݺ��ϰų� �� ���� ����ʽÿ�.
                PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "���� ���������� ���� �����ϴ�.", "Ȯ��");
                break;
            default:
                // �ƹ�ư �濡 �������� �� �ߴ�.
                PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "������ �߻��߽��ϴ�.", "Ȯ��");
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
                // { "IsInRoom",  isInRoom } ���� ƨ�� �� ���
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
                // TODO : �������۾�
                PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "���� ���� ������ �� �����ϴ�. ��� �� �ٽ� �õ������ּ���", "Ȯ��");
            }
            if (isRoom[i]) // �ִ� ��
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