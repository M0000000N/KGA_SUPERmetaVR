using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Unity.XR.Oculus;

public class ButtonController_script : MonoBehaviour
{
    // 버튼은 bool값을 받음 
    static readonly Dictionary<string, InputFeatureUsage<bool>> availableButtons = new Dictionary<string, InputFeatureUsage<bool>>
    {
        {"triggerButton", CommonUsages.triggerButton },
        {"thumbrest", OculusUsages.thumbrest },
        {"primary2DAxisClick", CommonUsages.primary2DAxisClick },
        {"primary2DAxisTouch", CommonUsages.primary2DAxisTouch },
        {"menuButton", CommonUsages.menuButton },
        {"gripButton", CommonUsages.gripButton },
        {"secondaryButton", CommonUsages.secondaryButton },
        {"secondaryTouch", CommonUsages.secondaryTouch },
        {"primaryButton", CommonUsages.primaryButton },
        {"primaryTouch", CommonUsages.primaryTouch },
    };

    public enum ButtonOption
    {
        triggerButton,
        thumbrest,
        primary2DAxisClick,
        primary2DAxisTouch,
        menuButton,
        gripButton,
        secondaryButton,
        secondaryTouch,
        primaryButton,
        primaryTouch
    };

    public InputDeviceCharacteristics deviceCharacteristic;

    public ButtonOption button;

    public UnityEvent OnPress;
    public UnityEvent OnRelease;

    public bool IsPressed { get; private set; }

    List<InputDevice> inputDevices;
    private InputDevice device;
    bool inputValue;
    bool cooldown = false;
    InputFeatureUsage<bool> inputFeature;

    void Awake()
    {
        string featureLabel = Enum.GetName(typeof(ButtonOption), button);
        availableButtons.TryGetValue(featureLabel, out inputFeature);
        inputDevices = new List<InputDevice>();
    }
    
    void Update()
    {
        InputDevices.GetDevicesWithCharacteristics(deviceCharacteristic, inputDevices);

        for (int i = 0; i < inputDevices.Count; i++)
        {
            if (inputDevices[i].TryGetFeatureValue(inputFeature,
                out inputValue) && inputValue)
            {
             
                if (!IsPressed)
                {
                    IsPressed = true;
                    OnPress.Invoke();
                }
            }

            else if (IsPressed)
            {
                IsPressed = false;
                OnRelease.Invoke();
                
            }
            
        }
    }
}
   