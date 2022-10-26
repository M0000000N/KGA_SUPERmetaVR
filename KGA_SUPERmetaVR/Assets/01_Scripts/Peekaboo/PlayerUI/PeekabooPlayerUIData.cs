using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PeekabooPlayerUIData : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject gameResultUI;
    [SerializeField]
    private TextMeshProUGUI playerRankingText;
    [SerializeField]
    private TextMeshProUGUI totalPlayerCount;
    [SerializeField]
    private TextMeshProUGUI surprisedEnemyNumbersText;
    [SerializeField]
    private TextMeshProUGUI survivalTimeText;
    [SerializeField]
    private TextMeshProUGUI NumberOfCoinsAcquiredText;
    [SerializeField]
    private Button watchingButton;
    [SerializeField]
    private Button exitButton;

    private Color winnerColor;

    private void Start()
    {
        winnerColor = new Color(255, 192, 0);
        exitButton.onClick.AddListener(() => { GoRoom(); });
        watchingButton.onClick.AddListener(() => { WatchingStatePlayer(); });
        gameResultUI.SetActive(false);
    }

    private void Update()
    {
        if (PeekabooGameManager.Instance.IsGameOver)
        {
            GameOverUI();
        }
    }

    public void GameOverUI()
    {
        // 게임시간이 0이하일시 모든 관전하기 버튼 비활성화
        if (PeekabooTimeManager.Instance.GameTimer <= 0f)
        {
            watchingButton.interactable = false;
            //if ()// 1등만 플레이어 컬러 변경
            //{
            //}
        }
        // 플레이어가 2명이하 일시 관전하기 버튼 비활성화
        if (PeekabooGameManager.Instance.NumberOfPlayers == 1 && PeekabooGameManager.Instance.NumberOfPlayers ==2)
        {
            watchingButton.interactable = false;
            if (PeekabooGameManager.Instance.NumberOfPlayers == 1)
            {
                playerRankingText.color = winnerColor;
            }
        }
        playerRankingText.text = "# " + PeekabooGameManager.Instance.NumberOfPlayers.ToString();
        totalPlayerCount.text = "/ " + PeekabooGameManager.Instance.TotalNumberOfPeopleFirstEnterdRoom.ToString();
        surprisedEnemyNumbersText.text = "놀래 킨 적 : "; // + 플레이어가 놀래킨 수
        survivalTimeText.text = "생존 시간 : " + ((int)(PeekabooTimeManager.Instance.SurvivalTime / 60)).ToString() + "분" + ((int)(PeekabooTimeManager.Instance.SurvivalTime % 60)).ToString() + "초";
        NumberOfCoinsAcquiredText.text = "획득 코인 :       X " + (PeekabooGameManager.Instance.TotalNumberOfPeopleFirstEnterdRoom - PeekabooGameManager.Instance.NumberOfPlayers + 124).ToString();
        gameResultUI.SetActive(true);
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
        PhotonNetwork.LoadLevel("Login");
    }

    public void WatchingStatePlayer()
    {
        // 관전할 수 있게 플레이어 변경
        Debug.Log("관전 미구현");
    }

}
