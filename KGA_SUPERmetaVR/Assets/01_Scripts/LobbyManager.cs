using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

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
            else
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

    //private void OnClickRandomJoinButton()
    //{
    //    if (nickname.text.Length == 0)
    //    {
    //        logText.text = "�г����� �Է��ϼ���";
    //        return;
    //    }

    //    // ������ ������ �������̶�� �� ���� ����
    //    if (PhotonNetwork.IsConnected)
    //    {
    //        Data data = FindObjectOfType<Data>();
    //        data.Nickname = nickname.text;


    //        PhotonNetwork.JoinRandomRoom();
    //    }
    //    else
    //    {
    //        deactivateJoinButton("������ ����. ������ �õ� ��..");
    //        PhotonNetwork.ConnectUsingSettings();
    //    }
    //}

    //private void OnClickCreateRoomButton()
    //{
    //    if (nickname.text.Length == 0)
    //    {
    //        logText.text = "�г����� �Է��ϼ���";
    //        return;
    //    }

    //    createRoomPopUp.SetActive(true);
    //}


    public void CreateRoom(RoomOptions _roomOptions)
    {
        if (PhotonNetwork.IsConnected == false) return;
        // [To do]
        // PhotonNetwork.NickName = GameManager.Instance.PlayerData.Nickname;

        PhotonNetwork.JoinOrCreateRoom(SetRoomName(), _roomOptions, TypedLobby.Default);

        Peekaboo_WaitingRoomUIManager.Instance.PlayRoomUI.gameObject.SetActive(true);
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
    }

    public string SetRoomName()
    {
        for (int i = 1; i <= 9999; i++)
        {
            if(isEmptyRoomList[i]) // ���
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
