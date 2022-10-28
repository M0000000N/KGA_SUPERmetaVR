using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationController : MonoBehaviour
{
    static private bool teleportIsAcitve = false;
    public enum ControllerType
    {
        RigthHand,
        LeftHand,
    }

    // 트리거 버튼 누르면 순간이동 
    public ControllerType targetController;
    public XRRayInteractor rayInteractor;
    public TeleportationProvider teleportationProvider;
    public InputActionAsset inputAction;
    public InputAction thumbstickInputAction;
    public InputAction teleportActivate;
    public InputAction teleportCancle;

     
    private void Start()
    {
        rayInteractor.enabled = false;

       // teleportActivate = inputAction.FindActionMap("XRI " + targetController.ToString()).FindAction("Teleport Mode Activate");
      //  teleportActivate.Enable();
        //teleportActivate.performed += OnTeleportActivate;

        //teleportCancle = inputAction.FindActionMap("XRI " + targetController.ToString()).FindAction("Teleport Mode Cancel");
        //teleportCancle.Enable();
        //teleportCancle.performed += OnTeleportCancel;

        //// 트리거 버튼으로 바꾸기 
        //thumbstickInputAction = inputAction.FindActionMap("XRI " + targetController.ToString()).FindAction("Move");
        //thumbstickInputAction.Enable();
    }

    private void OnDestroy()
    {
        teleportActivate.performed -= OnTeleportActivate;
        teleportCancle.performed -= OnTeleportCancel;
    }

    private void Update()
    {
        if(!teleportIsAcitve)
        {
            return;
        }    
        if(!rayInteractor.enabled)
        {
            return;
        }
        if(thumbstickInputAction.triggered) 
        {
            return; 
        }
        if(!rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit raycastHit))
        {
            rayInteractor.enabled = false;
            teleportIsAcitve = false;
            return;
        }

        // 레이케스트 위치 받아옴
        TeleportRequest teleportRequest = new TeleportRequest()
        {
            destinationPosition = raycastHit.point,
        };

        teleportationProvider.QueueTeleportRequest(teleportRequest);

        rayInteractor.enabled = false;
        teleportIsAcitve = false;
    }

    private void OnTeleportActivate(InputAction.CallbackContext context)
    {
        if(!teleportIsAcitve)
        {
            rayInteractor.enabled = true;
            teleportIsAcitve = true;
        }
    }

    private void OnTeleportCancel(InputAction.CallbackContext context)
    {
        if(teleportIsAcitve && rayInteractor.enabled == true)
        {
            rayInteractor.enabled = true;
            teleportIsAcitve = false; 
        }
    }
}
