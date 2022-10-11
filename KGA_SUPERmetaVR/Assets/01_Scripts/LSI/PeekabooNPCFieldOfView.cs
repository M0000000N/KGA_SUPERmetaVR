using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooNPCFieldOfView : MonoBehaviour
{
    [SerializeField]
    private float viewAngle;
    [SerializeField]
    private float viewDistance;

    void Update()
    {
        Vector3 leftBoundary = DirectionFromAngle(-viewAngle / 2);
        Vector3 rightBoundary = DirectionFromAngle(viewAngle / 2);

        Debug.DrawLine(transform.position, transform.position + leftBoundary * viewDistance, Color.red);
        Debug.DrawLine(transform.position, transform.position + rightBoundary * viewDistance, Color.red);
    }

    private Vector3 DirectionFromAngle(float angleY)
    {
        angleY += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleY * Mathf.Deg2Rad), 0f, Mathf.Cos(angleY * Mathf.Deg2Rad));
    }
}