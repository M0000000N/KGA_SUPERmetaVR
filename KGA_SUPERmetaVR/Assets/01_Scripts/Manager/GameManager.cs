using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameManager : SingletonBehaviour<GameManager>
{
    public GameObject PlayerPrefeb;
    public Button exitButton;
    [SerializeField]
    private PeekabooCreateMap createMap;

    private int playerCount;
    
    public void Start()
    {
        playerCount = 2 * createMap.MapSize * createMap.MapSize;
        //테스트용
        for (int i = 0; i < 15; i++)
        {
            PlayerSpawn();
        }
        //
        PlayerSpawn();

        exitButton.onClick.AddListener(OnClickExitButton);
    }

    private void OnClickExitButton()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("00_Title");
    }

    void PlayerSpawn()
    {
        int randomPlayerposition = Random.Range(0, playerCount);
        Debug.Log($"{randomPlayerposition}");
        float randomPosX = Random.Range(createMap.Map[randomPlayerposition].x - createMap.MapLength / 2, createMap.Map[randomPlayerposition].x + createMap.MapLength / 2);
        float randomPosZ = Random.Range(createMap.Map[randomPlayerposition].z - createMap.MapLength / 2, createMap.Map[randomPlayerposition].z + createMap.MapLength / 2);
        Vector3 randomPos = new Vector3(randomPosX, 1f, randomPosZ);
        createMap.Map.RemoveAt(randomPlayerposition);
        GameObject playerObject = PhotonNetwork.Instantiate(PlayerPrefeb.name, randomPos, Quaternion.identity);
        --playerCount;
    }
}
