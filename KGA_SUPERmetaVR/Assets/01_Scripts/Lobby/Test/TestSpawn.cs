using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestSpawn : MonoBehaviourPun
{
    private void Awake()
    {
        Vector3 position = new Vector3(0f, 0f, 0f);
        GameObject spawnObject = PhotonNetwork.Instantiate("������ �Թ�����", position, Quaternion.identity);
    }
}