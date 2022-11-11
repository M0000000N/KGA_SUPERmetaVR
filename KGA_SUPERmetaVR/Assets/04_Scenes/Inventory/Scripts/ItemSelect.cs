using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ItemSelect : MonoBehaviour
{
    [SerializeField] GameObject leftHand;
    private XRRayInteractor leftRayInteractor;
    private GameObject targetObject;
    private GameObject grabObject;

    [SerializeField] private InputActionProperty isGrap;

    private bool isDestroyCloverRun = false;
    private bool isGrabRun = false;

    void Start()
    {
        leftRayInteractor = leftHand.GetComponent<XRRayInteractor>();
    }

    private void Update()
    {
        if (isGrap.action.IsPressed() && isGrabRun == false)
        {
            Grab();
        }
        else if (isGrap.action.IsPressed() == false && isGrabRun)
        {
            GrabOut();
        }
    }

    public void HoverGet3DRayCastHit()
    {
        //raycastall vs raycast 
        if (leftRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit _leftRayHit))
        {
            if (isGrabRun == false)
            {
                string targetTag = _leftRayHit.transform.gameObject.tag;

                if (targetTag == "ThreeLeafClover" || targetTag == "FourLeafClover")
                {
                    targetObject = _leftRayHit.transform.gameObject;
                }
                else
                {
                    targetObject = null;
                }
            }
        }
        else
        {
            targetObject = null;
        }
    }

    private void Grab()
    {
        isGrabRun = true;
        string targetTag = string.Empty;
        if (targetObject != null)
        {
            grabObject = targetObject;
            targetTag = grabObject.tag;
        }
        if (targetTag.Equals("ThreeLeafClover")) // 세잎클로버면
        {
            if (isDestroyCloverRun == false)
            {
                StartCoroutine(DestroyObject());
            }
        }
        else if (targetTag.Equals("FourLeafClover")) // 네잎클로버면
        {
            //  TODO : 멋진 파티클효과
        }
    }

    private void GrabOut()
    {
        isGrabRun = false;
        if (grabObject == null) return;
        CloverInfo targetCloverInfo = grabObject.GetComponent<CloverInfo>();
        if (targetCloverInfo == null) return;

        if (targetCloverInfo.IsStartFadedout == false)
        {
            leftRayInteractor.enableInteractions = false;
            StartCoroutine(Activeinteractor());

            StopCoroutine(DestroyObject());
            targetCloverInfo.IsStartFadedout = true; 
            isDestroyCloverRun = false;
            targetObject = null;
            grabObject = null;
        }
    }

    private IEnumerator DestroyObject()
    {
        if (grabObject == null) yield break;

        isDestroyCloverRun = true;
        CloverInfo targetCloverInfo = grabObject.GetComponent<CloverInfo>();
        yield return new WaitForSeconds(2f);
        if (targetCloverInfo.IsStartFadedout == false)
        {
            StartCoroutine(Activeinteractor());

            targetCloverInfo.IsStartFadedout = true;
            targetObject = null;
            grabObject = null;
        }
        isDestroyCloverRun = false;
    }

    IEnumerator Activeinteractor()
    {
        yield return new WaitForSeconds(2f);
        leftRayInteractor.enableInteractions = true;

    }

    IEnumerator ChangeTag(GameObject _item)
    {
        yield return new WaitForSeconds(3f);
        _item.tag = "Item";
    }
}