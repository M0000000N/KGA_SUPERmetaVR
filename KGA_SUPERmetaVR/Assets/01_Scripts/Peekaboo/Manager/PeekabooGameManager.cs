//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using System;

public class PeekabooGameManager : OnlyOneSceneSingleton<PeekabooGameManager>
{
    [SerializeField]
    private GameObject playerPrefeb;

    public GameObject PlayerPrefeb { get { return playerPrefeb; } }

    [SerializeField]
    private PeekabooCreateMap createMap;
    public PeekabooCreateMap CreateMap { get { return createMap; } }

    [SerializeField]
    private PeekabooSpawner peekabooSpawner;
    public PeekabooSpawner PeekabooSpawner { get { return peekabooSpawner; } }

    private int numberOfPlayers;
    public int NumberOfPlayers { get { return numberOfPlayers; } set { numberOfPlayers = value; } }

    public int TotalNumberOfPeopleFirstEnterdRoom { get; private set; }

    private bool isGameOver;
    public bool IsGameOver { get { return isGameOver; } set { isGameOver = value; } }

    private int surprisedEnemyNumbers;

    public int SurprisedEnemyNumbers { get { return surprisedEnemyNumbers; } set { surprisedEnemyNumbers = value; } }

    private PeekabooPlayerUIData peekabooPlayerUIData;


    public void Start()
    {
        peekabooPlayerUIData = playerPrefeb.GetComponentInChildren<PeekabooPlayerUIData>();
        surprisedEnemyNumbers = 0;
        IsGameOver = false;
        TotalNumberOfPeopleFirstEnterdRoom = PhotonNetwork.CountOfPlayers;
        numberOfPlayers = PhotonNetwork.CountOfPlayers;
        //exitButton.onClick.AddListener(OnClickExitButton);
    }

    private void Update()
    {
        if (!isGameOver)
        {
            Debug.Log("게임오버");
            PeekabooGameOver();

        }
    }


    public void PlayerGameOver()
    {
        // playerScore["score"] = (int)photonView.Owner.CustomProperties[]
        isGameOver = true;
        // 플레이어 이동 및 시점등 모든 상호작용 멈춤
        //PeekabooUIManager.Instance.GameOverUI(numberOfPlayers);
    }


    private void PeekabooGameOver()
    {
        if (false) // 플레이어가 죽었을 때
        {
            PlayerGameOver();
        }
        else if (false)//numberOfPlayers == 1)
        {
            Debug.Log("혼자남음");
            PlayerGameOver();
            
        }
        else if (PeekabooTimeManager.Instance.GameTimer <= 0f)
        {
            Debug.Log("시간 다댐");
            PlayerGameOver();
        }
        else if (false) // 종료 버튼을 눌럿을시
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
