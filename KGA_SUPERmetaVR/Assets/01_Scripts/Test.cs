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
                photonView.RPC("TestMethod", RpcTarget.All, myNPC.photonView.ViewID);
            }
            else
            {
                int photonViewID = myNPC.gameObject.GetPhotonView().ViewID;
                photonView.RPC("TestMethod", RpcTarget.MasterClient, photonViewID);
            }
            
        }
    }

    [PunRPC]
    private void TestMethod(int _viewID)
    {
        GameObject target = PhotonView.Find(_viewID).gameObject;
        PeekabooCharacter tttarget = target.GetComponent<PeekabooCharacter>();
        tttarget.TakeDamage(gameObject);
    }
}