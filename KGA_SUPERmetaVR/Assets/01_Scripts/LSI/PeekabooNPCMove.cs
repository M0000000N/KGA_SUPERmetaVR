using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PeekabooNPCMove : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float minDistance;
    [SerializeField]
    private float maxDistance;

    private NavMeshAgent myAgent;

    private void Awake()
    {
        myAgent.GetComponent<NavMeshAgent>();
    }

    public void SetNextDestination()
    {
        float distance = Random.Range(minDistance, maxDistance);
        Vector3 nextDestination = Random.insideUnitSphere * distance;
        myAgent.destination = nextDestination;
    }
}