using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterPositionSync : MonoBehaviourPun
{
    private Transform target;

    private void Awake()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        target = GameObject.Find("Ingame_Camera").transform.GetChild(0).GetChild(0);
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        transform.position = target.position;
    }
}