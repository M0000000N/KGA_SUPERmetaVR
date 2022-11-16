using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using Photon.Pun;

public class RestrictedRangeGround : MonoBehaviourPun
{
    [SerializeField]
    ActionBasedContinuousMoveProvider stickMove;

    private void Start()
    {
        stickMove.enabled = false; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine == false) return;

        if(other.gameObject.tag == "Player")
        {
            stickMove.enabled = true; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (photonView.IsMine == false) return;

        if (other.gameObject.tag.Equals("Player"))
        {
            stickMove.enabled = false;
        }
    }
}
