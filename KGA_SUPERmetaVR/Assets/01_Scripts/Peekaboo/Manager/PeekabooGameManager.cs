//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using System;

public class PeekabooGameManager : OnlyOneSceneSingleton<PeekabooGameManager>
{
    public GameObject PlayerPrefeb;
    //public Button exitButton;
    public GameObject TestNPC;

    [SerializeField]
    private PeekabooCreateMap createMap;
    public PeekabooCreateMap CreateMap { get { return createMap; } }

    private int numberOfPlayers;
    public int NumberOfPlayers { get { return numberOfPlayers; } set { numberOfPlayers = value; } }

    public int TotalNumberOfPeopleFirstEnterdRoom { get; private set; }

    private bool isGameOver;
    public bool IsGameOver { get { return isGameOver; } set { isGameOver = value; } }

    private int surprisedEnemyNumbers;

    public int SurprisedEnemyNumbers { get { return surprisedEnemyNumbers; } set { surprisedEnemyNumbers = value; } }

    [SerializeField]
    private Button exitButton;
    public void Start()
    {
        exitButton.onClick.AddListener(() => { GoRoom(); });
        surprisedEnemyNumbers = 0;
        IsGameOver = false;
        TotalNumberOfPeopleFirstEnterdRoom = PhotonNetwork.CountOfPlayers;
        numberOfPlayers = PhotonNetwork.CountOfPlayers;
        //exitButton.onClick.AddListener(OnClickExitButton);
    }

    private void GoRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("00_Title");
    }


    public void PlayerGameOver()
    {
       // playerScore["score"] = (int)photonView.Owner.CustomProperties[]
         
        // �÷��̾� �̵� �� ������ ��� ��ȣ�ۿ� ����
        PeekabooUIManager.Instance.GameOverUI(numberOfPlayers);
    }


    private void PeekabooGameOver()
    {
        if (true) // �÷��̾ �׾��� ��
        {
            PlayerGameOver();
        }
        else if (numberOfPlayers == 1)
        {
            PlayerGameOver();
            
        }
        else if (PeekabooTimeManager.Instance.GameTimer <= 0f)
        {
            PlayerGameOver();
        }
        else if (true) // ���� ��ư�� ��������
        {
            PlayerGameOver();
        }
    }

    private void PeekabooEnforceGameShutdown()
    {
        // ������ ������ �� ī��Ʈ �ϳ� ����
        // �÷��̾�� ����? ���⼭ ����?
        PlayerGameOver();
    }

    public void WatchingStatePlayer()
    {
        // ������ �� �ְ� �÷��̾� ����'
        Debug.Log("���� �̱���");
    }

}
