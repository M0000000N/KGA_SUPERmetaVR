using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PeekabooNPCMove : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    #region ���߿� CSV ����ؼ� ������ �������������� �ڵ� �����ϸ� ������ ����
    [SerializeField]
    private List<float> mapData;
    /*
     * 0 -> �� ���� ũ��
     * 1 -> �� ���� ũ��
     * 2 -> �� ����
    */
    #endregion

    private NavMeshAgent agent;
    public float moveRangeMinX;
    public float moveRangeMaxX;
    public float moveRangeMinY;
    public float moveRangeMaxY;
    public float moveRangeMinZ;
    public float moveRangeMaxZ;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        moveRangeMinX = -(mapData[0] / 2);
        moveRangeMaxX = mapData[0] / 2;
        moveRangeMinY = -(mapData[1] / 2);
        moveRangeMaxY = mapData[1] / 2;
        moveRangeMinZ = -(mapData[2] / 2);
        moveRangeMaxZ = mapData[2] / 2;
    }

    public void Move()
    {
        Vector3 des = SetNextDestination();
        agent.destination = des;
    }

    private Vector3 SetNextDestination()
    {
        float newDestinationX = Random.Range(moveRangeMinX, moveRangeMaxX);
        float newDestinationY = Random.Range(moveRangeMinY, moveRangeMaxY);
        float newDestinationZ = Random.Range(moveRangeMinZ, moveRangeMaxZ);
        Vector3 newDestination = new Vector3(newDestinationX, newDestinationY, newDestinationZ);

        return newDestination;
    }
}