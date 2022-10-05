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

    [Header("Ÿ��Ʋ ȭ��")]
    [SerializeField] TMP_InputField nickname;
    [SerializeField] TextMeshProUGUI logText;

    [SerializeField] Button randomJoinButton;
    [SerializeField] Button createRoomButton;

    [Header("��ũ�� ��")]
    [SerializeField] Button PlusButton;

    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Transform roomBtnParent;
    [SerializeField] RoomButton roomBtnPref;

    private List<RoomButton> roomButtons = new List<RoomButton>();

    [Header("�游��� �˾�")]
    [SerializeField] GameObject createRoomPopUp;
    [SerializeField] TMP_InputField createRoomName;
    [SerializeField] Button createRoomButtonInPannel;

    int index = 0; // �� ��� ���ڰ� ���� ��

    private static readonly RoomOptions RandomRoomOptions = new RoomOptions()
    {
        MaxPlayers = 3
    };

    private void Awake()
    {
        createRoomName = createRoomPopUp.GetComponentInChildren<TMP_InputField>();
        createRoomPopUp.SetActive(false);

        // ��ư �̺�Ʈ �޼ҵ� ����
        randomJoinButton.onClick.AddListener(OnClickRandomJoinButton);
        createRoomButton.onClick.AddListener(OnClickCreateRoomButton);
        createRoomButtonInPannel.onClick.AddListener(OnClickcreateRoomButtonInPannel);

        PhotonNetwork.GameVersion = gameVersion;

        // ������ ���� ����õ�
        PhotonNetwork.ConnectUsingSettings();

        deactivateJoinButton("������");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList) // �� ������ ��
            {
                int index = roomButtons.FindIndex(x => x.RoomInfo.Name == info.Name);
                if(index != -1)
                {
                    Destroy(roomButtons[index].gameObject);
                    roomButtons.RemoveAt(index);
                }
            }
            else // �� �߰����� ��
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
        // randomJoinButtonText.text = "�����ϱ�";
    }

    public override void OnConnectedToMaster()
    {
        activeJoinButton();
        logText.text = "�����Ϳ� ���� ���ӵ�";

        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause) // ConnectUsingSettings()�� ������ ������ �� ȣ��Ǵ� �ݹ��Լ���.
    {
        deactivateJoinButton("������ ����. ������ �õ� ��..");

        PhotonNetwork.ConnectUsingSettings();
    }

    private void OnClickRandomJoinButton()
    {
        if (nickname.text.Length == 0)
        {
            logText.text = "�г����� �Է��ϼ���";
            return;
        }

        // ������ ������ �������̶�� �� ���� ����
        if (PhotonNetwork.IsConnected)
        {
            Data data = FindObjectOfType<Data>();
            data.Nickname = nickname.text;


            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            deactivateJoinButton("������ ����. ������ �õ� ��..");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void OnClickCreateRoomButton()
    {
        if (nickname.text.Length == 0)
        {
            logText.text = "�г����� �Է��ϼ���";
            return;
        }

        createRoomPopUp.SetActive(true);
    }

    private void OnClickcreateRoomButtonInPannel()
    {
        if (PhotonNetwork.IsConnected == false) return;
        if (createRoomName.text.Length == 0)
        {
            logText.text = "�� �̸��� �Է��ϼ���";
            return;
        }

        PhotonNetwork.JoinOrCreateRoom(createRoomName.text, RandomRoomOptions, TypedLobby.Default);

        createRoomPopUp.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        logText.text = "�濡 ������";

        PhotonNetwork.LoadLevel("01_Main");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        logText.text = "�� ���� ����, ���ο� �� ����..";

        OnClickCreateRoomButton();
    }
}
