using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun; 

public class TeleportationManager : MonoBehaviourPun
{
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private TeleportationProvider provider;
    private InputAction triggerButton;
    private bool isActive; 

    private void Start()
    {
        rayInteractor.enabled = false; 

        var activate = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Mode Action");
        activate.Enable();
        activate.performed += OnTeleportActivate;

        var cancle = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Mode Action");
        activate.Enable();
        cancle.performed += OnTeleportCancel; 

    }

    private void Update()
    {
        if (isActive == false)
            return;

        // �����̵� ����
        if(rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit) == false)
        {
            rayInteractor.enabled = false;
            isActive = false; 
        }

        TeleportRequest request = new TeleportRequest()
        {
            destinationPosition = hit.point
        };

        provider.QueueTeleportRequest(request);
        
    }

    private void OnTeleportActivate(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = true;
        isActive = true; 
    }

    private void OnTeleportCancel(InputAction.CallbackContext context)
    {
       rayInteractor.enabled = false;
        isActive = false;
    }


}
