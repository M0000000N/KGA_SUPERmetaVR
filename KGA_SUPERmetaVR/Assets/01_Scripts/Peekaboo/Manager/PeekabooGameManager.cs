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
         
        // 플레이어 이동 및 시점등 모든 상호작용 멈춤
        PeekabooUIManager.Instance.GameOverUI(numberOfPlayers);
    }


    private void PeekabooGameOver()
    {
        if (true) // 플레이어가 죽었을 때
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
        else if (true) // 종료 버튼을 눌럿을시
        {
            PlayerGameOver();
        }
    }

    private void PeekabooEnforceGameShutdown()
    {
        // 강제로 종료할 시 카운트 하나 줄임
        // 플레이어에서 관리? 여기서 관리?
        PlayerGameOver();
    }

    public void WatchingStatePlayer()
    {
        // 관전할 수 있게 플레이어 변경'
        Debug.Log("관전 미구현");
    }

}
