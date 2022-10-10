using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMoveTester : MonoBehaviour
{
    [SerializeField]
    private int numberOfNPC;

    private PeekabooNPCMove[] NPCs;
    private bool inputMouseLeftButton = false;

    void Awake()
    {
        NPCs = GetComponentsInChildren<PeekabooNPCMove>();
    }

    void Update()
    {
        inputMouseLeftButton = Input.GetKeyDown(KeyCode.Mouse0);

        if (inputMouseLeftButton)
        {
            for (int i = 0; i < numberOfNPC; i++)
            {
                NPCs[i].Move();
            }
        }
    }
}