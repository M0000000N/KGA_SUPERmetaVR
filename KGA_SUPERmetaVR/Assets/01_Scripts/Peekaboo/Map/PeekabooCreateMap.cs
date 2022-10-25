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

public class PeekabooCreateMap : MonoBehaviourPunCallbacks
{
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

    private List<Vector3> playerPositionList;

    private Vector3 playerPosition;

    private List<GameObject> dummyPlayerList;

    [SerializeField]
    private GameObject dummyPlayer;
    private void Awake()
    {
        mapLength = 20f;
        mapData = new Dictionary<int, MAPDATA>();
        playerPositionList = new List<Vector3>();
        if (PhotonNetwork.IsMasterClient)
        {
            dummyPlayerList = new List<GameObject>();
        }
    }

    private void Start()
    {
        numberOfPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
        // Debug.Log($"������ ������ ��{PhotonNetwork.CountOfPlayers}");
        mapSize = mapSizeX * mapSizeZ;
        maxNumberOfNPCs = numberOfNPCsProportionalToTheNumberOfPlayers * numberOfPlayers;
        CreateMap();
        Debug.Log($"������Ŭ���̾�Ʈ{PhotonNetwork.IsMasterClient}");
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < numberOfPlayers; ++i)
            {
                playerPositionList.Add(SpawnPlayerPosition());
            }
        }
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < numberOfPlayers; ++i)
            {
                Destroy(dummyPlayerList[i]);
            }
        }
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < numberOfPlayers; ++i)
            {
                photonView.RPC("RPCPlayerAllocation", RpcTarget.All, i, playerPositionList[i]);
            }
        }
        SpawnNPC();
    }

    private void Update()
    {
        Debug.Log($"������ ������ ��{PhotonNetwork.CountOfPlayers}");
        Debug.Log($"�� ������ ��{PhotonNetwork.CurrentRoom.PlayerCount}");

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

    [PunRPC]
    private void RPCPlayerAllocation(int index, Vector3 _playerList)
    {
        if (index == PhotonNetwork.LocalPlayer.ActorNumber - 1)
        {
            GameObject player = PhotonNetwork.Instantiate(PeekabooGameManager.Instance.PlayerPrefeb.name, _playerList, Quaternion.identity);
            if(player.GetComponentInChildren<PhotonView>().IsMine)
            {
                PeekabooGameManager.Instance.OVRCamera.transform.parent = player.transform;
                PeekabooGameManager.Instance.OVRCamera.transform.localPosition = Vector3.zero;
            }
        }
    }

    //���������� ����
    //private void SpawnPlayer()
    //{
    //    int colls = 0;
    //    NavMeshHit hit;
    //    int randomPlayerIndex = Random.Range(0, mapSize);
    //    do
    //    {
    //        // �������� ���� �ε������� �ο� ���� �ϳ��� �ε����� �ִ� �ο����� �����ϸ� �ִ� �ο����� ���� �ο��� ���� ���� �ε������� ã�´�   
    //        while (mapData[randomPlayerIndex].NumberOfPlayersCreatedInZone == 0)
    //        {
    //            randomPlayerIndex = Random.Range(0, mapSize);
    //        }
    //        // �ε������� �������� �ε����� �ش��ϴ� ���� �� �� ���� �������� ���Ѵ�
    //        Vector3 randomMapPosition = mapData[randomPlayerIndex].MapPosition;
    //        float randomPositonX = Random.Range(randomMapPosition.x - MapLength / 2, randomMapPosition.x + MapLength / 2);
    //        float randomPositonZ = Random.Range(randomMapPosition.z - MapLength / 2, randomMapPosition.z + MapLength / 2);

    //        Vector3 randomPosition = new Vector3(randomPositonX, 1f, randomPositonZ);

    //        // �������� ���� ���� ����ũ�� ���� �ƴ϶�� �ݰ� 5f�� ���� ����� ���� ���Ѵ�
    //        NavMesh.SamplePosition(randomPosition, out hit, 5f, NavMesh.AllAreas);
    //        hit.position += Vector3.up * 1f;
    //        // ������ ������ ���� �÷��̾ �ִ��� üũ�Ѵ�
    //        int layerMask = LayerMask.GetMask("Player");
    //        Collider[] hitColliders = new Collider[numberOfPlayers];
    //        colls = Physics.OverlapSphereNonAlloc(hit.position, distanceBetweenPlayersCreated, hitColliders, layerMask);
    //    } while (colls != 0); // ���� �÷��̾ �ִٸ� �ٽ� ���� ��ġ���� ���Ѵ�.

    //    // ���ǿ� �´� ��ġ���� ���´ٸ� �÷��̾ �����Ѵ�.
    //    GameObject playerObject = PhotonNetwork.Instantiate(PeekabooGameManager.Instance.PlayerPrefeb.name, hit.position, Quaternion.identity);
    //    // ��ġ���� �ش�Ǵ� �ε����� ���簡���� �÷��̾� ���� ���δ�
    //    --mapData[randomPlayerIndex].NumberOfPlayersCreatedInZone;
    //}

    private Vector3 SpawnPlayerPosition()
    {
        int colls = 0;
        NavMeshHit hit;
        int randomPlayerIndex = Random.Range(0, mapSize);
        do
        {
            // �������� ���� �ε������� �ο� ���� �ϳ��� �ε����� �ִ� �ο����� �����ϸ� �ִ� �ο����� ���� �ο��� ���� ���� �ε������� ã�´�   
            while (mapData[randomPlayerIndex].NumberOfPlayersCreatedInZone == 0)
            {
                randomPlayerIndex = Random.Range(0, mapSize);
            }
            // �ε������� �������� �ε����� �ش��ϴ� ���� �� �� ���� �������� ���Ѵ�
            Vector3 randomMapPosition = mapData[randomPlayerIndex].MapPosition;
            float randomPositonX = Random.Range(randomMapPosition.x - MapLength / 2, randomMapPosition.x + MapLength / 2);
            float randomPositonZ = Random.Range(randomMapPosition.z - MapLength / 2, randomMapPosition.z + MapLength / 2);

            Vector3 randomPosition = new Vector3(randomPositonX, 1f, randomPositonZ);

            // �������� ���� ���� ����ũ�� ���� �ƴ϶�� �ݰ� 5f�� ���� ����� ���� ���Ѵ�
            NavMesh.SamplePosition(randomPosition, out hit, 5f, NavMesh.AllAreas);
            hit.position += Vector3.up * 1f;
            // ������ ������ ���� �÷��̾ �ִ��� üũ�Ѵ�
            int layerMask = LayerMask.GetMask("Player");
            Collider[] hitColliders = new Collider[numberOfPlayers];
            colls = Physics.OverlapSphereNonAlloc(hit.position, distanceBetweenPlayersCreated, hitColliders, layerMask);
            dummyPlayerList.Add(Instantiate(dummyPlayer, hit.position, Quaternion.identity));
        } while (colls != 0); // ���� �÷��̾ �ִٸ� �ٽ� ���� ��ġ���� ���Ѵ�.

        // ���ǿ� �´� ��ġ���� ���´ٸ� �÷��̾ �����Ѵ�.
        //GameObject playerObject = PhotonNetwork.Instantiate(PeekabooGameManager.Instance.PlayerPrefeb.name, hit.position, Quaternion.identity);
        // ��ġ���� �ش�Ǵ� �ε����� ���簡���� �÷��̾� ���� ���δ�
        --mapData[randomPlayerIndex].NumberOfPlayersCreatedInZone;
        return hit.position;
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
                    int numberOfRandomNPCSpawn = Random.Range(0, (maximumNumberOfCharactersInZone + 1 + (mapData[mapindex].NumberOfPlayersCreatedInZone - numberOfPlayersCreatedInZone) - mapData[mapindex].NumberOfNPCPlacedInZone) / 4);
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

}
