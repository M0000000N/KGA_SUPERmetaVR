using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustum : MonoBehaviourPunCallbacks
{
    private GameObject[] playerCustumList;
    //private PhotonView photonView;

    private void Start()
    {
        playerCustumList = new GameObject[19];
        int count = 0;
        foreach(Transform child in transform)
        {
            playerCustumList[count] = child.gameObject;
            count++;
        }
        //ItemManager.Instance.PlayerCustumList = playerCustumList;
    }

    public void ChangeCustum (int _IDnumber)
    {
        photonView.RPC("RPCChangeCustum", RpcTarget.All, _IDnumber);
    }

    [PunRPC]
    public void RPCChangeCustum(int _IDnumber)
    {
        for (int i = 0; i < playerCustumList.Length; i++)
        {
            playerCustumList[i].SetActive(false);
        }
        for (int i = 0; i < playerCustumList.Length; i++)
        {
            Item item = playerCustumList[i].GetComponent<Item>();
            if (item.ItemID == _IDnumber)
            {
                playerCustumList[i].SetActive(true);
                break;
            }
        }
    }
}
