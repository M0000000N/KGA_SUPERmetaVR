using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooNPC : MonoBehaviour
{
    private PeekabooNPCMove npcMove;

    public GameObject lookingTarget;
    public bool isLooking;

    void Awake()
    {
        npcMove = GetComponent<PeekabooNPCMove>();
        lookingTarget = null;
        isLooking = false;
    }

    void Update()
    {
        if (isLooking)
        {
            transform.LookAt(lookingTarget.transform);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isLooking)
        {
            return;
        }

        lookingTarget = other.gameObject;
        isLooking = true;
    }

    void OnTriggerExit(Collider other)
    {
        lookingTarget = null;
        isLooking = false;
    }
}