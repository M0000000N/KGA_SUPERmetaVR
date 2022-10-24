//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using System;

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

    private int surprisedEnemyNumbers;
    public int SurprisedEnemyNumbers { get { return surprisedEnemyNumbers; } set { surprisedEnemyNumbers = value; } }

    // ?? �̰� �߰��Ͻź�? ���� ���̴°���
    private PeekabooPlayerUIData peekabooPlayerUIData;


    private void Start()
    {
        peekabooPlayerUIData = playerPrefeb.GetComponentInChildren<PeekabooPlayerUIData>();
        surprisedEnemyNumbers = 0;
        IsGameOver = false;
        TotalNumberOfPeopleFirstEnterdRoom = PhotonNetwork.CountOfPlayers;
        numberOfPlayers = PhotonNetwork.CountOfPlayers;
    }

    private void Update()
    {
        // �÷��̾ �������� �����Ǹ� ������ ����
        if (numberOfPlayers == 1)
        {
            isGameOver = true;
        }
    }

    public void PlayerGameOver()
    {
        isGameOver = true;
        // �÷��̾� �̵� �� ������ ��� ��ȣ�ۿ� ����
    }


    private void PeekabooGameOver()
    {
        if (false) // �÷��̾ �׾��� ��
        {
            PlayerGameOver();
        }
        else if (numberOfPlayers == 1)
        {
            Debug.Log("ȥ�ڳ���");
            PlayerGameOver();
            
        }
        else if (PeekabooTimeManager.Instance.GameTimer <= 0f)
        {
            Debug.Log("�ð� �ٴ�");
            PlayerGameOver();
        }
        else if (false) // ���� ��ư�� ��������
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
}
