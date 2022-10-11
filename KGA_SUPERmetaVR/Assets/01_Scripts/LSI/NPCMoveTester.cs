using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMoveTester : MonoBehaviour
{
    [SerializeField]
    private int numberOfNPC;

    private PeekabooNPCMove[] NPCs;
    private bool inputLeftMouseButton = false;

    void Awake()
    {
        NPCs = GetComponentsInChildren<PeekabooNPCMove>();
    }

    void Update()
    {
        inputLeftMouseButton = Input.GetKeyDown(KeyCode.Mouse0);

        if (inputLeftMouseButton)
        {
            for (int i = 0; i < numberOfNPC; i++)
            {
                NPCs[i].Move();
            }
        }
    }
}