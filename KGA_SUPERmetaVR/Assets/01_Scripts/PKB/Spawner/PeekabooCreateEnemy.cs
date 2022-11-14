using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PeekabooCreateEnemy : MonoBehaviour
{
    [SerializeField]
    private GameObject NPCprefab;

    public GameObject GetObject(Transform _transform)
    {
        GameObject newObject = PhotonNetwork.Instantiate(NPCprefab.name, _transform.position, Quaternion.identity);
        return newObject;
    }
}
