using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooNPCFieldOfView : MonoBehaviour
{
    [SerializeField]
    private float viewAngle;

    private float viewAngleHalf;

    private void Awake()
    {
        viewAngleHalf = viewAngle / 2;
    }

    public bool CheckView(Vector3 _targetPosition)
    {
        Vector3 direction = _targetPosition - transform.position;
        float angleToTarget = Vector3.Angle(transform.forward, direction);
        if (angleToTarget <= viewAngleHalf)
        {
            return true;
        }

        return false;
    }
}