using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;

public enum SCENESTATE
{
    LOGIN,
    PLAYMINIMANIMO,
    PEEKABOOLOBBY,
    PLAYPEEKABOO,
}

public class LobbyManager : SingletonBehaviour<LobbyManager>
{
    public List<RoomInfo> NowRooms = new List<RoomInfo>(); // ������ ��
    RoomOptions roomOptions;
    bool[] isRoom = new bool[10000]; // �� �̸� ����
    public bool[] IsRoom { get { return isRoom; } set { return; } } // �� �̸� ����

    [SerializeField] int MMMMaxPlayer = 20;
    [SerializeField] int PKBMaxPlayer = 14;
    [SerializeField] int PKBMaxRoomCount = 9999;
    public SCENESTATE CurrentSceneIndex { get { return currentSceneIndex; } set { currentSceneIndex = value; } }
    [SerializeField] private SCENESTATE currentSceneIndex = SCENESTATE.LOGIN; //0-login, 1-Ver.1_Lobby, 2-PKB_Main, 3-PKB_InGame, 4-Tutorial

    private void Awake() // �÷��̾ �ڴ�� ���� ������ ���� ������
    {
        PhotonNetwork.SendRate = 10;
        PhotonNetwork.SerializationRate = 30;
    }

    private void Start()
    {
        // ������ ���� ����õ�
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        if (currentSceneIndex == SCENESTATE.LOGIN)
        {
            LoginManager.Instance.JoinCanvas.GetComponent<JoinCanvas>().Login.interactable = true;
            LoginManager.Instance.JoinCanvas.GetComponent<JoinCanvas>().SignUp.interactable = true;
            LoginManager.Instance.JoinCanvas.GetComponent<JoinCanvas>().ForgetPassword.interactable = true;
        }
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

    public void JoinOrCreateRoom(string _password = null, bool _isMinimanimo = false)
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            return;
        }

        // bool isInRoom = false; ���� ƨ�� �� ���
        string roomName;
        int maxPlayer;
        if (_isMinimanimo) // �̴ϸ��ϸ�� ����� 00
        {
            roomName = "00";
            maxPlayer = MMMMaxPlayer;
        }
        else
        {
            if (SetRoomName() == null) return;
            roomName = SetRoomName();
            //roomName = photonView.RPC("SetRoomName", RpcTarget.All);
            maxPlayer = PKBMaxPlayer;
        }

        roomOptions = new RoomOptions()
        {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = Convert.ToByte(maxPlayer),
            BroadcastPropsChangeToAll = true
        };

        roomOptions.CustomRoomProperties = new Hashtable()
            {
                { "RoomName", roomName },
                { "Password", _password },
                // { "IsInRoom",  isInRoom } 
            };

        roomOptions.CustomRoomPropertiesForLobby = new string[]
        {
                "RoomName",
                "Password",
            // "IsInRoom"
        };
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            return;
        }

        if (PhotonNetwork.CurrentRoom.CustomProperties["RoomName"].ToString() == "00")
        {
            if (PhotonNetwork.NickName != string.Empty && PhotonNetwork.LocalPlayer.IsLocal)
            {
                PhotonNetwork.LoadLevel("Ver.1_Lobby");
            }
        }
        else
        {
            PKB_MainUIManager.Instance.PlayRoomUI.gameObject.SetActive(true);
            PKB_MainUIManager.Instance.PlayRoomUI.SetRoomInfo(roomOptions);
            Debug.Log($"�����ο� / �ִ��ο� : {PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}");
        }
        Debug.Log("���� �� �̸� : " + PhotonNetwork.CurrentRoom.CustomProperties["RoomName"].ToString());
    }

    public override void OnLeftRoom()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            return;
        }
        if (currentSceneIndex == SCENESTATE.PLAYMINIMANIMO || currentSceneIndex == SCENESTATE.PLAYPEEKABOO)
        {
            PhotonNetwork.LoadLevel("PKB_Main");
            PhotonNetwork.JoinLobby();
            currentSceneIndex = SCENESTATE.PEEKABOOLOBBY;
        }
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

    [PunRPC]
    public string SetRoomName()
    {
        for (int i = 1; i <= PKBMaxRoomCount; i++)
        {
            if (i == PKBMaxRoomCount)
            {
                // TODO : �������۾�
                PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "���� ���� ������ �� �����ϴ�. ��� �� �ٽ� �õ����ּ���", "Ȯ��");
                return null;
            }
            if (isRoom[i]) // �ִ� ��
            {
                continue;
            }
            else
            {
                return i.ToString(); //("{0:00}", i.ToString()), ("%02d", i)
            }
        }
        return "0";
    }
}