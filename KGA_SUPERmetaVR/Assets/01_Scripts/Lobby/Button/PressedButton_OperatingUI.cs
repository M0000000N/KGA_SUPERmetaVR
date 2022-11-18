using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

// Y¹öÆ°
public class PressedButton_OperatingUI : MonoBehaviour
{

    public bool TurnOn = true;
    public InputActionReference OperatingReference;
    public GameObject Operating;

    private void Start()
    {
        Operating.SetActive(false);
        OperatingReference.action.started += DoPressedThing;
    }

    private void OnEnable()
    {
        OperatingReference.asset.Enable();
    }

    private void OnDisable()
    {
        OperatingReference.asset.Disable();
    }

    private void OnDestroy()
    {
        OperatingReference.action.started -= DoPressedThing;
    }

    private void DoPressedThing(InputAction.CallbackContext context)
    {   
            Operating.SetActive(TurnOn);
            TurnOn = !TurnOn;
    }

}