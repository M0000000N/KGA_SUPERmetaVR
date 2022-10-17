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
                PKB_PlayRoomUI playRoom = new PKB_PlayRoomUI();
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

    public string SetRoomName() // TODO : 
    {
        for (int i = 1; i <= 10000; i++)
        {
            if(i==10000)
            {
                Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "���� ���� ������ �� �����ϴ�. ��� �� �ٽ� �õ������ּ���", "Ȯ��");
            }
            if(isEmptyRoomList[i]) // ���
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
