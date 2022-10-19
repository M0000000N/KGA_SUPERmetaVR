using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTester : MonoBehaviour
{
    public List<GameObject> NPCs;

    private void Awake()
    {
        NPCs = new List<GameObject>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            foreach (GameObject NPC in NPCs)
            {
                PeekabooNPC npc = NPC.GetComponent<PeekabooNPC>();
                IDamageable target = npc.GetComponent<IDamageable>();
                target.TakeDamage(gameObject);
            }
        }
    }
}