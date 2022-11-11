using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using Newtonsoft.Json.Bson;

public class RestrictedRangeGround : MonoBehaviour
{
    ActionBasedContinuousMoveProvider stickMove;

    private void Start()
    {
        stickMove = GameObject.Find("Lobby Camera(Clone)").GetComponent<ActionBasedContinuousMoveProvider>();
        stickMove.GetComponent<ActionBasedContinuousMoveProvider>().enabled = false; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            stickMove.GetComponent<ActionBasedContinuousMoveProvider>().enabled = true; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            stickMove.GetComponent<ActionBasedContinuousMoveProvider>().enabled = false;
        }
    }
}
