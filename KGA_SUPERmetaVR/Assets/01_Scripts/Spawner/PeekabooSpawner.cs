using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PeekabooSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private int NPCcount;

    public int NPCCount { get { return NPCcount; } }

    private NavMeshAgent navMeshAgent;


    private void Start()
    {

    }

    private void Update()
    {

    }
    public void FirstSpawn(Vector3 _mapPosition, int _randomSpawnNPC)
    {
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        //int RandomNPC = Random.Range(0, NPCcount);
        for (int i = 0; i < _randomSpawnNPC; i++)
        {
            Spawn(_mapPosition);
        }
    }

    private void Spawn(Vector3 _mapPosition)
    {
        Vector3 spawnPosition = GetRandomPointOnNavMesh(_mapPosition);
        //Debug.Log($"{spawnPosition}");
        spawnPosition += Vector3.up * 1f;
        transform.position = spawnPosition;


        //Debug.Log("¸¸µé¾îÁü");
        var monster = PeekabooEnemyObjectPool.GetObject(transform);
        transform.position = new Vector3(0f, 0f, 0f);

    }

    private Vector3 GetRandomPointOnNavMesh(Vector3 _center)
    {
        float randomPositionX = Random.Range(_center.x - (GameManager.Instance.CreateMap.MapLength / 2) + 1, _center.x + (GameManager.Instance.CreateMap.MapLength /2) - 1);
        float randomPositionZ = Random.Range(_center.z - (GameManager.Instance.CreateMap.MapLength / 2) + 1, _center.z + (GameManager.Instance.CreateMap.MapLength /2) - 1);
        Vector3 randomPosition = new Vector3(randomPositionX, _center.y, randomPositionZ);

        NavMeshHit hit;

        NavMesh.SamplePosition(randomPosition, out hit, 1f, NavMesh.AllAreas);


        return hit.position;
    }


}
