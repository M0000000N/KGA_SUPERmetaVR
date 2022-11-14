using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PeekabooSpawner : MonoBehaviour
{

    [SerializeField]
    private PeekabooCreateEnemy peekabooEnemyCreate;

    // 삭제할 부분
    private List<GameObject> testNPCList;

    private void Awake()
    {
        testNPCList = new List<GameObject>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("1번눌림");
            int randnum = Random.Range(0, 5);
            PeekabooCharacter NPC = testNPCList[randnum].GetComponentInChildren<PeekabooCharacter>();
            NPC.MyFSM.ChangeState(PEEKABOOCHARACTERSTATE.NPCDIE);
        }
    }
    //

    public void FirstSpawn(Vector3 _mapPosition, int _randomSpawnNPC)
    {
        for (int i = 0; i < _randomSpawnNPC; i++)
        {
            Spawn(_mapPosition); 
        }
    }

    private void Spawn(Vector3 _mapPosition)
    {
        Vector3 spawnPosition = GetRandomPointOnNavMesh(_mapPosition);
        spawnPosition += Vector3.up * 1f;
        transform.position = spawnPosition;
        int layerMask = LayerMask.GetMask("Enemy","Player");
        Collider[] colls = Physics.OverlapSphere(transform.position, PeekabooGameManager.Instance.CreateMap.DistanceBetweenCharactersCreated, layerMask);
        if (colls.Length > 0)
        {
            foreach (Collider col in colls)
            {
                //Debug.Log("123");
                if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Player")
                {
                    Spawn(_mapPosition);
                    break;
                }
            }
        }
        else
        {
            GameObject Enemy = peekabooEnemyCreate.GetObject(transform);
            // 테스트용
            testNPCList.Add(Enemy);
            //
            transform.position = new Vector3(0f, 0f, 0f);
        }
    }

    public Vector3 RespawnNPC(Vector3 _NPCposition)
    {
        PeekabooCreateMap createMap = PeekabooGameManager.Instance.CreateMap;
        float mapLength = createMap.MapLength;
        for (int i = 0; i < PeekabooGameManager.Instance.CreateMap.MapSize; i++)
        {
            if (createMap.MapData[i].MapPosition.x - mapLength / 2 < _NPCposition.x && createMap.MapData[i].MapPosition.x + mapLength / 2 > _NPCposition.x && createMap.MapData[i].MapPosition.z - mapLength / 2 < _NPCposition.z && createMap.MapData[i].MapPosition.z + mapLength / 2 > _NPCposition.z)
            {
                while (true)
                {
                    int respawnNPCIndex = Random.Range(0, createMap.MapSize);
                    if (respawnNPCIndex != i)
                    {
                        _NPCposition = GetRandomPointOnNavMesh(createMap.MapData[respawnNPCIndex].MapPosition);
                        Debug.Log($"리스폰 지정 위치 {_NPCposition}");
                        return _NPCposition;
                    }
                }
            }
        }
        return _NPCposition;
    }

    private Vector3 GetRandomPointOnNavMesh(Vector3 _center)
    {
        float mapLength = PeekabooGameManager.Instance.CreateMap.MapLength;
        float randomPositionX = Random.Range(_center.x - (mapLength / 2), _center.x + (mapLength / 2));
        float randomPositionZ = Random.Range(_center.z - (mapLength / 2), _center.z + (mapLength / 2));
        Vector3 randomPosition = new Vector3(randomPositionX, _center.y, randomPositionZ);

        NavMeshHit hit;

        NavMesh.SamplePosition(randomPosition, out hit, 5f, NavMesh.AllAreas);

        return hit.position;
    }


}
