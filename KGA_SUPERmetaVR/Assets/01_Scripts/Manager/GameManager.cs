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

    public PeekabooCreateMap CreateMap { get { return createMap; } }

    //private int numberOfPlayers;

    //private int maxNumberOfNPCs;
    //[SerializeField]
    //private int numberOfNPCsProportionalToTheNumberOfPlayers;
    //public int NumberOfNPCs { get { return maxNumberOfNPCs; } }


    public void Start()
    {
        //numberOfPlayers = createMap.MapSizeX * createMap.MapSizeZ;
        //maxNumberOfNPCs = numberOfNPCsProportionalToTheNumberOfPlayers * numberOfPlayers;
        ////테스트용
        //for (int i = 0; i < 15; i++)
        //{
        //    PlayerSpawn();
        //}
        ////
        //PlayerSpawn();

        exitButton.onClick.AddListener(OnClickExitButton);
    }

    private void OnClickExitButton()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("00_Title");
    }

    //void PlayerSpawn()
    //{
    //    int randomPlayerposition = Random.Range(0, numberOfPlayers);
        
    //    float randomPosX = Random.Range(createMap.MapData[randomPlayerposition].mapposiotion.x - createMap.MapLength / 2, createMap.MapData[randomPlayerposition].mapposiotion.x + createMap.MapLength / 2);
    //    float randomPosZ = Random.Range(createMap.MapData[randomPlayerposition].mapposiotion.z - createMap.MapLength / 2, createMap.MapData[randomPlayerposition].mapposiotion.z + createMap.MapLength / 2);
    //    Vector3 randomPos = new Vector3(randomPosX, 1f, randomPosZ);
    //    //createMap.Map.RemoveAt(randomPlayerposition);
    //    if (createMap.MapData[randomPlayerposition].numberOfPlayersCreatedInZone == 0)
    //    {
    //        PlayerSpawn();
    //    }
    //    else
    //    {
    //        Debug.Log($"{randomPlayerposition}");
    //        GameObject playerObject = PhotonNetwork.Instantiate(PlayerPrefeb.name, randomPos, Quaternion.identity);
    //        --createMap.MapData[randomPlayerposition].numberOfPlayersCreatedInZone;
    //        Debug.Log($"남은 인원수 {createMap.MapData[randomPlayerposition].numberOfPlayersCreatedInZone}");
    //    }
        
    //    //--numberOfPlayers;
    //}
}
