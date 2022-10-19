using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PeekabooSpawner : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    // 테스트용 삭제
    private float elsptime;
    //


    private void Update()
    {
        //// 리스폰 테스트용 삭제
        //elsptime += Time.deltaTime;
        //if (elsptime > 3f)
        //{
        //    float x = Random.Range(-10f, 30f);
        //    float z = Random.Range(-10f, 70f);
        //    Vector3 npcPosition = new Vector3(x, 0, z);
        //    RespawnNPC(npcPosition);
        //    elsptime = -3000f;
        //}
        ////
    }
    public void FirstSpawn(Vector3 _mapPosition, int _randomSpawnNPC)
    {
        //navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        for (int i = 0; i < _randomSpawnNPC; i++)
        {
            Spawn(_mapPosition); // 풀링으로 NPC 여러개를 만들어서 배치하는 녀석을 참조갛고 싶은거임 ㅇㅋ? 
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
            var monster = PeekabooEnemyObjectPool.GetObject(transform);
            transform.position = new Vector3(0f, 0f, 0f);
        }
       
    }

    public void RespawnNPC(Vector3 _NPCposition)
    {
        for (int i = 0; i < PeekabooGameManager.Instance.CreateMap.MapSize; i++)
        {
            if (PeekabooGameManager.Instance.CreateMap.MapData[i].MapPosition.x - PeekabooGameManager.Instance.CreateMap.MapLength / 2 < _NPCposition.x && PeekabooGameManager.Instance.CreateMap.MapData[i].MapPosition.x + PeekabooGameManager.Instance.CreateMap.MapLength / 2 > _NPCposition.x && PeekabooGameManager.Instance.CreateMap.MapData[i].MapPosition.z - PeekabooGameManager.Instance.CreateMap.MapLength / 2 < _NPCposition.z && PeekabooGameManager.Instance.CreateMap.MapData[i].MapPosition.z + PeekabooGameManager.Instance.CreateMap.MapLength / 2 > _NPCposition.z)
            {
                while (true)
                {
                    int respawnNPCIndex = Random.Range(0, PeekabooGameManager.Instance.CreateMap.MapSize);
                    if (respawnNPCIndex != i)
                    {
                        Debug.Log("리스폰되는중");
                        //Vector3 spawnPosition = GetRandomPointOnNavMesh(PeekabooGameManager.Instance.CreateMap.MapData[respawnNPCIndex].MapPosition);
                        //spawnPosition += Vector3.up * 20f;
                        //transform.position = spawnPosition;
                        //var monster = PeekabooEnemyObjectPool.GetObject(transform);
                        _NPCposition = GetRandomPointOnNavMesh(PeekabooGameManager.Instance.CreateMap.MapData[respawnNPCIndex].MapPosition);
                        _NPCposition += Vector3.up * 15f;   
                        break;
                    }
                }

            }
        }
    }

    private Vector3 GetRandomPointOnNavMesh(Vector3 _center)
    {
        float randomPositionX = Random.Range(_center.x - (PeekabooGameManager.Instance.CreateMap.MapLength / 2), _center.x + (PeekabooGameManager.Instance.CreateMap.MapLength / 2));
        float randomPositionZ = Random.Range(_center.z - (PeekabooGameManager.Instance.CreateMap.MapLength / 2), _center.z + (PeekabooGameManager.Instance.CreateMap.MapLength / 2));
        Vector3 randomPosition = new Vector3(randomPositionX, _center.y, randomPositionZ);

        NavMeshHit hit;

        NavMesh.SamplePosition(randomPosition, out hit, 5f, NavMesh.AllAreas);


        return hit.position;
    }


}
