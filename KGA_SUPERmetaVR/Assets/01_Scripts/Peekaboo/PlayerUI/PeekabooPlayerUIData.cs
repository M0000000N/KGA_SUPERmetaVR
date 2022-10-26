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
        // ���ӽð��� 0�����Ͻ� ��� �����ϱ� ��ư ��Ȱ��ȭ
        if (PeekabooTimeManager.Instance.GameTimer <= 0f)
        {
            watchingButton.interactable = false;
            //if ()// 1� �÷��̾� �÷� ����
            //{
            //}
        }
        // �÷��̾ 2������ �Ͻ� �����ϱ� ��ư ��Ȱ��ȭ
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
        surprisedEnemyNumbersText.text = "� Ų �� : "; // + �÷��̾ �Ų ��
        survivalTimeText.text = "���� �ð� : " + ((int)(PeekabooTimeManager.Instance.SurvivalTime / 60)).ToString() + "��" + ((int)(PeekabooTimeManager.Instance.SurvivalTime % 60)).ToString() + "��";
        NumberOfCoinsAcquiredText.text = "ȹ�� ���� :       X " + (PeekabooGameManager.Instance.TotalNumberOfPeopleFirstEnterdRoom - PeekabooGameManager.Instance.NumberOfPlayers + 124).ToString();
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
        // ������ �� �ְ� �÷��̾� ����
        Debug.Log("���� �̱���");
    }

}
