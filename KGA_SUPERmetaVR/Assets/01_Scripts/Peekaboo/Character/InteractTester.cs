using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTester : MonoBehaviour
{
    [SerializeField]
    private PeekabooPC PC;
    [SerializeField]
    private PeekabooNPC NPC;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PC.TakeDamage(NPC.gameObject);
            NPC.TakeDamage(PC.gameObject);
        }
    }
}