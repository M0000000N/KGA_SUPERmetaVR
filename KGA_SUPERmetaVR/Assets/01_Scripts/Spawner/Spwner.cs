using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spwner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private int NPCcount;

    public int NPCCount { get { return NPCcount; } }
    [SerializeField]
    private float MaxDistance = 5f;


    private NavMeshAgent _navMeshAgent;
    private float _lastSpawnTime = 0f;

    private void Start()
    {
        //_navMeshAgent = GetComponentInChildren<NavMeshAgent>();

        //for (int i = 0; i < NPCCount; i++)
        //{
        //    spawn();
        //}
    } 

    private void Update()
    {
        //if (Time.time >= _lastSpawnTime + SpawnCooltime)
        //{
        //    _lastSpawnTime = Time.time;
        //    spawn();
        //}
    }
    public void firstSpawn(Vector3 _mapPosition)
    {
        _navMeshAgent = GetComponentInChildren<NavMeshAgent>();

        for (int i = 0; i < NPCCount; i++)
        {
            spawn(_mapPosition);
        }
    }

    void spawn(Vector3 _mapPosition)
    {
        
        Vector3 spawnPosition = GetRandomPointOnNavMesh(_mapPosition, MaxDistance);

        spawnPosition += Vector3.up * 1f;
        transform.position = spawnPosition;

        //if (Vector3.Distance(transform.position, transform.position) > MaxDistance)
        //{
        //    return;
        //}
        //else
        //{
        Debug.Log("¸¸µé¾îÁü");
            var monster = EnemyObjectPool.GetObject(transform);
        transform.position = new Vector3(0f, 0f, 0f);
        //
    }

    private Vector3 GetRandomPointOnNavMesh(Vector3 _center, float _maxdistance)
    {
        Vector3 randomPos = Random.insideUnitSphere * _maxdistance + _center;
        NavMeshHit hit;

        NavMesh.SamplePosition(randomPos, out hit, _maxdistance, NavMesh.AllAreas);

        
        return hit.position;
    }


}
