using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PeekabooSpwner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private int NPCcount;

    public int NPCCount { get { return NPCcount; } }
    [SerializeField]
    private float maxDistance = 5f;

    private NavMeshAgent navMeshAgent;

    private void Start()
    {

    }

    private void Update()
    {

    }
    public void FirstSpawn(Vector3 _mapPosition)
    {
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        int RandomNPC = Random.Range(0, NPCcount);
        for (int i = 0; i < RandomNPC; i++)
        {
            Spawn(_mapPosition);
        }
    }

    void Spawn(Vector3 _mapPosition)
    {

        Vector3 spawnPosition = GetRandomPointOnNavMesh(_mapPosition, maxDistance);

        spawnPosition += Vector3.up * 1f;
        transform.position = spawnPosition;


        Debug.Log("¸¸µé¾îÁü");
        var monster = PeekabooEnemyObjectPool.GetObject(transform);
        transform.position = new Vector3(0f, 0f, 0f);

    }

    private Vector3 GetRandomPointOnNavMesh(Vector3 _center, float _maxdistance)
    {
        Vector3 randomPos = Random.insideUnitSphere * _maxdistance + _center;
        NavMeshHit hit;

        NavMesh.SamplePosition(randomPos, out hit, _maxdistance, NavMesh.AllAreas);


        return hit.position;
    }


}
