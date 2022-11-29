using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ItemSelect : MonoBehaviour
{
    PlayerData playerData;
    [Header("��Ʈ�ѷ�")]
    [SerializeField] private InputActionProperty isGrap; // �޼ո� �ش�
    [SerializeField] GameObject[] hand; // index : 0-left, 1-right
    private XRRayInteractor[] rayInteractor;
    private XRInteractorLineVisual[] XRInteractorLineVisual;
    private ContinuousMoveProviderBase[] ContinuousMoveProviderBase;

    [Header("��ȣ�ۿ���")]
    private GameObject targetObject; // ȣ���� ���� ��
    private GameObject grabObject; // ����� �� ���� ��
    [SerializeField] GameObject itemSocket;

    private bool isGrabRun = false;
    private string grabTag = string.Empty;
    private int grabLayer = 0;

    private bool isFFFGameStart = false;
    private bool isDestroyCloverRun = false;
    private CloverInfo grabCloverInfo;
    private FD_Dragon grabStarInfo;
    private bool isPlay;

    void Start()
    {
#if �κ��
        playerData = GameManager.Instance.PlayerData;

        if (playerData.PaperSwanData.beRewarded > 0)
        {
            StopCoroutine(ResultMessage());
            StartCoroutine(ResultMessage());
        }
#endif

        for (int i = 0; i < hand.Length; i++)
        {
            rayInteractor[i] = hand[i].GetComponent<XRRayInteractor>();
            XRInteractorLineVisual[i] = hand[i].GetComponent<XRInteractorLineVisual>();
            ContinuousMoveProviderBase[i] = hand[i].GetComponent<ContinuousMoveProviderBase>();
        }
    }

    public void HideRightRay(bool _isHide)
    {
        Color blueColor = Color.HSVToRGB(196, 62, 100);
        Color whiteColor = Color.HSVToRGB(0, 0, 97);
        if (_isHide) // ������ ���Ӹ��
        {
            isFFFGameStart = true;
            for (int i = 0; i < hand.Length; i++)
            {
                ContinuousMoveProviderBase[i].moveSpeed = 0; // ������ ����
                rayInteractor[i].maxRaycastDistance = 0.2f; // ���̱��� ����
                XRInteractorLineVisual[i].validColorGradient = new Gradient // ���� �����ϰ�
                {
                    colorKeys = new[] { new GradientColorKey(blueColor, 0f), new GradientColorKey(blueColor, 1f) },
                    alphaKeys = new[] { new GradientAlphaKey(0f, 0f), new GradientAlphaKey(0f, 1f) },
                };
                XRInteractorLineVisual[i].invalidColorGradient = new Gradient
                {
                    colorKeys = new[] { new GradientColorKey(whiteColor, 0f), new GradientColorKey(whiteColor, 1f) },
                    alphaKeys = new[] { new GradientAlphaKey(0f, 0f), new GradientAlphaKey(0f, 1f) },
                };
            }
        }
        else
        {
            isFFFGameStart = false;
            for (int i = 0; i < hand.Length; i++)
            {
                ContinuousMoveProviderBase[i].moveSpeed = 2;
                rayInteractor[i].maxRaycastDistance = 3f;
                XRInteractorLineVisual[i].validColorGradient = new Gradient
                {
                    colorKeys = new[] { new GradientColorKey(blueColor, 0f), new GradientColorKey(blueColor, 1f) },
                    alphaKeys = new[] { new GradientAlphaKey(0f, 0f), new GradientAlphaKey(0f, 1f) },
                };
                XRInteractorLineVisual[i].invalidColorGradient = new Gradient
                {
                    colorKeys = new[] { new GradientColorKey(whiteColor, 0f), new GradientColorKey(whiteColor, 1f) },
                    alphaKeys = new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) },
                };
            }
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
        if(isFFFGameStart)
        {
            RaycastResult leftRayResult;
            if (rayInteractor[0].TryGetCurrentUIRaycastResult(out leftRayResult))
            {
                Button leftButton = leftRayResult.gameObject.GetComponent<Button>();
                if (leftButton.interactable == true)
                {
                    leftButton.onClick.Invoke();
                }
            }
            RaycastResult rightRayResult;
            if (rayInteractor[1].TryGetCurrentUIRaycastResult(out rightRayResult))
            {
                Button rightButton = rightRayResult.gameObject.GetComponent<Button>();
                if (rightButton.interactable == true)
                {
                    rightButton.onClick.Invoke();
                }
            }
        }
        else
        {
            if (rayInteractor[0].TryGetCurrent3DRaycastHit(out RaycastHit _leftRayHit))
            {
                if (isGrabRun == false)
                {
                    string targetTag = _leftRayHit.transform.gameObject.tag;
                    int targetLayer = _leftRayHit.transform.gameObject.layer;

                    if (targetTag.Equals("ThreeLeafClover") || targetTag.Equals("FourLeafClover") || targetTag.Equals("Star"))
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
    }

    private void Grab()
    {
        isGrabRun = true;
        grabTag = string.Empty;
        grabLayer = -1;

        if (targetObject == null) return;

        grabObject = targetObject;
        grabTag = grabObject.tag;
        grabLayer = grabObject.layer;
        Debug.Log("Ÿ������");

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
        else if (grabTag.Equals("Star"))
        {
            SoundManager.Instance.PlaySE("star_hend.mp3");
            isPlay = false;
            if (FlyDragonDataBase.Instance.CheckCooltime(1))
            {
                if (FlyDragonDataBase.Instance.UpdatePlayData())
                {
                    isPlay = true;
                }
            }
            else
            {
                // ���� ��Ÿ���� ������ �ʾ��� ��
                grabObject.GetComponent<FD_Dragon>().IsStartFadedout = true;
            }
        }

        if (grabTag.Equals("ThreeLeafClover") || grabTag.Equals("FourLeafClover"))
        {
            SoundManager.Instance.PlaySE("CC_Pick.mp3");
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
                rayInteractor[0].enableInteractions = false;
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
            //TODO : ���� ����Ͽ� ����
            // SoundManager.Instance.PlaySE("Star_sky");
            grabStarInfo.IsParticlePlay = true;

            if (isPlay)
            {
                isPlay = false;
                playerData.PaperSwanData.beRewarded = 1;
                StopCoroutine(ResultMessage());
                StartCoroutine(ResultMessage());
            }
            else
            {
                // ��Ÿ���� ������ ���� ���
                grabStarInfo.IsStartFadedout = true;
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
        rayInteractor[0].enableInteractions = true;
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
            if (FlyDragonDataBase.Instance.CheckCooltime(1))
            {
                GameManager.Instance.PlayerData.PaperSwanData.beRewarded = 0;
                DataBase.Instance.sqlcmdall($"UPDATE {FlyDragonTableInfo.table_name} SET " +
                            $"{FlyDragonTableInfo.be_rewarded} = {playerData.PaperSwanData.beRewarded} " +
                            $"WHERE {FlyDragonTableInfo.user_id} = '{playerData.ID}'");
                RewardManager.Instance.OpenRewardMessage();
                break;
            }
            yield return new WaitForSecondsRealtime(300f);
        }
    }
}