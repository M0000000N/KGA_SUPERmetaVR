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
    [SerializeField] GameObject itemSocket;
    [SerializeField] private InputActionProperty isGrap;
    private bool isDestroyCloverRun = false;
    private bool isGrabRun = false;
    private string grabTag = string.Empty;
    private int grabLayer = 0;
    private CloverInfo grabCloverInfo;
    private FD_Dragon grabStarInfo;
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
                int targetLayer = _leftRayHit.transform.gameObject.layer;

                if (targetTag.Equals("ThreeLeafClover") || targetTag.Equals("FourLeafClover") || targetTag.Equals("Star") )
                {
                    targetObject = _leftRayHit.transform.gameObject;
                }
                else if (targetLayer.Equals(LayerMask.NameToLayer("Item")))
                {
                    Debug.Log("ȣ�����̾�巷��");
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
        grabTag = string.Empty;
        grabLayer = -1;

        if (targetObject != null)
        {
            grabObject = targetObject;
            grabTag = grabObject.tag;
            grabLayer = grabObject.layer;
            Debug.Log("Ÿ������");
        }
        if (grabLayer.Equals(LayerMask.NameToLayer("Item")) || grabLayer.Equals(LayerMask.NameToLayer("GrabItem")))
        {
            itemSocket.SetActive(true);
            grabObject.layer = LayerMask.NameToLayer("GrabItem");
            foreach (Transform child in grabObject.transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("GrabItem");
            }
        }
        if (grabTag.Equals("ThreeLeafClover")) // ����Ŭ�ι���
        {
            if (isDestroyCloverRun == false)
            {
                StartCoroutine(DestroyObject());
            }
        }
    }
    private void GrabOut()
    {
        isGrabRun = false;
        itemSocket.SetActive(false);
        if (grabObject == null) return;

        if (grabTag.Equals("ThreeLeafClover") || grabTag.Equals("FourLeafClover"))
        {
            grabCloverInfo = grabObject.GetComponent<CloverInfo>();

            if (grabCloverInfo == null) return;

            if (grabCloverInfo.IsStartFadedout == false)
            {
                leftRayInteractor.enableInteractions = false;
                StartCoroutine(Activeinteractor());
                StopCoroutine(DestroyObject());
                grabCloverInfo.IsStartFadedout = true;
                isDestroyCloverRun = false;
                targetObject = null;
                grabObject = null;
            }
        }
        else if (grabTag.Equals("Star"))
        {
            grabStarInfo = grabObject.GetComponent<FD_Dragon>();

            if (grabStarInfo == null) return;

            // grabStarInfo.ParticlePUN("ParticleOn");
            grabStarInfo.ParticleOn();

            if (FlyDragonDataBase.Instance.CheckCooltime(2))
            {
                FlyDragonDataBase.Instance.UpdatePlayData();
                StopCoroutine(ResultMessage());
                StartCoroutine(ResultMessage());
            }
            else
            {
                // ��Ÿ���� ������ ���� ���
                grabStarInfo.DestroyStar();
            }
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
    IEnumerator ResultMessage()
    {
        // TODO : ���� ���� �� �ڷ�ƾ üũ �ʿ�
        while (true)
        {
            yield return new WaitForSecondsRealtime(600f);
            if (FlyDragonDataBase.Instance.CheckCooltime(2))
            {
                RewardManager.Instance.OpenRewardMessage();
                break;
            }
        }
    }
}