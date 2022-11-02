using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CC_GrabClover : MonoBehaviour
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

    private bool isGrab;

    private bool isCoroutine = false;
    private void Start()
    {
        leftRayInteractor = leftHand.GetComponent<XRRayInteractor>();
        rightRayInteractor = rightHand.GetComponent<XRRayInteractor>();
        leftXRController = leftHand.GetComponent<XRController>();
        rightXRController = rightHand.GetComponent<XRController>();
        isGrab = false;
    }

    void Update()
    {
        GetTriggerValue("ThreeLeafClover", 2f);
        GetTriggerValue("FourLeafClover", 0f);
    }

    public void GetTriggerValue(string _tag, float _time)
    {
        bool leftTriggerValue, rightTriggerValue;
        InputHelpers.IsPressed(leftXRController.inputDevice, InputHelpers.Button.Grip, out leftTriggerValue);
        InputHelpers.IsPressed(rightXRController.inputDevice, InputHelpers.Button.Grip, out rightTriggerValue);

        if (leftTriggerValue || rightTriggerValue && isGrab == false) // ������ �޼��� �ϳ��� trigger�� ������
        {
            if(isGrab == false)
            {
                isGrab = true;

                if (RayCastHit()) // hit ������ �޾ƿ�
                {
                    string targetTag = targetObject.tag;
                    if (targetTag == _tag)
                    {
                        if (targetTag == "ThreeLeafClover" && isCoroutine == false)
                        {
                            isCoroutine = true;
                            StartCoroutine("DestroyObject", _time);
                        }
                        else if (targetTag == "FourLeafClover")
                        {
                            //  TODO : ���� ��ƼŬȿ��
                        }
                    }
                }
            }
        }
        //else
        //{
        //    isGrab = false;
        //    targetObject.SetActive(false);
        //}
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
        if (targetObject.tag == "ThreeLeafClover")
        {
            targetObject.SetActive(false);
            GameObject respawnClover = targetObject;
            targetObject = null;

            // TODO : ���� ������ �ִٸ� ��Ұ� �ʿ�

            yield return new WaitForSeconds(1);

            respawnClover.SetActive(true);
            CloverSpawnManager.Instance.ReSpawnClover(respawnClover.transform, respawnClover.GetComponent<CloverInfo>().Area);
        }
        isCoroutine = false;
    }
}
