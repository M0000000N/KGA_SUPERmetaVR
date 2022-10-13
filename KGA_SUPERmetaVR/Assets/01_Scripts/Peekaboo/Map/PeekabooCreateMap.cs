using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class MAPDATA
{
    private int numberOfPlayersCreatedInZone;
    public int NumberOfPlayersCreatedInZone { get { return numberOfPlayersCreatedInZone; } set { numberOfPlayersCreatedInZone = value; } }
    private Vector3 mapPosition;
    public Vector3 MapPosition { get { return mapPosition; } set { mapPosition = value; } }
    private int numberOfNPCPlacedInZone;
    public int NumberOfNPCPlacedInZone { get { return numberOfNPCPlacedInZone; } set { numberOfNPCPlacedInZone = value; } }
}

public class PeekabooCreateMap : MonoBehaviour
{
    [Header("맵 프리팹")]
    [SerializeField]
    private GameObject mapPrefab;

    [Header("스포너 스크립트")]
    [SerializeField]
    private PeekabooSpawner spawner;

    [Header("맵 가로 세로 크기")]
    [SerializeField]
    private int mapSizeX;
    [SerializeField]
    private int mapSizeZ;

    [Header("테스트용 플레이어 수")]
    [SerializeField]
    private int numberOfPlayers;

    [Header("게임 시작시 각 구역에 스폰되는 최대 플레이어 수")]
    [SerializeField]
    private int numberOfPlayersCreatedInZone;

    [Header("게임 시작시 각 구역에 존재하는 최대 캐릭터 수")]
    [SerializeField]
    private int maximumNumberOfCharactersInZone;

    [Header("플레이어 한 명 당 생성되는 NPC수")]
    [SerializeField]
    private int numberOfNPCsProportionalToTheNumberOfPlayers;

    [Header("처음 플레이어 생성시 플레이어간의 거리(값이 크면 오류 발생)")]
    [SerializeField]
    private float distanceBetweenPlayersCreated;

    [Header("처음 NPC 생성시 캐릭터간의 거리(값이 크면 오류 발생)")]
    [SerializeField]
    private float distanceBetweenCharactersCreated;

    public float DistanceBetweenCharactersCreated { get { return distanceBetweenCharactersCreated; } }

    private float mapLength;
    public float MapLength { get { return mapLength; } }

    private int mapSize;
    public int MapSize { get { return mapSize; } }
    // 서버에서 플레이어 인원 받아오면 수정 예정

    private int maxNumberOfNPCs;

    private Dictionary<int, MAPDATA> mapData;

    public Dictionary<int, MAPDATA> MapData { get { return mapData; } }


    private void Awake()
    {
        mapLength = 20f;
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
        SpawnNPC();
    }

    private void CreateMap()
    {
        int mapIndex = 0;
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int z = 0; z < mapSizeZ; z++)
            {
                Vector3 mapPosition = new Vector3(x * mapLength, 0, z * mapLength);
                PhotonNetwork.Instantiate(mapPrefab.name, mapPosition, Quaternion.identity);

                MAPDATA mapdata = new MAPDATA();
                mapdata.NumberOfPlayersCreatedInZone = numberOfPlayersCreatedInZone;
                mapdata.MapPosition = mapPosition;
                mapdata.NumberOfNPCPlacedInZone = 0;
                mapData.Add(mapIndex, mapdata);
                mapIndex++;
            }
        }
    }

    private void SpawnPlayer()
    {
        int randomPlayerIndex = Random.Range(0, mapSize);

        float randomPositonX = Random.Range(mapData[randomPlayerIndex].MapPosition.x - MapLength / 2, mapData[randomPlayerIndex].MapPosition.x + MapLength / 2);
        float randomPositonZ = Random.Range(mapData[randomPlayerIndex].MapPosition.z - MapLength / 2, mapData[randomPlayerIndex].MapPosition.z + MapLength / 2);
        Vector3 randomPosition = new Vector3(randomPositonX, 1f, randomPositonZ);
        if (mapData[randomPlayerIndex].NumberOfPlayersCreatedInZone == 0)
        {
            SpawnPlayer();
        }
        else
        {
            int layerMask = LayerMask.GetMask("Player");
            Collider[] colls = Physics.OverlapSphere(randomPosition, distanceBetweenPlayersCreated, layerMask);
            if(colls.Length > 0)
            {
                foreach (Collider col in colls)
                {
                    if (col.gameObject.tag == "Player")
                    {
                        SpawnPlayer();
                        break;
                    }
                }
            }
            else
            {
                GameObject playerObject = PhotonNetwork.Instantiate(PeekabooGameManager.Instance.PlayerPrefeb.name, randomPosition, Quaternion.identity);
                --mapData[randomPlayerIndex].NumberOfPlayersCreatedInZone;
            }
            
        }
    }

    private void SpawnNPC()
    {
        int mapindex = 0;
        while (true)
        {
            if (maxNumberOfNPCs > 0 && mapindex < mapSize)
            {
                int numberOfRandomNPCSpawn = Random.Range(0, maximumNumberOfCharactersInZone + 1 + (mapData[mapindex].NumberOfPlayersCreatedInZone - numberOfPlayersCreatedInZone) - mapData[mapindex].NumberOfNPCPlacedInZone);
                if (maxNumberOfNPCs - numberOfRandomNPCSpawn < 0)
                {
                    numberOfRandomNPCSpawn = maxNumberOfNPCs;
                }
                spawner.FirstSpawn(mapData[mapindex].MapPosition, numberOfRandomNPCSpawn);
                mapData[mapindex].NumberOfNPCPlacedInZone += numberOfRandomNPCSpawn;
                maxNumberOfNPCs -= numberOfRandomNPCSpawn;
                mapindex++;
            }
            else if (maxNumberOfNPCs > 0 && mapindex >= mapSize)
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
