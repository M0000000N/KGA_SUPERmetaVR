using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameManager : SingletonBehaviour<GameManager>
{
    PlayerData playerData;
    public PlayerData PlayerData { get { return playerData; } set { playerData = value; } }

    void Awake()
    {
        Initialize();
    }


    public GameObject PlayerPrefeb;
    public Button exitButton;
    [SerializeField]
    private PeekabooCreateMap createMap;
    public PeekabooCreateMap CreateMap { get { return createMap; } }

    public GameObject TestNPC;

    public void Start()
    {
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    private void OnClickExitButton()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("00_Title");
    }

    public void Initialize()
    {
        playerData = new PlayerData();
        PlayerData.PlayerPeekabooData = new PlayerPeekabooData();
        // 테스트 코드
        GameManager.Instance.PlayerData.PlayerPeekabooData.SelectCharacter = 1;
        GameManager.Instance.PlayerData.PlayerPeekabooData.Character = new int[5];
        GameManager.Instance.PlayerData.PlayerPeekabooData.Character[0] = 0;
        GameManager.Instance.PlayerData.PlayerPeekabooData.Character[1] = 1;
        GameManager.Instance.PlayerData.PlayerPeekabooData.Character[2] = 1;
        GameManager.Instance.PlayerData.PlayerPeekabooData.Character[3] = 1;
        GameManager.Instance.PlayerData.PlayerPeekabooData.Character[4] = 0;
        // 테스트 코드
    }
}
