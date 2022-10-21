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

public class PeekabooCreateMap : MonoBehaviourPunCallbacks, IPunObservable
{
    //[Header("�� ������")]
    //[SerializeField]
    //private GameObject mapPrefab;

    [Header("������ ��ũ��Ʈ")]
    [SerializeField]
    private PeekabooSpawner spawner;

    [Header("�� ���� ���� ũ��")]
    [SerializeField]
    private int mapSizeX;
    [SerializeField]
    private int mapSizeZ;

  //[Header("�׽�Ʈ�� �÷��̾� ��")]
    //[SerializeField]
    private int numberOfPlayers;

    [Header("���� ���۽� �� ������ �����Ǵ� �ִ� �÷��̾� ��")]
    [SerializeField]
    private int numberOfPlayersCreatedInZone;

    [Header("���� ���۽� �� ������ �����ϴ� �ִ� ĳ���� ��")]
    [SerializeField]
    private int maximumNumberOfCharactersInZone;

    [Header("�÷��̾� �� �� �� �����Ǵ� NPC��")]
    [SerializeField]
    private int numberOfNPCsProportionalToTheNumberOfPlayers;

    [Header("ó�� �÷��̾� ������ �÷��̾�� �Ÿ�(���� ũ�� ���� �߻�)")]
    [SerializeField]
    private float distanceBetweenPlayersCreated;

    [Header("ó�� NPC ������ ĳ���Ͱ��� �Ÿ�(���� ũ�� ���� �߻�)")]
    [SerializeField]
    private float distanceBetweenCharactersCreated;

    public float DistanceBetweenCharactersCreated { get { return distanceBetweenCharactersCreated; } }

    private float mapLength;
    public float MapLength { get { return mapLength; } }

    private int mapSize;
    public int MapSize { get { return mapSize; } }

    private int maxNumberOfNPCs;

    private Dictionary<int, MAPDATA> mapData;

    public Dictionary<int, MAPDATA> MapData { get { return mapData; } }


    private void Awake()
    {
        mapLength = 20f;
        mapData = new Dictionary<int, MAPDATA>();
    }

    private void Start()
    {
        numberOfPlayers = PhotonNetwork.CountOfPlayers;
        Debug.Log($"������ ������ ��{PhotonNetwork.CountOfPlayers}");
        mapSize = mapSizeX * mapSizeZ;
        maxNumberOfNPCs = numberOfNPCsProportionalToTheNumberOfPlayers * numberOfPlayers;
        CreateMap();
        //�׽�Ʈ��
        //for (int i = 0; i < numberOfPlayers; i++)
        //{
        //    SpawnPlayer();
        //}
        SpawnPlayer();
        SpawnNPC();
    }

    private void CreateMap()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int mapIndex = 0;
            for (int x = 0; x < mapSizeX; x++)
            {
                for (int z = 0; z < mapSizeZ; z++)
                {
                    Vector3 mapPosition = new Vector3(x * mapLength, 0, z * mapLength);
                    MAPDATA mapdata = new MAPDATA();
                    mapdata.NumberOfPlayersCreatedInZone = numberOfPlayersCreatedInZone;
                    mapdata.MapPosition = mapPosition;
                    mapdata.NumberOfNPCPlacedInZone = 0;
                    mapData.Add(mapIndex, mapdata);
                    mapIndex++;
                }
            }
        }
    }

    private void SpawnPlayer()
    {
        int randomPlayerIndex = Random.Range(0, mapSize);

        float randomPositonX = Random.Range(mapData[randomPlayerIndex].MapPosition.x - MapLength / 2, mapData[randomPlayerIndex].MapPosition.x + MapLength / 2);
        float randomPositonZ = Random.Range(mapData[randomPlayerIndex].MapPosition.z - MapLength / 2, mapData[randomPlayerIndex].MapPosition.z + MapLength / 2);
        Vector3 randomPosition = new Vector3(randomPositonX, 1f, randomPositonZ);
        NavMeshHit hit;

        NavMesh.SamplePosition(randomPosition, out hit, 5f, NavMesh.AllAreas);
        hit.position += Vector3.up * 1f;
        if (mapData[randomPlayerIndex].NumberOfPlayersCreatedInZone == 0)
        {
            SpawnPlayer();
        }
        else
        {
            int layerMask = LayerMask.GetMask("Player");
            Collider[] colls = Physics.OverlapSphere(hit.position, distanceBetweenPlayersCreated, layerMask);
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
                GameObject playerObject = PhotonNetwork.Instantiate(PeekabooGameManager.Instance.PlayerPrefeb.name, hit.position, Quaternion.identity);
                --mapData[randomPlayerIndex].NumberOfPlayersCreatedInZone;
            }
            
        }
    }

    private void SpawnNPC()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int mapindex = 0;
            while (true)
            {
                if (maxNumberOfNPCs > 0 && mapindex < mapSize)
                {
                    int numberOfRandomNPCSpawn = Random.Range(0, (maximumNumberOfCharactersInZone + 1 + (mapData[mapindex].NumberOfPlayersCreatedInZone - numberOfPlayersCreatedInZone) - mapData[mapindex].NumberOfNPCPlacedInZone) / 2);
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            for (int i = 0; i < numberOfPlayers; ++i)
            {
                stream.SendNext(mapData[i].NumberOfPlayersCreatedInZone);
            }
        }
        else
        {
            for (int i = 0; i < numberOfPlayers; ++i)
            {
                mapData[i].NumberOfPlayersCreatedInZone = (int)stream.ReceiveNext();
            }
        }
    }
}
