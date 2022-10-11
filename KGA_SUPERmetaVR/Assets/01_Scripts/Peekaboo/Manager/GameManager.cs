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

    public void Initialize()
    {
        playerData = new PlayerData();
        PlayerData.PlayerPeekabooData = new PlayerPeekabooData();
        // 테스트 코드
        GameManager.Instance.PlayerData.PlayerPeekabooData.SelectCharacter = 1;
        GameManager.Instance.PlayerData.PlayerPeekabooData.Character = new int[5];
        for (int i = 0; i < GameManager.Instance.PlayerData.PlayerPeekabooData.Character.Length; i++)
        {
            GameManager.Instance.PlayerData.PlayerPeekabooData.Character[i] = i;
        }
        // 테스트 코드
    }

    public GameObject PlayerPrefeb;
    public Button exitButton;
    public void Start()
    {
        float randomPosX = Random.Range(-30f, 30f);
        float randomPosZ = Random.Range(-30f, 30f);
        Vector3 randomPos = new Vector3(randomPosX, 1f, randomPosZ);

        GameObject playerObject = PhotonNetwork.Instantiate(PlayerPrefeb.name, randomPos, Quaternion.identity);

        exitButton.onClick.AddListener(OnClickExitButton);
    }

    private void OnClickExitButton()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("00_Title");
    }
}
