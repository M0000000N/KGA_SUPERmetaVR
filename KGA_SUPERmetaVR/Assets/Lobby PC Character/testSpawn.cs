using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class testSpawn : MonoBehaviourPun
{
    private void Awake()
    {
        Vector3 position = new Vector3(0f, 0f, 0f);
        PhotonNetwork.Instantiate("TestPlayer", position, Quaternion.identity);
    }
}