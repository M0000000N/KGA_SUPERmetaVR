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
                SetRay(0.2f, false);
                Get3DRayCastHit();
                // 한손 내미는 애니메이션
                break;
            case 2:
                SetRay(0.2f, false);
                Get3DRayCastHit();
                // 양손 내미는 애니메이션
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
        if (rightRayInteractor.TryGetCurrent3DRaycastHit(out rightRayHit))
        {
            // NPC 손이랑 부딪혔을 때
            if(rightRayHit.transform.gameObject.tag == "NPCHand")
            {
                // rightRayHit.gameObject.GetComponent<TwoBoneIKConstraintData>().target = righttHand.transform;
            }
        }
        if (leftRayInteractor.TryGetCurrent3DRaycastHit(out leftRayHit))
        {
            // NPC 손이랑 부딪혔을 때
            if (leftRayHit.transform.gameObject.tag == "NPCHand")
            {
                // leftRayHit.gameObject.GetComponent<TwoBoneIKConstraintData>().target = lefttHand.transform;
            }
        }
    }

    public void GetUIRayCastHit()
    {
        if (rightRayInteractor.TryGetCurrentUIRaycastResult(out rightRayResult))
        {
            if (rightRayResult.gameObject.GetComponent<Button>().interactable == true)
            {
                rightRayResult.gameObject.GetComponent<Button>().onClick.Invoke();
                rightRayResult.gameObject.GetComponent<Button>().interactable = false;
            }
        }
        if (leftRayInteractor.TryGetCurrentUIRaycastResult(out leftRayResult))
        {
            if (leftRayResult.gameObject.GetComponent<Button>().interactable == true)
            {
                leftRayResult.gameObject.GetComponent<Button>().onClick.Invoke();
                leftRayResult.gameObject.GetComponent<Button>().interactable = false;
            }
        }
    }
}
