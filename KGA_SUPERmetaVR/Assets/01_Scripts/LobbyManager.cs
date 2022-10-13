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

    //[Header("Ÿ��Ʋ ȭ��")]
    //[SerializeField] TMP_InputField nickname;
    //[SerializeField] TextMeshProUGUI logText;

    //[SerializeField] Button randomJoinButton;
    //[SerializeField] Button createRoomButton;

    //[Header("��ũ�� ��")]
    //[SerializeField] Button PlusButton;

    //[SerializeField] ScrollRect scrollRect;
    //[SerializeField] Transform roomBtnParent;
    //[SerializeField] RoomButton roomBtnPref;

    //private List<RoomButton> roomButtons = new List<RoomButton>();

    int index = 0; // �� ��� ���ڰ� ���� ��

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
        // ��ư �̺�Ʈ �޼ҵ� ����
        //randomJoinButton.onClick.AddListener(OnClickRandomJoinButton);
        //createRoomButton.onClick.AddListener(OnClickCreateRoomButton);
        //createRoomButtonInPannel.onClick.AddListener(OnClickcreateRoomButtonInPannel);

        //PhotonNetwork.GameVersion = gameVersion;

        // ������ ���� ����õ�
        PhotonNetwork.ConnectUsingSettings();
        isEmptyRoomList[1] = true;

        //deactivateJoinButton("������");
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

    //private void deactivateJoinButton(string message)
    //{
    //    randomJoinButton.interactable = false;
    //    logText.text = message;
    //}

    //private void activeJoinButton()
    //{
    //    randomJoinButton.interactable = true;
    //    // randomJoinButtonText.text = "�����ϱ�";
    //}

    public override void OnConnectedToMaster()
    {
        //activeJoinButton();
        //logText.text = "�����Ϳ� ���� ���ӵ�";

        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause) // ConnectUsingSettings()�� ������ ������ �� ȣ��Ǵ� �ݹ��Լ���.
    {
        //deactivateJoinButton("������ ����. ������ �õ� ��..");

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

    public void CreateRoom(string _roomName)
    {
        if (PhotonNetwork.IsConnected == false) return;
        // [To do]
        // PhotonNetwork.NickName = GameManager.Instance.PlayerData.Nickname;

        PhotonNetwork.JoinOrCreateRoom(_roomName, RoomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        // logText.text = "�濡 ������";

        // PhotonNetwork.LoadLevel("01_Main");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // logText.text = "�� ���� ����, ���ο� �� ����..";

        // OnClickCreateRoomButton();
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
