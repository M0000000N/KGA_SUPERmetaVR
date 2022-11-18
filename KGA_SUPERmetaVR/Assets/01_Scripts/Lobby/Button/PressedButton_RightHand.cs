using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PressedButton_RightHand : MonoBehaviour
{

    public bool TurnOn = true;
    public InputActionReference InventoryReference;
    public GameObject Inventory;

    private void Start()
    {
        InventoryReference.action.started += DoPressedThing;    
    }

    private void OnEnable()
    {
        InventoryReference.asset.Enable();
    }

    private void OnDisable()
    {
        InventoryReference.asset.Disable();
    }

    private void OnDestroy()
    {
        InventoryReference.action.started -= DoPressedThing;
    }

    private void DoPressedThing(InputAction.CallbackContext context)
    {
        Inventory.SetActive(TurnOn);
        TurnOn = !TurnOn;
    }

}
