using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustum : MonoBehaviour
{
    private GameObject[] playerCustumList;
    private PhotonView photonView;

    private void Start()
    {
        playerCustumList = new GameObject[2];
        int count = 0;
        foreach(Transform child in transform)
        {
            playerCustumList[count] = child.gameObject;
            count++;
        }
        ItemManager.Instance.PlayerCustumList = playerCustumList;
    }

    public void ChangeCustum (int _IDnumber)
    {
        photonView.RPC("RPCChangeCustum", RpcTarget.All, _IDnumber);
    }

    [PunRPC]
    private void RPCChangeCustum(int _IDnumber)
    {
        for (int i = 0; i < ItemManager.Instance.PlayerCustumList.Length; i++)
        {

            ItemManager.Instance.PlayerCustumList[i].SetActive(false);
        }
        for (int i = 0; i < ItemManager.Instance.PlayerCustumList.Length; i++)
        {
            Item item = ItemManager.Instance.PlayerCustumList[i].GetComponent<Item>();
            if (item.ItemID == _IDnumber)
            {
                ItemManager.Instance.PlayerCustumList[i].SetActive(true);
                break;
            }
        }
    }
}
