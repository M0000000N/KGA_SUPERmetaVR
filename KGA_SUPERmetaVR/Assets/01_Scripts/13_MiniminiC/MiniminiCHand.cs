using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniminiCHand : MonoBehaviour
{
    public bool CanPick;

    void OnTriggerEnter(Collider other)
    {
        CanPick = true;
    }

    void OnTriggerExit(Collider other)
    {
        CanPick = false;
    }
}
