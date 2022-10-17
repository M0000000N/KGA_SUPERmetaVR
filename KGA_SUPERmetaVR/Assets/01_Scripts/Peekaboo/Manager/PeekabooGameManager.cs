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
        // 플레이어 이동 및 시점등 모든 상호작용 멈춤
        PeekabooUIManager.Instance.GameOverUI(numberOfPlayers);
    }

    private void OnClickExitButton()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("00_Title");
    }

    private void PeekabooGameOver()
    {
        if (true) // 플레이어가 죽었을 때
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
        else if (true) // 종료 버튼을 눌럿을시
        {
            PlayerGameOver();
        }
    }

    private void PeekabooEnforceGameShutdown()
    {
        // 강제로 종료할 시 카운트 하나 줄임
        // 플레이어에서 관리? 여기서 관리?
        PlayerGameOver();
    }

    public void WatchingStatePlayer()
    {
        // 관전할 수 있게 플레이어 변경'
        Debug.Log("관전 미구현");
    }

    public void LeavePeekabooGame()
    {
        PhotonNetwork.LoadLevel("Login");
    }
}
