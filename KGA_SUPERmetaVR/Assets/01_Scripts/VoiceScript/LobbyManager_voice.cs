using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun.Demo.Cockpit;
using System.Linq;

//�����ͼ����� ���� ������ ���� ������ �����ϴ� ����
//������ ������ �� ���� ���
public class LobbyManager_voice : MonoBehaviourPunCallbacks
{
    public Text connectInfoText;
    public Button joinButton;

    public List<Region> RegionsList;


    //���� ���� ���ӹ��� ����
    string gameVersion = "1";

    // Start is called before the first frame update
    void Start()
    {
        SettingInit();

        //������ ���� ����
        string label = "������ ������ ������...";
        MasterServerConnect(label);
    }

    #region CustomFuntion

    //���� �ʱ�ȭ
    void SettingInit()
    {
        //���� ���ӹ��� ����
        PhotonNetwork.GameVersion = gameVersion;

        joinButton.onClick.AddListener(() =>
        {
            Connect();
        });
    }

    //������ ���� ����
    void MasterServerConnect(string _connectInfoText)
    {
        //������ ���� ���ӽõ�
        PhotonNetwork.ConnectUsingSettings();
        //PhotonNetwork.ConnectToBestCloudServer();
        joinButton.interactable = false;
        connectInfoText.text = _connectInfoText;
    }

    //�� ���� �õ�
    public void Connect()
    {
        //�� ���ӹ�ư ��Ȱ��ȭ
        joinButton.interactable = false;

        //������ ������ �������̶��
        if (PhotonNetwork.IsConnected)
        {
            connectInfoText.text = "�뿡 ����...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            //������ ������ �������� �ƴ϶��, ������ ������ ���� �õ�
            string label = "�������� : ������ ������ ������� ����\n���� ��õ� ��...";
            //������ �������� ������ �õ�
            MasterServerConnect(label);
        }
    }

    #endregion
    #region override

    //������ ���� ���� ����
    public override void OnConnectedToMaster()
    {
        //�� ���� ��ư Ȱ��ȭ
        joinButton.interactable = true;
        connectInfoText.text = "�¶��� : ������ ������ �����";

        Debug.Log(PhotonNetwork.PlayerList.Length);
    }

    //������ ������ ������ ������
    public override void OnDisconnected(DisconnectCause cause)
    {
        string label = "�������� : ������ ������ ������� ����\n���� ��õ� ��...";

        //���� ������ �õ�
        MasterServerConnect(label);
    }

    //���� �� ���ӽ���
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectInfoText.text = "���ο���� �������...";
        //maxplayer ������ ���� �� ����
        //UserId ����
        RoomOptions options = new RoomOptions();
        options.PublishUserId = true;
        PhotonNetwork.CreateRoom(null, options);
    }

    //���� �� ���� ����
    public override void OnJoinedRoom()
    {
        connectInfoText.text = "�� ���� ����";

        PhotonNetwork.LoadLevel("Ver.1_Lobby");
    }
    #endregion
}
