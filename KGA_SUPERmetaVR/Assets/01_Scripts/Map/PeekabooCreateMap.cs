using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class MAPDATA
{
    public int numberOfPlayersCreatedInZone;
    public Vector3 mapposiotion;
    public int numberOfNPCPlacedInZone = 0;
}

public class PeekabooCreateMap : MonoBehaviour
{
    [SerializeField]
    private int numberOfPlayersCreatedInZone;
    [SerializeField]
    private int mapSizeX;
    [SerializeField]
    private int mapSizeZ;
    //public int MapSizeX { get { return mapSizeX; } }
    //public int MapSizeZ { get { return mapSizeZ; } }
    [SerializeField]
    private GameObject mapPrefab;
    [SerializeField]
    private PeekabooSpawner spawner;

    //private int mapIndex;
    [SerializeField]
    private int maxCharcter;

    [SerializeField]
    private float mapLength;
    public float MapLength { get { return mapLength; } }

    private int numberOfNPCs;

    private int mapSize;
    [SerializeField]
    private int numberOfPlayers;

    private int maxNumberOfNPCs;


    [SerializeField]
    private int numberOfNPCsProportionalToTheNumberOfPlayers;


    private Dictionary<int,MAPDATA> mapData;

    //public Dictionary<int, MAPDATA> MapData { get { return mapData; } }


    private void Awake()
    {
        //mapIndex = 0;

        mapData = new Dictionary<int, MAPDATA>();
        CreateMap();
    }

    private void Start()
    {

        mapSize = mapSizeX * mapSizeZ;
        maxNumberOfNPCs = numberOfNPCsProportionalToTheNumberOfPlayers * numberOfPlayers;
        //테스트용
        for (int i = 0; i < numberOfPlayers; i++)
        {
            SpawnPlayer();
        }
        //
        //SpawnPlayer();
        SpawnNPC();
    }

    private void CreateMap()
    {
        int mapindex = 0;
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int z = 0; z < mapSizeZ; z++)
            {
                Vector3 mapPosition = new Vector3(x * mapLength, 0, z * mapLength);
                PhotonNetwork.Instantiate(mapPrefab.name,mapPosition,Quaternion.identity);
                
                //spawner.FirstSpawn(mapPosition,5);
                MAPDATA mapdata = new MAPDATA();
                mapdata.numberOfPlayersCreatedInZone = numberOfPlayersCreatedInZone;
                mapdata.mapposiotion = mapPosition;
                mapdata.numberOfNPCPlacedInZone = 0;
                mapData.Add(mapindex,mapdata);
                mapindex++;
            }
        }
    }

    private void SpawnPlayer()
    {
        int randomPlayerposition = Random.Range(0, mapSize);

        float randomPosX = Random.Range(mapData[randomPlayerposition].mapposiotion.x - MapLength / 2, mapData[randomPlayerposition].mapposiotion.x + MapLength / 2);
        float randomPosZ = Random.Range(mapData[randomPlayerposition].mapposiotion.z - MapLength / 2, mapData[randomPlayerposition].mapposiotion.z + MapLength / 2);
        Vector3 randomPos = new Vector3(randomPosX, 1f, randomPosZ);
        if (mapData[randomPlayerposition].numberOfPlayersCreatedInZone == 0)
        {
            SpawnPlayer();
        }
        else
        {
            //Debug.Log($"{randomPlayerposition}");
            GameObject playerObject = PhotonNetwork.Instantiate(GameManager.Instance.PlayerPrefeb.name, randomPos, Quaternion.identity);
            --mapData[randomPlayerposition].numberOfPlayersCreatedInZone;
            //Debug.Log($"남은 인원수 {mapData[randomPlayerposition].numberOfPlayersCreatedInZone}");
        }

    }


    private void SpawnNPC()
    {
        int mapindex = 0;
       while (true)
        {
            if (maxNumberOfNPCs > 0 && mapindex < mapSizeX * mapSizeZ)
            {
                int numberOfRandomNPCSpawn = Random.Range(0, maxCharcter + 1 + (mapData[mapindex].numberOfPlayersCreatedInZone - numberOfPlayersCreatedInZone) - mapData[mapindex].numberOfNPCPlacedInZone);
                if (maxNumberOfNPCs - numberOfRandomNPCSpawn < 0)
                {
                    numberOfRandomNPCSpawn = maxNumberOfNPCs;
                }
                spawner.FirstSpawn(mapData[mapindex].mapposiotion, numberOfRandomNPCSpawn);
                mapData[mapindex].numberOfNPCPlacedInZone += numberOfRandomNPCSpawn;
                maxNumberOfNPCs -= numberOfRandomNPCSpawn;
                Debug.Log($"맵데이타포지션 {mapindex},{numberOfRandomNPCSpawn},{mapData[mapindex].numberOfNPCPlacedInZone}, {maxNumberOfNPCs}");
                mapindex++;
            }
            else if (maxNumberOfNPCs > 0 && mapindex >= mapSizeX * mapSizeZ)
            {
                mapindex = 0;
                SpawnNPC();
            }
            else
            {
                break;
            }
        }
                
       
       
    }



}
