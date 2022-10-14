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
    public void Start()
    {
        numberOfPlayers = PhotonNetwork.CountOfPlayers;
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    public void GameOver()
    {
        // 플레이어 이동 및 시점등 모든 상호작용 멈춤
        PeekabooUIManager.Instance.GameOverUI();
    }

    private void OnClickExitButton()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("00_Title");
    }
}
