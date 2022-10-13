using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTester : MonoBehaviour
{
    [SerializeField]
    private GameObject attacker;
    [SerializeField]
    private PeekabooNPC NPC;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            NPC.TakeDamage(attacker);
        }
    }
}