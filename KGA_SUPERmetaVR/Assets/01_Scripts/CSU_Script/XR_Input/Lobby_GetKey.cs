using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class Lobby_GetKey : MonoBehaviour
{
    [SerializeField]
    public XRNode XrNode = XRNode.RightHand;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;
    private bool wasPressed;

    [SerializeField]
    private GameObject rightRay; 


    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(XrNode, devices);
        device = devices.FirstOrDefault();
    }

    void OnEnable()
    {
        if (!device.isValid)
        {
            GetDevice();
        }
    }

    private void Start()
    {
        rightRay.SetActive(false);
    }

    private void Update()
    {
        bool isPressedNow = false;
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out isPressedNow) && isPressedNow && !wasPressed)
        {
           rightRay.SetActive(true);
           // wasPressed = true;
        }
        if (!isPressedNow && wasPressed)
        {
            rightRay.SetActive(false);
            wasPressed = isPressedNow;
        }

    }



}
