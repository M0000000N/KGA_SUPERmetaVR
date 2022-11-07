using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.EventSystems;
using UnityEngine.Animations.Rigging;

public class ItemSelect : MonoBehaviour
{
    [SerializeField] GameObject leftHand;
    private XRRayInteractor leftRayInteractor;
    private XRController leftXRController;
    private LineRenderer leftLineRenderer;
    private RaycastResult leftRayResult;
    private RaycastHit leftRayHit;

    [SerializeField] GameObject rightHand;
    private XRRayInteractor rightRayInteractor;
    private XRController rightXRController;
    private LineRenderer rightLineRenderer;
    private RaycastResult rightRayResult;
    private RaycastHit rightRayHit;

    private bool isLeftRayCast;
    private bool isRightRayCast;
    void Start()
    {
        leftRayInteractor = leftHand.GetComponent<XRRayInteractor>();
        leftXRController = leftHand.GetComponent<XRController>();
        leftLineRenderer = leftHand.GetComponent<LineRenderer>();

        rightRayInteractor = rightHand.GetComponent<XRRayInteractor>();
        rightXRController = rightHand.GetComponent<XRController>();
        rightLineRenderer = rightHand.GetComponent<LineRenderer>();

        isLeftRayCast = false;
        isRightRayCast = false;
    }

    void Update()
    {

       // Get3DRayCastHit();

    }

    private void SetRay(float _maxRaycastDistance, bool _useWorldSpace)
    {
        leftRayInteractor.maxRaycastDistance = _maxRaycastDistance;
        rightRayInteractor.maxRaycastDistance = _maxRaycastDistance;
        leftLineRenderer.useWorldSpace = _useWorldSpace;
        rightLineRenderer.useWorldSpace = _useWorldSpace;
    }

    public void Get3DRayCastHit()
    {
        if (leftRayInteractor.TryGetCurrent3DRaycastHit(out leftRayHit))
        {
            isLeftRayCast = true;
            GrabHand(leftRayHit);
        }
        if (rightRayInteractor.TryGetCurrent3DRaycastHit(out rightRayHit))
        {
            isRightRayCast = true;
            GrabHand(rightRayHit);
        }
    }

    public void HoverGet3DRayCastHit()
    {
        if (leftRayInteractor.TryGetCurrent3DRaycastHit(out leftRayHit))
        {
            HoverGrabHand(leftRayHit);
        }
        if (rightRayInteractor.TryGetCurrent3DRaycastHit(out rightRayHit))
        {
            HoverGrabHand(rightRayHit);
        }
    }

    public void HoverGrabHand(RaycastHit _raycastHit)
    {
        Debug.Log("호버잡았다");
        
        if (_raycastHit.transform.gameObject.tag == "GrabItem")
        {
            _raycastHit.transform.gameObject.tag = "Item";
        }
    }

    public void GrabHand(RaycastHit _raycastHit)
    {
        Debug.Log("잡았다");
        if (_raycastHit.transform.gameObject.tag == "Item")
        {
            if (isLeftRayCast)
            {
                leftLineRenderer.enabled = false;
            }
            if (isRightRayCast)
            {
                rightLineRenderer.enabled = false;
            }
            _raycastHit.transform.gameObject.tag = "GrabItem";
        }
    }

    public void GrabOutHnad()
    {
        Debug.Log("놓았다");
        if (isLeftRayCast == false)
        {
            leftLineRenderer.enabled = true;
            isLeftRayCast = true;
        }
        if (isRightRayCast == false)
        {
            rightLineRenderer.enabled = true;
            isRightRayCast = true;
        }
    }

    public void GetUIRayCastHit()
    {
        if (rightRayInteractor.TryGetCurrentUIRaycastResult(out rightRayResult))
        {
            Button rightButton = rightRayResult.gameObject.GetComponent<Button>();
            if (rightButton.interactable == true)
            {
                rightButton.onClick.Invoke();
                rightButton.interactable = false;
            }
        }
        if (leftRayInteractor.TryGetCurrentUIRaycastResult(out leftRayResult))
        {
            Button leftButton = leftRayResult.gameObject.GetComponent<Button>();
            if (leftButton.interactable == true)
            {
                leftButton.onClick.Invoke();
                leftButton.interactable = false;
            }
        }
    }
}