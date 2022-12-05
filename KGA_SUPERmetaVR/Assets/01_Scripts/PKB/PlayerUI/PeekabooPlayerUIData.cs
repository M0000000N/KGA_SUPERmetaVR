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
    private TextMeshProUGUI playerScoreText;
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
        
        watchingButton.onClick.AddListener(() => { WatchingStatePlayer(); });
        exitButton.onClick.AddListener(() => { ExitGame(); });
        gameResultUI.SetActive(false);
    }

    private void Update()
    {
        //if (PeekabooGameManager.Instance.IsGameOver)
        //{
        //    Debug.Log("헬로");
        //    GameOverUI();
        //}
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
       
        if (PeekabooGameManager.Instance.NumberOfPlayers == 1)
        {
            GameManager.Instance.PlayerData.IsWin = true;
            playerRankingText.color = winnerColor;
        }
        playerRankingText.text = "# " + PeekabooGameManager.Instance.PlayerRanking.ToString();
        totalPlayerCount.text = "/ " + PeekabooGameManager.Instance.TotalNumberOfPeopleFirstEnterdRoom.ToString();
        playerScoreText.text = "놀래 킨 적 : " + PeekabooGameManager.Instance.PlayerScore.ToString();
        survivalTimeText.text = "생존 시간 : " + ((int)(PeekabooTimeManager.Instance.SurvivalTime / 60)).ToString() + "분" + ((int)(PeekabooTimeManager.Instance.SurvivalTime % 60)).ToString() + "초";
        NumberOfCoinsAcquiredText.text = "획득 코인 :       X " + (PeekabooGameManager.Instance.TotalNumberOfPeopleFirstEnterdRoom - PeekabooGameManager.Instance.NumberOfPlayers + 10).ToString();
        gameResultUI.SetActive(true);
    }

    public void ExitGame()
    {
        if (PhotonNetwork.InRoom)
        {
            SoundManager.Instance.PlayBGM("PKBOO_Main_bgm.wav");
            PhotonNetwork.LeaveRoom();
        }
    }

    public void WatchingStatePlayer()
    {
        // 관전할 수 있게 플레이어 변경
        Debug.Log("관전 미구현");
    }

}
