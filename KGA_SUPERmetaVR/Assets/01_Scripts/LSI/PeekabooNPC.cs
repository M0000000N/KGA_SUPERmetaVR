using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooNPC : MonoBehaviour
{
    private PeekabooNPCMove npcMove;

    void Awake()
    {
        npcMove = GetComponent<PeekabooNPCMove>();
    }
}