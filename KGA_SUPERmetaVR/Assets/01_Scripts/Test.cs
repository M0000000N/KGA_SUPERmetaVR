using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Test : MonoBehaviourPun
{
    [SerializeField]
    private PeekabooNPC myNPC;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("TestMethod", RpcTarget.All);
            }
            else
            {
                photonView.RPC("TestMethod", RpcTarget.MasterClient);
            }
            
        }
    }

    [PunRPC]
    private void TestMethod()
    {
        myNPC.TakeDamage(gameObject);
    }
}