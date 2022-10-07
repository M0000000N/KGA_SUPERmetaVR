using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMoveTester : MonoBehaviour
{
    [SerializeField]
    private PeekabooNPCMove move;

    private bool inputMouseLeftButton = false;

    void Update()
    {
        inputMouseLeftButton = Input.GetKeyDown(KeyCode.Mouse0);

        if (inputMouseLeftButton)
        {
            move.Move();
        }
    }
}