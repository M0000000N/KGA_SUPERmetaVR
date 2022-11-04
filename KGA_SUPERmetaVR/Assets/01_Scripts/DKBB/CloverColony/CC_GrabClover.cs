using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using InputDevice = UnityEngine.XR.InputDevice;
using CommonUsages = UnityEngine.XR.CommonUsages;

public class CC_GrabClover : MonoBehaviour
{
    [SerializeField] GameObject leftHand;
    private XRRayInteractor leftRayInteractor;
    private RaycastHit leftRayHit;

    [SerializeField] GameObject rightHand;
    private XRRayInteractor rightRayInteractor;
    private RaycastHit RightRayHit;
    private GameObject targetObject;

    private bool isGrab;

    private XRHandController xRLefttHand;
    private XRHandController xRRightHand;

    private InputDevice inputDevice;

    void Start()
    {
        isGrab = false;

        xRLefttHand = leftHand.GetComponentInChildren<XRHandController>();
        xRRightHand = rightHand.GetComponentInChildren<XRHandController>();
    }

    void Update()
    {
        GetTriggerValue("ThreeLeafClover", 2f);
        GetTriggerValue("FourLeafClover", 0f);
    }

    public void GetTriggerValue(string _tag, float _time)
    {
        inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out isGrab);
        if (isGrab)
        {
            if (RayCastHit()) // hit 정보를 받아옴
            {
                string targetTag = targetObject.tag;
                if (targetTag == _tag)
                {
                    if (targetTag == "ThreeLeafClover" /*&& isCoroutine == false*/)
                    {
                        StartCoroutine("DestroyObject", _time);
                    }
                    if (targetTag == "FourLeafClover")
                    {
                        //  TODO : 멋진 파티클효과
                    }
                }
            }
        }
        else
        {
            StartCoroutine("DestroyObject", 0);
        }
    }

    public bool RayCastHit()
    {
        if (rightRayInteractor.TryGetCurrent3DRaycastHit(out RightRayHit))
        {
            targetObject = RightRayHit.transform.gameObject;
        }
        else if (leftRayInteractor.TryGetCurrent3DRaycastHit(out leftRayHit))
        {
            targetObject = leftRayHit.transform.gameObject;
        }
        else
        {
            targetObject = null;
            return false;
        }
        return true;
    }

    IEnumerator DestroyObject(float _time)
    {
        yield return new WaitForSeconds(_time);
        targetObject.SetActive(false);

        GameObject respawnClover = targetObject;
        targetObject = null;

        // TODO : 잡은 판정이 있다면 취소가 필요

        yield return new WaitForSeconds(1);

        respawnClover.SetActive(true);
        CloverSpawnManager.Instance.ReSpawnClover(respawnClover.transform, respawnClover.GetComponent<CloverInfo>().Area);
        StopCoroutine("DestroyObject");
    }
}