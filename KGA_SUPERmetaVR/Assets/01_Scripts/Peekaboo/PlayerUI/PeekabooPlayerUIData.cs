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

    private float time;
    private void Start()
    {
        winnerColor = new Color(255, 192, 0);
        exitButton.onClick.AddListener(() => { GoRoom(); });
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

        Debug.Log("���ӿ���������");
        if (PeekabooGameManager.Instance.NumberOfPlayers == 1)
        {
            watchingButton.interactable = false;
            playerRankingText.color = winnerColor;
        }
        // �÷��̾ 2������ �Ͻ� �����ϱ� ��ư ��Ȱ��ȭ
        // ���ӽð��� 0�����Ͻ� ��� �����ϱ� ��ư ��Ȱ��ȭ
        playerRankingText.text = "# " + PeekabooGameManager.Instance.NumberOfPlayers.ToString();
        totalPlayerCount.text = "/ " + PeekabooGameManager.Instance.TotalNumberOfPeopleFirstEnterdRoom.ToString();
        // surprisedEnemyNumbers = �÷��̾ �Ų ��
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

}
