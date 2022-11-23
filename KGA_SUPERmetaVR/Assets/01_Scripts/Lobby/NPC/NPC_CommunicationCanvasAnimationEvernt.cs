using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_CommunicationCanvasAnimationEvernt : MonoBehaviour
{
    [SerializeField] private NPC_CommunicationManager communicationManager;

    private void Start()
    {
        if (communicationManager == null)
        {
            communicationManager = transform.GetComponentInParent<NPC_CommunicationManager>();
        }
    }

    public void PlayAnimationEvent()
    {
        communicationManager.SetDialogue();
    }
}
