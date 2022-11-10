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

    private bool isGrabItem;
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
        isGrabItem = false;
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
            if (!isGrabItem)
            {
                GrabHand(leftRayHit);
            }
            else
            {
                GrabOutHnad(leftRayHit);
            }
        }
    }

    public void HoverGet3DRayCastHit()
    {
        if (leftRayInteractor.TryGetCurrent3DRaycastHit(out leftRayHit) && isGrabItem == false)
        {
            HoverGrabHand(leftRayHit);
        }
    }

    public void HoverGrabHand(RaycastHit _raycastHit)
    {
        Debug.Log("ȣ����Ҵ�");

        if (_raycastHit.transform.gameObject.tag == "DeleteItem")
        {
            
            Debug.Log("����Ŭ�ι��Դϴ�");
        }
        else if (_raycastHit.transform.gameObject.tag == "LuckyItem")
        {
            Debug.Log("����Ŭ�ι��Դϴ�");
        }
    }

    IEnumerator DeleteItem(GameObject _item)
    {
        yield return new WaitForSeconds(2f);
        Destroy(_item);
        isGrabItem = true;
    }

    public void GrabHand(RaycastHit _raycastHit)
    {
        Debug.Log("��Ҵ�");
        
        if (_raycastHit.transform.gameObject.tag == "Item")
        {
            StartCoroutine(DeleteItem((_raycastHit.transform.gameObject)));
            Debug.Log("����Ŭ�ι��Դϴ� ��~~~~~~~~~~~~~~~~~~~~~~~~~");
        }
        else if (_raycastHit.transform.gameObject.tag == "LuckyItem")
        {
            isGrabItem = true;
            Debug.Log("����Ŭ�ι��Դϴ� ���� �κ��丮�� ��������");
            _raycastHit.transform.gameObject.tag = "Item";
        }
    }

    public void GrabOutHnad(RaycastHit _raycastHit)
    {
        //Debug.Log("���Ҵ�");
        //StartCoroutine(ChangeTag(_raycastHit.transform.gameObject));
        //    leftLineRenderer.enabled = true;
        //isGrabItem = false;
        if (_raycastHit.transform.gameObject.tag == "DeleteItem")
        {
            Destroy(_raycastHit.transform.gameObject);
            Debug.Log("����Ŭ�ι��� �����˴ϴ�~");
        }
        else if (_raycastHit.transform.gameObject.tag == "Item")
        {
            Destroy(_raycastHit.transform.gameObject);
            Debug.Log("����Ŭ�ι��� �����˴ϴ� �Ф�");
        }
        isGrabItem = false;
    }

    IEnumerator ChangeTag(GameObject _item)
    {
        yield return new WaitForSeconds(3f);
        _item.tag = "Item";
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