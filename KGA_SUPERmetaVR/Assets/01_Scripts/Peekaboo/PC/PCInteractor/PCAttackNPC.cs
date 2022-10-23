using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PCAttackNPC : MonoBehaviour
{
    [SerializeField]
    private PeekabooPC PC;
    [SerializeField]
    private PeekabooNPC NPC;

    [SerializeField]
    private PCAppearPeekaboo peekaboo;

    private void Update()
    {
        // �������� ���ϰ� ����� 
        peekaboo.ShowPeekaboo();
       // PCAttackNPCs();
    }

    public void PCAttackNPCs()
    {
       // PC.TakeDamage(NPC.gameObject);
        NPC.TakeDamage(PC.gameObject);
    }

}
