using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using InputDevice = UnityEngine.XR.InputDevice;

public class CC_GrabClover : MonoBehaviour
{
    [SerializeField] GameObject leftHand;
    [SerializeField] GameObject rightHand;

    private XRHandController xRLefttHand;
    private XRHandController xRRightHand;

    private XRRayInteractor leftRayInteractor;
    private XRRayInteractor rightRayInteractor;

    private GameObject targetObject;

    private bool isGrab;

    private UnityEngine.XR.InputDevice inputDevice;

    void Start()
    {
        isGrab = false;

        xRLefttHand = leftHand.GetComponentInChildren<XRHandController>();
        xRRightHand = rightHand.GetComponentInChildren<XRHandController>();

        leftRayInteractor = leftHand.GetComponentInChildren<XRRayInteractor>();
        rightRayInteractor = rightHand.GetComponentInChildren<XRRayInteractor>();
    }

    void Update()
    {
        GetTriggerValue("ThreeLeafClover", 2f);
        GetTriggerValue("FourLeafClover", 0f);
    }


    public void GetTriggerValue(string _tag, float _time)
    {
        xRLefttHand.InputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out isGrab);
        xRRightHand.InputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out isGrab);
       
                string targetTag = targetObject.tag;
        if (isGrab) // 잡았을 때
        {
            if (RayCastHit()) // hit 정보를 받아옴
            {
                if (targetTag == _tag) // 내가 잡은 클로버가
                {
                    if (targetTag == "ThreeLeafClover") // 세잎클로버면
                    {
                        StartCoroutine("DestroyObject", _time); // 2초뒤에 폭발
                    }
                    if (targetTag == "FourLeafClover") // 네잎클로버면
                    {
                        //  TODO : 멋진 파티클효과
                    }
                }
            }
        }
        else
        {
            if(targetTag == "ThreeLeafClover" || targetTag == "FourLeafClover" && targetObject != null )
            {
                StartCoroutine("DestroyObject", 0);
            }
        }
    }

    IEnumerator DestroyObject(float _time)
    {
        yield return new WaitForSeconds(_time); // 몇초뒤에
        targetObject.SetActive(false); // 없애버려

        GameObject respawnClover = targetObject;
        targetObject = null;

        // TODO : 잡은 판정이 있다면 취소가 필요
        yield return new WaitForSeconds(1);

        respawnClover.SetActive(true);
        CloverSpawnManager.Instance.ReSpawnClover(respawnClover.transform, respawnClover.GetComponent<CloverInfo>().Area);
        StopCoroutine("DestroyObject");
    }

    public bool RayCastHit()
    {
        if (leftRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit leftRayHit))
        {
            targetObject = leftRayHit.transform.gameObject;
        }
        else if (rightRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit RightRayHit))
        {
            targetObject = RightRayHit.transform.gameObject;
        }
        else
        {
            targetObject = null;
            return false;
        }
        return true;
    }
}