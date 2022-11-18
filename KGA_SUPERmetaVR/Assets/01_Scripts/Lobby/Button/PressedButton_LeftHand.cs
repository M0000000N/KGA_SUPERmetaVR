using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class PressedButton_LeftHand : MonoBehaviour
{
    public bool TurnOn = true;
    public InputActionReference FriendList;
    public GameObject FreindList;

    private void Start()
    {
        //FreindList.SetActive(false);
        FriendList.action.started += DoPressedThing;
    }

    private void OnEnable()
    {
        FriendList.asset.Enable();
    }

    private void OnDisable()
    {
        FriendList.asset.Disable();
    }

    private void OnDestroy()
    {
        FriendList.action.started -= DoPressedThing;
    }

    private void DoPressedThing(InputAction.CallbackContext context)
    { 
        FreindList.SetActive(TurnOn);
        TurnOn = !TurnOn;
    }

}