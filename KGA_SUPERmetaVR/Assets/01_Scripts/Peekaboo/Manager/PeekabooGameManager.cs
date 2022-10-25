//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using System;
using System.Linq;

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

    // 현재 남아있는 플레이어 수
    private int numberOfPlayers;
    public int NumberOfPlayers { get { return numberOfPlayers; } set { numberOfPlayers = value; } }

    // 전체 방 인원 수
    public int TotalNumberOfPeopleFirstEnterdRoom { get; private set; }

    private bool isGameOver;
    public bool IsGameOver { get { return isGameOver; } set { isGameOver = value; } }

    [SerializeField]
    private GameObject ovrCamera;
    public GameObject OVRCamera { get { return ovrCamera; } set { ovrCamera = value; } }

    private int surprisedEnemyNumbers;
    public int SurprisedEnemyNumbers { get { return surprisedEnemyNumbers; } set { surprisedEnemyNumbers = value; } }

    private Dictionary<int, int> playerScoreList;

    public Dictionary<int, int> PlayerScoreList { get { return playerScoreList; } set { playerScoreList = value; } }

    private int playerScore;

    public int PlayerScore { get { return playerScore; } }

    private int playerRanking;

    public int PlayerRanking { get { return playerRanking; } }

    // ?? 이거 추가하신분? 어디다 쓰이는건지
    private PeekabooPlayerUIData peekabooPlayerUIData;


    private void Start()
    {
        peekabooPlayerUIData = playerPrefeb.GetComponentInChildren<PeekabooPlayerUIData>();
        surprisedEnemyNumbers = 0;
        IsGameOver = false;
        TotalNumberOfPeopleFirstEnterdRoom = PhotonNetwork.CurrentRoom.PlayerCount;
        numberOfPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
        if (PhotonNetwork.IsMasterClient)
        {
            playerScoreList = new Dictionary<int, int>();
            for (int i = 0; i < numberOfPlayers; ++i)
            {
                playerScoreList.Add(i, 0);
            }
        }
    }

    private void Update()
    {
        if (numberOfPlayers == 1)
        {
            PlayerGameOver();
        }
        Debug.Log($"게임결과{IsGameOver}");
        Debug.Log($"현재 플레이어 수{numberOfPlayers}");
    }

    public void PlayerGameOver()
    {
        isGameOver = true;
        if (PeekabooTimeManager.Instance.GameTimer <= 0f)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                playerRanking = 1;
                int equalRanking = 1;
                int minScore = TotalNumberOfPeopleFirstEnterdRoom;
                var sortVar = from item in PeekabooGameManager.Instance.PlayerScoreList orderby item.Value descending select item;
                foreach (var item in sortVar)
                {
                    if (minScore >= item.Value)
                    {
                        if (minScore == item.Value)
                        {
                            equalRanking++;
                        }
                        else
                        {
                            minScore = item.Value;
                            playerRanking = playerRanking + equalRanking;
                            equalRanking = 1;
                        }
                        
                    }
                    photonView.RPC("RPCTimeOverScore", RpcTarget.All, item.Key, item.Value, playerRanking);
                }
            }
        }
        else
        {
            playerRanking = numberOfPlayers;
            photonView.RPC("RPCRequestPlayerScore", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }

    [PunRPC]
    private void RPCRequestPlayerScore(int _playerActorNumber)
    {
        int requestPlayerScore = playerScoreList[_playerActorNumber];
        playerScoreList.Remove(_playerActorNumber);
        photonView.RPC("RPCGivePlayerScore", RpcTarget.All,_playerActorNumber,requestPlayerScore);
    }

    [PunRPC]
    private void RPCGivePlayerScore(int _playerActorNumber, int _requestPlayerScore)
    {
        if (_playerActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            playerScore = _requestPlayerScore;
        }
    }

    [PunRPC]
    private void RPCTimeOverScore(int _playerActorNumber, int _requestPlayerScore, int _playerRanking)
    {
        if (_playerActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            playerScore = _requestPlayerScore;
            playerRanking = _playerRanking;
        }
    }


    private void PeekabooGameOver()
    {
        if (false) // 플레이어가 죽었을 때
        {
            PlayerGameOver();
        }
        else if (numberOfPlayers == 1)
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
}
