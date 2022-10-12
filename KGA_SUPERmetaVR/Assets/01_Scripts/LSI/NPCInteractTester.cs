using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractTester : MonoBehaviour
{
    [SerializeField]
    private PeekabooNPC NPC;
    [SerializeField]
    private GameObject Attacker;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            NPC.TakeDamage(Attacker);
        }
    }
}