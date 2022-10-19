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
    public List<PlayRoomUI> roomNameList = new List<PlayRoomUI>();
    bool[] isEmptyRoomList = new bool[10000];


    private void Awake()
    {
        // ������ ���� ����õ�
        PhotonNetwork.ConnectUsingSettings();
        isEmptyRoomList[1] = true;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            if (room.RemovedFromList) // �� ������ ��
            {
                int index = roomNameList.FindIndex(x => x.RoomInfo.Name == room.Name);
                if (index != -1)
                {
                    isEmptyRoomList[index] = true;
                }
            }
            else // TODO : �̰� �� �ʿ����� Ȯ��
            {
                PlayRoomUI playRoom = new PlayRoomUI();
                playRoom.SetRoomInfo(room);
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

    public void CreateRoom(string _password)
    {
        if (PhotonNetwork.IsConnected == false) return;

        RoomOptions roomOptions = new RoomOptions()
        {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = 14
        };

        roomOptions.CustomRoomProperties = new Hashtable()
        {
            { "password", _password }
        };

        roomOptions.CustomRoomPropertiesForLobby = new string[]
        {
            "isPrivate", "password"
        };

        PhotonNetwork.CreateRoom(SetRoomName(), roomOptions, TypedLobby.Default);
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
        // �� ���� ����, �������� �� ���� �˾�
        // [To Do] db���� �ؽ�Ʈ �޾ƿ���
        Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "���� ���������� ���� �����ϴ�.\n��� �� �ٽ� �õ����ּ���.", "Ȯ��");

        switch (returnCode) // TODO : ���߿� �����ͷ� ������
        {
            case -1:
                // ������ ������ �߻��߽��ϴ�. ������� �õ��ϰ� Exit Game�� �����Ͻʽÿ�.
                Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "������ ������ �߻��߽��ϴ�. �ٽ� �õ��� �ּ���", "Ȯ��");
                break;
            case 32765:
                // ������ �� á���ϴ�. ������ �Ϸ�Ǳ� ���� �Ϻ� �÷��̾ �濡 ������ ��쿡�� ���� �߻����� �ʽ��ϴ�.
                Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "������ �� á���ϴ�.", "Ȯ��");
                break;
            case 32764:
                // ������ ����Ǿ� ������ �� �����ϴ�. �ٸ� ���ӿ� �����ϼ���.
                Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "������ ����Ǿ� ������ �� �����ϴ�. �ٸ� ���ӿ� �����ϼ���.", "Ȯ��");
                break;
            case 32760:
                // ������ ��ġ����ŷ�� �����ų� �� ���� ���� ���� �ִ� ��쿡�� �����մϴ�. �� �� �Ŀ� �ݺ��ϰų� �� ���� ����ʽÿ�.
                Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "�濡 �� �� �����ϴ�.", "Ȯ��");
                break;
            default:
                // �ƹ�ư �濡 �������� �� �ߴ�.
                Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "������ �߻��߽��ϴ�.", "Ȯ��");
                break;
        }

        Debug.Log("���η����� ���� ���� �ڵ� : " +returnCode);
    }

    public void CreateRoom(string _password)
    {
        if (PhotonNetwork.IsConnected)
        {
            roomOptions = new RoomOptions()
            {
                IsOpen = true,
                IsVisible = true,
                MaxPlayers = 14
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
    public string SetRoomName() // TODO : 
    {
        for (int i = 1; i <= 9999; i++)
        {
            if(isEmptyRoomList[i]) // ���
            {
                isEmptyRoomList[i] = false;
                roomNameList.Add(new PlayRoomUI());
                
                return i.ToString();
            }
            else
            {
                return i.ToString();
            }
        }
        return "0";
    }
}
