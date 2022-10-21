using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PeekabooNPCMove : MonoBehaviour
{
    public NavMeshAgent myAgent { get; private set; }

    [SerializeField]
    private PeekabooNPC myNPC;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float minDistance;
    [SerializeField]
    private float maxDistance;

    private void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();

        myAgent.speed = moveSpeed;
        SetNextDestination();
    }

    public bool CheckArrival(Vector3 _myPosition)
    {
        float remainingDistance = (myAgent.destination - _myPosition).sqrMagnitude;
        if (remainingDistance <= 0.01f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetNextDestination()
    {
        float distance = Random.Range(minDistance, maxDistance);
        Vector3 nextDestination = transform.position + Random.insideUnitSphere * distance;
        myAgent.destination = nextDestination;
    }
}