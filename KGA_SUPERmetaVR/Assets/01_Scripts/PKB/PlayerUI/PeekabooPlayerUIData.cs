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
        //    Debug.Log("���");
        //    GameOverUI();
        //}
    }

    public void GameOverUI()
    {
        // ���ӽð��� 0�����Ͻ� ��� �����ϱ� ��ư ��Ȱ��ȭ
        if (PeekabooTimeManager.Instance.GameTimer <= 0f)
        {
            watchingButton.interactable = false;
            //if ()// 1� �÷��̾� �÷� ����
            //{
            //}
        }
        // �÷��̾ 2������ �Ͻ� �����ϱ� ��ư ��Ȱ��ȭ
       
        if (PeekabooGameManager.Instance.NumberOfPlayers == 1)
        {
            GameManager.Instance.PlayerData.IsWin = true;
            playerRankingText.color = winnerColor;
        }
        playerRankingText.text = "# " + PeekabooGameManager.Instance.PlayerRanking.ToString();
        totalPlayerCount.text = "/ " + PeekabooGameManager.Instance.TotalNumberOfPeopleFirstEnterdRoom.ToString();
        playerScoreText.text = "� Ų �� : " + PeekabooGameManager.Instance.PlayerScore.ToString();
        survivalTimeText.text = "���� �ð� : " + ((int)(PeekabooTimeManager.Instance.SurvivalTime / 60)).ToString() + "��" + ((int)(PeekabooTimeManager.Instance.SurvivalTime % 60)).ToString() + "��";
        NumberOfCoinsAcquiredText.text = "ȹ�� ���� :       X " + (PeekabooGameManager.Instance.TotalNumberOfPeopleFirstEnterdRoom - PeekabooGameManager.Instance.NumberOfPlayers + 10).ToString();
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
        // ������ �� �ְ� �÷��̾� ����
        Debug.Log("���� �̱���");
    }

}
