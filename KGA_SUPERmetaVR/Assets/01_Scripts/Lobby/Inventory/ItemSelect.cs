using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ItemSelect : MonoBehaviour
{
    PlayerData playerData;

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

    private bool isPlay;
    private int coolTimeValue = 0;

    void Start()
    {
        playerData = GameManager.Instance.PlayerData;
        leftRayInteractor = leftHand.GetComponent<XRRayInteractor>();

        if(playerData.PaperSwanData.beRewarded > 0)
        {
            StopCoroutine(ResultMessage());
            StartCoroutine(ResultMessage());
        }
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
        grabTag = string.Empty;
        grabLayer = -1;

        if (targetObject != null)
        {
            grabObject = targetObject;
            grabTag = grabObject.tag;
            grabLayer = grabObject.layer;
            Debug.Log("타겟잇음");
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
        if (grabTag.Equals("ThreeLeafClover")) // 세잎클로버면
        {
            if (isDestroyCloverRun == false)
            {
                StartCoroutine(DestroyObject());
            }
        }
        else if(grabTag.Equals("Star"))
        {
            isPlay = false;
            if (FlyDragonDataBase.Instance.CheckCooltime(coolTimeValue))
            {
                if(FlyDragonDataBase.Instance.UpdatePlayData())
                {
                    isPlay = true;
                }
            }
            else
            {
                // 아직 쿨타임이 지나지 않았을 때
                grabObject.GetComponent<FD_Dragon>().IsStartFadedout = true;
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

             if (isPlay)
            {
                isPlay = false;
                playerData.PaperSwanData.beRewarded = 1;
                StopCoroutine(ResultMessage());
                StartCoroutine(ResultMessage());
            }
            else
            {
                // 쿨타임이 지나지 않은 경우
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
        // TODO : 게임 시작 시 코루틴 체크 필요
        while (true)
        {
            if (FlyDragonDataBase.Instance.CheckCooltime(coolTimeValue))
            {
                GameManager.Instance.PlayerData.PaperSwanData.beRewarded = 0;
                DataBase.Instance.sqlcmdall($"UPDATE {FlyDragonTableInfo.table_name} SET " +
                            $"{FlyDragonTableInfo.be_rewarded} = {playerData.PaperSwanData.beRewarded} " +
                            $"WHERE {FlyDragonTableInfo.user_id} = '{playerData.ID}'");
                RewardManager.Instance.OpenRewardMessage();
                break;
            }
            yield return new WaitForSecondsRealtime(600f);
        }
    }
}