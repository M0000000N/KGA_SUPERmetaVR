using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PeekabooGameManager : OnlyOneSceneSingleton<PeekabooGameManager>
{
    public GameObject PlayerPrefeb;
    public Button exitButton;
    [SerializeField]
    private PeekabooCreateMap createMap;
    public PeekabooCreateMap CreateMap { get { return createMap; } }

    public GameObject TestNPC;

    private int numberOfPlayers;
    public int NumberOfPlayers { get { return numberOfPlayers; } set { numberOfPlayers = value; } }

    public int TotalNumberOfPeopleFirstEnterdRoom { get; private set; }

    private bool isGameOver;
    public bool IsGameOver { get { return isGameOver; } set { isGameOver = value; } }
    public void Start()
    {
        IsGameOver = false;
        TotalNumberOfPeopleFirstEnterdRoom = PhotonNetwork.CountOfPlayers;
        numberOfPlayers = PhotonNetwork.CountOfPlayers;
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    public void PlayerGameOver()
    {
        // �÷��̾� �̵� �� ������ ��� ��ȣ�ۿ� ����
        PeekabooUIManager.Instance.GameOverUI(numberOfPlayers);
    }

    private void OnClickExitButton()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("00_Title");
    }

    private void PeekabooGameOver()
    {
        if (true) // �÷��̾ �׾��� ��
        {
            PlayerGameOver();
        }
        else if (numberOfPlayers == 1)
        {
            PlayerGameOver();
        }
        else if (PeekabooTimeManager.Instance.GameTimer <= 0f)
        {
            PlayerGameOver();
        }
        else if (true) // ���� ��ư�� ��������
        {
            PlayerGameOver();
        }
    }

    private void PeekabooEnforceGameShutdown()
    {
        // ������ ������ �� ī��Ʈ �ϳ� ����
        // �÷��̾�� ����? ���⼭ ����?
        PlayerGameOver();
    }

    public void WatchingStatePlayer()
    {
        // ������ �� �ְ� �÷��̾� ����'
        Debug.Log("���� �̱���");
    }

    public void LeavePeekabooGame()
    {
        PhotonNetwork.LoadLevel("Login");
    }
}
