using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.EventSystems;
using UnityEngine.Animations.Rigging;

public class FFF_Player : MonoBehaviour
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
        switch (FFF_GameManager.Instance.flow)
        {
            case 0:
                SetRay(20, true);
                break;
            case 1:
                // 오른손 내미는 애니메이션
                SetRay(10f, true);
                Get3DRayCastHit();
                break;
            case 2:
                // 양손 내미는 애니메이션
                SetRay(0.2f, false);
                Get3DRayCastHit();
                break;
            case 3:
                GetUIRayCastHit();
                break;
        }
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
            GrabHand(leftRayHit, leftHand.transform);
        }
        if (rightRayInteractor.TryGetCurrent3DRaycastHit(out rightRayHit))
        {
            GrabHand(rightRayHit, rightHand.transform);
        }
    }

    public void GrabHand(RaycastHit _raycastHit, Transform _transform)
    {
        if (_raycastHit.transform.gameObject.tag == "FFF_LeftHand")
        {
            _raycastHit.transform.gameObject.GetComponentInParent<RigBuilder>().layers[0].rig.GetComponentInChildren<SetTarget>().SetTransform(_transform);
        }
        if (_raycastHit.transform.gameObject.tag == "FFF_RightHand")
        {
            _raycastHit.transform.gameObject.GetComponentInParent<RigBuilder>().layers[1].rig.GetComponentInChildren<SetTarget>().SetTransform(_transform);
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
