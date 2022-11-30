using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PressedButton_SoundSetting : MonoBehaviour
{
    public bool TurnOn = true;
    public InputActionReference SoundSettingBtn;
    public GameObject SoundSetting;

    private void Start()
    {
        SoundSettingBtn.action.started += DoPressedThing;
    }

    private void OnEnable()
    {
        SoundSettingBtn.asset.Enable();
    }

    private void OnDisable()
    {
        SoundSettingBtn.asset.Disable();
    }

    private void OnDestroy()
    {
        SoundSettingBtn.action.started -= DoPressedThing;
    }

    private void DoPressedThing(InputAction.CallbackContext context)
    {
        SoundSetting.SetActive(TurnOn);
        TurnOn = !TurnOn;
    }

}