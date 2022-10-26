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
    [Header("스포너 스크립트")]
    [SerializeField]
    private PeekabooSpawner spawner;

    [Header("맵 가로 세로 크기")]
    [SerializeField]
    private int mapSizeX;
    [SerializeField]
    private int mapSizeZ;

    //[Header("테스트용 플레이어 수")]
    //[SerializeField]
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

    private int maxNumberOfNPCs;

    private Dictionary<int, MAPDATA> mapData;
    public Dictionary<int, MAPDATA> MapData { get { return mapData; } }

    private List<Vector3> playerPositionList;
    private Vector3 playerPosition;

    private void Awake()
    {
        mapLength = 20f;
        mapData = new Dictionary<int, MAPDATA>();
        playerPositionList = new List<Vector3>();
    }

    private void Start()
    {
        numberOfPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
        // Debug.Log($"서버룸 접속자 수{PhotonNetwork.CountOfPlayers}");
        mapSize = mapSizeX * mapSizeZ;
        maxNumberOfNPCs = numberOfNPCsProportionalToTheNumberOfPlayers * numberOfPlayers;
        CreateMap();
        Debug.Log($"마스터클라이언트{PhotonNetwork.IsMasterClient}");
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
                photonView.RPC("RPCPlayerAllocation", RpcTarget.All, i, playerPositionList[i]);
            }
        }
        
        SpawnNPC();
    }

    private void Update()
    {
        //Debug.Log($"서버룸 접속자 수{PhotonNetwork.CountOfPlayers}");
        //Debug.Log($"룸 접속자 수{PhotonNetwork.CurrentRoom.PlayerCount}");

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
                PeekabooGameManager.Instance.OVRCamera.transform.parent = player.transform.Find("2^1PC");
                PeekabooGameManager.Instance.OVRCamera.transform.localPosition = Vector3.zero;
            }
        }
    }

    //최종본에서 삭제
    //private void SpawnPlayer()
    //{
    //    int colls = 0;
    //    NavMeshHit hit;
    //    int randomPlayerIndex = Random.Range(0, mapSize);
    //    do
    //    {
    //        // 랜덤으로 정한 인덱스값의 인원 수가 하나의 인덱스의 최대 인원수랑 동일하면 최대 인원수가 현재 인원수 보다 적은 인덱스값을 찾는다   
    //        while (mapData[randomPlayerIndex].NumberOfPlayersCreatedInZone == 0)
    //        {
    //            randomPlayerIndex = Random.Range(0, mapSize);
    //        }
    //        // 인덱스값이 정해지면 인덱스에 해당하는 범위 중 한 곳을 랜덤으로 정한다
    //        Vector3 randomMapPosition = mapData[randomPlayerIndex].MapPosition;
    //        float randomPositonX = Random.Range(randomMapPosition.x - MapLength / 2, randomMapPosition.x + MapLength / 2);
    //        float randomPositonZ = Random.Range(randomMapPosition.z - MapLength / 2, randomMapPosition.z + MapLength / 2);

    //        Vector3 randomPosition = new Vector3(randomPositonX, 1f, randomPositonZ);

    //        // 랜덤으로 정한 곳이 베이크된 곳이 아니라면 반경 5f중 가장 가까운 곳을 정한다
    //        NavMesh.SamplePosition(randomPosition, out hit, 5f, NavMesh.AllAreas);
    //        hit.position += Vector3.up * 1f;
    //        // 정해진 범위내 같은 플레이어가 있는지 체크한다
    //        int layerMask = LayerMask.GetMask("Player");
    //        Collider[] hitColliders = new Collider[numberOfPlayers];
    //        colls = Physics.OverlapSphereNonAlloc(hit.position, distanceBetweenPlayersCreated, hitColliders, layerMask);
    //    } while (colls != 0); // 만약 플레이어가 있다면 다시 랜덤 위치값을 구한다.

    //    // 조건에 맞는 위치값이 나온다면 플레이어를 생성한다.
    //    GameObject playerObject = PhotonNetwork.Instantiate(PeekabooGameManager.Instance.PlayerPrefeb.name, hit.position, Quaternion.identity);
    //    // 위치값에 해당되는 인덱스의 존재가능한 플레이어 수를 줄인다
    //    --mapData[randomPlayerIndex].NumberOfPlayersCreatedInZone;
    //}

    private Vector3 SpawnPlayerPosition()
    {
        int colls = 0;
        NavMeshHit hit;
        int randomPlayerIndex = Random.Range(0, mapSize);
        do
        {
            // 랜덤으로 정한 인덱스값의 인원 수가 하나의 인덱스의 최대 인원수랑 동일하면 최대 인원수가 현재 인원수 보다 적은 인덱스값을 찾는다   
            while (mapData[randomPlayerIndex].NumberOfPlayersCreatedInZone == 0)
            {
                randomPlayerIndex = Random.Range(0, mapSize);
            }
            // 인덱스값이 정해지면 인덱스에 해당하는 범위 중 한 곳을 랜덤으로 정한다
            Vector3 randomMapPosition = mapData[randomPlayerIndex].MapPosition;
            float randomPositonX = Random.Range(randomMapPosition.x - MapLength / 2, randomMapPosition.x + MapLength / 2);
            float randomPositonZ = Random.Range(randomMapPosition.z - MapLength / 2, randomMapPosition.z + MapLength / 2);

            Vector3 randomPosition = new Vector3(randomPositonX, 1f, randomPositonZ);

            // 랜덤으로 정한 곳이 베이크된 곳이 아니라면 반경 5f중 가장 가까운 곳을 정한다
            NavMesh.SamplePosition(randomPosition, out hit, 5f, NavMesh.AllAreas);
            hit.position += Vector3.up * 1f;
            // 정해진 범위내 같은 플레이어가 있는지 체크한다
            int layerMask = LayerMask.GetMask("Player");
            Collider[] hitColliders = new Collider[numberOfPlayers];
            colls = Physics.OverlapSphereNonAlloc(hit.position, distanceBetweenPlayersCreated, hitColliders, layerMask);
        } while (colls != 0); // 만약 플레이어가 있다면 다시 랜덤 위치값을 구한다.

        // 조건에 맞는 위치값이 나온다면 플레이어를 생성한다.
        //GameObject playerObject = PhotonNetwork.Instantiate(PeekabooGameManager.Instance.PlayerPrefeb.name, hit.position, Quaternion.identity);
        // 위치값에 해당되는 인덱스의 존재가능한 플레이어 수를 줄인다
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
