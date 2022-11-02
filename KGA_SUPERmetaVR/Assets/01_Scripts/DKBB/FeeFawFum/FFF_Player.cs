using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.EventSystems;

public class FFF_Player : MonoBehaviour
{
    [SerializeField] GameObject leftHand;
    private XRRayInteractor leftRayInteractor;
    private XRController leftXRController;
    private LineRenderer leftLineRenderer;
    private RaycastResult leftRaycastResult;

    [SerializeField] GameObject rightHand;
    private XRRayInteractor rightRayInteractor;
    private XRController rightXRController;
    private LineRenderer rightLineRenderer;
    private RaycastResult rightRaycastResult;

    private GameObject targetObject;

    void Start()
    {
        leftRayInteractor = leftHand.GetComponent<XRRayInteractor>();
        leftXRController = leftHand.GetComponent<XRController>();
        leftLineRenderer = leftHand.GetComponent<LineRenderer>();

        rightRayInteractor = rightHand.GetComponent<XRRayInteractor>();
        rightXRController = rightHand.GetComponent<XRController>();
        rightLineRenderer = rightHand.GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (FFF_GameManager.Instance.isReady)
        {
            SetRay(0.2f, false);
            RayCastHit();
        }
        else
        {
            SetRay(20, true);
        }
    }

    private void SetRay(float _maxRaycastDistance, bool _useWorldSpace)
    {
        leftRayInteractor.maxRaycastDistance = _maxRaycastDistance;
        rightRayInteractor.maxRaycastDistance = _maxRaycastDistance;
        leftLineRenderer.useWorldSpace = _useWorldSpace;
        rightLineRenderer.useWorldSpace = _useWorldSpace;
    }

    public void RayCastHit()
    {
        if (rightRayInteractor.TryGetCurrentUIRaycastResult(out rightRaycastResult))
        {
            if (rightRaycastResult.gameObject.GetComponent<Button>().interactable == true)
            {
                rightRaycastResult.gameObject.GetComponent<Button>().onClick.Invoke();
                rightRaycastResult.gameObject.GetComponent<Button>().interactable = false;
            }
        }
        else if (leftRayInteractor.TryGetCurrentUIRaycastResult(out leftRaycastResult))
        {
            if (leftRaycastResult.gameObject.GetComponent<Button>().interactable == true)
            {
                leftRaycastResult.gameObject.GetComponent<Button>().onClick.Invoke();
                leftRaycastResult.gameObject.GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            return;
        }
    }
}
