using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FD_GrabPaperSwan : MonoBehaviour
{
    [Header("Hand")]
    [SerializeField] GameObject leftHand;
    [SerializeField] GameObject rightHand;

    private XRRayInteractor leftRayInteractor;
    private XRRayInteractor rightRayInteractor;
    private XRController leftXRController;
    private XRController rightXRController;

    private RaycastHit leftRayHit;
    private RaycastHit RightRayHit;
    private GameObject targetObject;

    bool isCoroutine;

    private void Start()
    {
        leftRayInteractor = leftHand.GetComponent<XRRayInteractor>();
        rightRayInteractor = rightHand.GetComponent<XRRayInteractor>();
        leftXRController = leftHand.GetComponent<XRController>();
        rightXRController = rightHand.GetComponent<XRController>();
    }

    void Update()
    {
        GetTriggerValue("PaperSwan", 5f);
    }

    public void GetTriggerValue(string _tag, float _time)
    {
        bool leftTriggerValue, rightTriggerValue;
        InputHelpers.IsPressed(leftXRController.inputDevice, InputHelpers.Button.Grip, out leftTriggerValue);
        InputHelpers.IsPressed(rightXRController.inputDevice, InputHelpers.Button.Grip, out rightTriggerValue);

        if (leftTriggerValue || rightTriggerValue) // 오른손 왼손중 하나라도 trigger를 누르면
        {
            if (RayCastHit())
            {
                if (targetObject.tag == _tag && isCoroutine == false)
                {
                    if(PaperSwanDataBase.Instance.CheckCooltime(3))
                    {
                        PaperSwanDataBase.Instance.UpdatePlayData();
                        isCoroutine = true;
                        StartCoroutine("DestroyObject", _time);
                    }
                }
                else
                {
                    isCoroutine = false;
                }
            }
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
        if (targetObject.tag == "PaperSwan")
        {
            targetObject.SetActive(false);
            StopCoroutine("ResultMessage");
            StartCoroutine("ResultMessage");
        }
        isCoroutine = false;
    }

    IEnumerator ResultMessage()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(600f);
            if(PaperSwanDataBase.Instance.CheckCooltime(2))
            {
                UnityEngine.Debug.Log("메시지 출력");
                break;
            }
        }
    }
}