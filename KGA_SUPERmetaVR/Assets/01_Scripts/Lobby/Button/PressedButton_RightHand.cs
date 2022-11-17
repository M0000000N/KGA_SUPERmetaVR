using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PressedButton_RightHand : MonoBehaviour
{
    [Header("Right Hand Button")]

    [SerializeField] InputActionProperty B_Button;
    [SerializeField] GameObject Inventory;

    bool isActive; 

    private void Start()
    {
        B_Button.action.Enable();
        Inventory.SetActive(false);
    }

    private void Update()
    {
        if (!isActive) return;

        if(isActive.Equals(true) && B_Button.action.IsPressed())
        {
            Inventory.SetActive(true);
            isActive = false;
        }

    }

}
