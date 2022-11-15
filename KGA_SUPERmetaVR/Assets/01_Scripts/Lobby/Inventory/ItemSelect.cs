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
    private string targetTag = string.Empty;
    private int targetLayer = 0;
    private CloverInfo targetCloverInfo;
    private FD_Dragon targetStarInfo;
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
                Debug.Log($"호버레이어{targetLayer}");
                Debug.Log($"아이템레이어{LayerMask.NameToLayer("Item")}");
                if (targetTag == "ThreeLeafClover" || targetTag == "FourLeafClover")
                {
                    targetObject = _leftRayHit.transform.gameObject;
                }
                else if (targetLayer == LayerMask.NameToLayer("Item"))
                {
                    Debug.Log("호버레이어드렁옴");
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
        targetTag = string.Empty;
        targetLayer = -1;
        Debug.Log("그렙댐");
        if (targetObject != null)
        {
            grabObject = targetObject;
            targetTag = grabObject.tag;
            targetLayer = grabObject.layer;
            Debug.Log("타겟잇음");
        }
        if (targetLayer == LayerMask.NameToLayer("Item"))
        {
            Debug.Log("레이어도드렁옴");
            itemSocket.SetActive(true);
            grabObject.layer = LayerMask.NameToLayer("GrabItem");
        }
        if (targetTag.Equals("ThreeLeafClover")) // 세잎클로버면
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
        if (targetTag.Equals("ThreeLeafClover") || targetTag.Equals("FourLeafClover"))
        {
            targetCloverInfo = grabObject.GetComponent<CloverInfo>();
        }
        else if (targetTag.Equals("Star"))
        {
            targetStarInfo.ParticleOn();

            if(FlyDragonDataBase.Instance.CheckCooltime(2))
            {
                FlyDragonDataBase.Instance.UpdatePlayData();
                StopCoroutine(ResultMessage());
                StartCoroutine(ResultMessage());
            }
            else
            {
                // 쿨타임이 지나지 않은 경우
            }
        }
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
    IEnumerator ResultMessage()
    {
        // TODO : 게임 시작 시 코루틴 체크 필요
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