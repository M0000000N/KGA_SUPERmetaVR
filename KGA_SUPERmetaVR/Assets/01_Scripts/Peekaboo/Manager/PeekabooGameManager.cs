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

    // ���� �����ִ� �÷��̾� ��
    private int numberOfPlayers;
    public int NumberOfPlayers { get { return numberOfPlayers; } set { numberOfPlayers = value; } }

    // ��ü �� �ο� ��
    public int TotalNumberOfPeopleFirstEnterdRoom { get; private set; }

    private bool isGameOver;
    public bool IsGameOver { get { return isGameOver; } set { isGameOver = value; } }

    [SerializeField]
    private GameObject ovrCamera;
    public GameObject OVRCamera { get { return ovrCamera; } set { ovrCamera = value; } }

    private Dictionary<int, int> playerScoreList;

    public Dictionary<int, int> PlayerScoreList { get { return playerScoreList; } set { playerScoreList = value; } }

    private int playerScore;

    public int PlayerScore { get { return playerScore; } }

    private int playerRanking;

    public int PlayerRanking { get { return playerRanking; } }

    private void Start()
    {
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
        if (numberOfPlayers == 1 && IsGameOver == false)
        {
            PlayerGameOver();
        }
        Debug.Log($"���Ӱ��{IsGameOver}");
        Debug.Log($"���� �÷��̾� ��{numberOfPlayers}");
    }

    public void PlayerGameOver()
    {
        isGameOver = true;
        if (PeekabooTimeManager.Instance.GameTimer <= 0f)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                playerRanking = 0;
                int equalRanking = 1;
                int minScore = TotalNumberOfPeopleFirstEnterdRoom;
                var sortVar = from item in playerScoreList orderby item.Value descending select item;
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
            photonView.RPC("RPCRequestPlayerScore", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber - 1);
        }
    }

    // ������ Ŭ���̾�Ʈ���� ������û
    [PunRPC]
    private void RPCRequestPlayerScore(int _playerActorNumber)
    {
        Debug.Log($"RPCRequestPlayerScore ���� �ѹ� {_playerActorNumber}");
        Debug.Log($"RPCRequestPlayerScore ����Ʈ {playerScoreList[0]}");
        int requestPlayerScore = playerScoreList[_playerActorNumber];
        playerScoreList.Remove(_playerActorNumber);
        photonView.RPC("RPCGivePlayerScore", RpcTarget.All,_playerActorNumber,requestPlayerScore);
    }

    // ��û�� �÷��̾�� ��������
    [PunRPC]
    private void RPCGivePlayerScore(int _playerActorNumber, int _requestPlayerScore)
    {
        if (_playerActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            playerScore = _requestPlayerScore;
        }
    }

    // �÷��̽ð��� ������ �� ������ ����� ������ ����� ����
    [PunRPC]
    private void RPCTimeOverScore(int _playerActorNumber, int _requestPlayerScore, int _playerRanking)
    {
        if (_playerActorNumber + 1 == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            playerScore = _requestPlayerScore;
            playerRanking = _playerRanking;
        }
    }

    private void PeekabooEnforceGameShutdown()
    {
        // ������ ������ �� ī��Ʈ �ϳ� ����
        PlayerGameOver();
    }
}
