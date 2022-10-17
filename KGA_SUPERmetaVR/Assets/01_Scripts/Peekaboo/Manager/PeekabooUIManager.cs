using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PeekabooUIManager : OnlyOneSceneSingleton<PeekabooUIManager>
{
    [SerializeField]
    private TextMeshProUGUI playerCountText;
    [SerializeField]
    private GameObject gameResultUI;
    [SerializeField]
    private TextMeshProUGUI playerRankingText;
    [SerializeField]
    private TextMeshProUGUI surprisedEnemyNumbersText;
    [SerializeField]
    private TextMeshProUGUI survivalTimeText;
    [SerializeField]
    private TextMeshProUGUI NumberOfCoinsAcquiredText;
    [SerializeField]
    private Button watchingButton;


    private void Update()
    {
        playerCountText.text = PeekabooGameManager.Instance.NumberOfPlayers.ToString();
        
    }

    public void GameOverUI(int _nowPlayerCount)
    {
        gameResultUI.SetActive(true);
        PeekabooGameManager.Instance.IsGameOver = true;
        if (PeekabooGameManager.Instance.NumberOfPlayers == 1)
        {
            watchingButton.interactable = false;
        }
        // 플레이어가 2명이하 일시 관전하기 버튼 비활성화
        // 게임시간이 0이하일시 모든 관전하기 버튼 비활성화
        playerRankingText.text = "# " + PeekabooGameManager.Instance.NumberOfPlayers.ToString() + " / " + PeekabooGameManager.Instance.TotalNumberOfPeopleFirstEnterdRoom.ToString();
        // surprisedEnemyNumbers = 플레이어가 놀래킨 수
        survivalTimeText.text = "생존 시간 : " + ((int)(PeekabooTimeManager.Instance.SurvivalTime / 60)).ToString() + "분" + ((int)(PeekabooTimeManager.Instance.SurvivalTime % 60)).ToString() + "초";
        NumberOfCoinsAcquiredText.text = "획득 코인 :       X " + (PeekabooGameManager.Instance.TotalNumberOfPeopleFirstEnterdRoom - PeekabooGameManager.Instance.NumberOfPlayers).ToString();
    }
}
