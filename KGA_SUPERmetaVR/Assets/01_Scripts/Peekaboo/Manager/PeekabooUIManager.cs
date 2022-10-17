using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PeekabooUIManager : OnlyOneSceneSingleton<PeekabooUIManager>
{
    [SerializeField]
    private TextMeshProUGUI playerCount;

    private void Update()
    {
        playerCount.text = PeekabooGameManager.Instance.NumberOfPlayers.ToString();
    }

    public void GameOverUI(int _nowPlayerCount)
    {
        // 플레이어가 2명이하 일시 관전하기 버튼 비활성화
        // 게임시간이 0이하일시 모든 관전하기 버튼 비활성화
    }
}
