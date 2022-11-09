using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField]
    private float handSpeed;
    [SerializeField]
    private float checkTime;

    private Vector3 recentPosition;
    private float elapsedTime;

    private void Awake()
    {
        recentPosition = transform.position;
        elapsedTime = 0f;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (checkTime <= elapsedTime)
        {
            handSpeed = (transform.position - recentPosition).sqrMagnitude;
            recentPosition = transform.position;
            elapsedTime = 0f;
        }
    }
}