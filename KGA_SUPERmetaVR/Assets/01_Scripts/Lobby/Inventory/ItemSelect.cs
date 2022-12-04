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
    private ContinuousMoveProviderBase ContinuousMoveProviderBase;

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
        playerData = GameManager.Instance.PlayerData;

        if (playerData.PaperSwanData.beRewarded > 0)
        {
            StopCoroutine(ResultMessage());
            StartCoroutine(ResultMessage());
        }
        ContinuousMoveProviderBase = GetComponent<ContinuousMoveProviderBase>();
        rayInteractor = new XRRayInteractor[hand.Length];
        XRInteractorLineVisual = new XRInteractorLineVisual[hand.Length];

        for (int i = 0; i < hand.Length; i++)
        {
            rayInteractor[i] = hand[i].GetComponent<XRRayInteractor>();
            XRInteractorLineVisual[i] = hand[i].GetComponent<XRInteractorLineVisual>();
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
        if (isFFFGameStart)
        {
            GetUIRayCastHit();
        }
    }

    public void GetUIRayCastHit() // ����������
    {
        RaycastResult leftRayResult;
        if (rayInteractor[0].TryGetCurrentUIRaycastResult(out leftRayResult))
        {
            Button leftButton = leftRayResult.gameObject.GetComponent<Button>();
            if (leftButton == null) return;
            if (leftButton.interactable == true)
            {
                leftButton.onClick.Invoke();
            }
        }
        RaycastResult rightRayResult;
        if (rayInteractor[1].TryGetCurrentUIRaycastResult(out rightRayResult))
        {
            Button rightButton = rightRayResult.gameObject.GetComponent<Button>();
            if (rightButton == null) return;
            if (rightButton.interactable == true)
            {
                rightButton.onClick.Invoke();
            }
        }
    }

    public void HoverGet3DRayCastHit()
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

        if (targetObject == null) return;

        grabObject = targetObject;
        grabTag = grabObject.tag;
        grabLayer = grabObject.layer;

        if (grabLayer.Equals(LayerMask.NameToLayer("Item")) || grabLayer.Equals(LayerMask.NameToLayer("GrabItem")))
        {
            itemSocket.SetActive(true);
            grabObject.layer = LayerMask.NameToLayer("GrabItem");
            foreach (Transform child in grabObject.transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("GrabItem");
            }
        }
        if (grabTag.Equals("ThreeLeafClover") || grabTag.Equals("FourLeafClover"))
        {
            grabCloverInfo = grabObject.GetComponent<CloverInfo>();
            SoundManager.Instance.PlaySE("CC_Pick.mp3");
            if (grabTag.Equals("ThreeLeafClover")) // ����Ŭ�ι���
            {
                if (isDestroyCloverRun == false)
                {
                    StartCoroutine(DestroyObject());
                }
            }
        }
        else if (grabTag.Equals("Star"))
        {
            grabStarInfo = grabObject.GetComponent<FD_Dragon>();
            isPlay = false;
            if (FlyDragonDataBase.Instance.CheckCooltime(0)) // ��Ÿ��
            {
                grabStarInfo.IsGrabParticlePlay = true;
                grabStarInfo.Rigidbody.isKinematic = true;
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
    }

    private void GrabOut()
    {
        isGrabRun = false;
        itemSocket.SetActive(false);
        if (grabObject == null) return;

        if (grabTag.Equals("ThreeLeafClover") || grabTag.Equals("FourLeafClover"))
        {
            if (grabCloverInfo == null) return;
            if (grabCloverInfo.IsStartFadedout == false)
            {
                rayInteractor[0].enableInteractions = false;
                StartCoroutine(Activeinteractor());
                StopCoroutine(DestroyObject());
                isDestroyCloverRun = false;
                grabCloverInfo.IsStartFadedout = true;
                grabObject = null;
            }
        }
        else if (grabTag.Equals("Star"))
        {
            if (grabStarInfo == null) return;
            if (isPlay)
            {
                grabStarInfo.IsFlyParticlePlay = true;
                isPlay = false;
                playerData.PaperSwanData.beRewarded = 1;
                StopCoroutine(ResultMessage());
                StartCoroutine(ResultMessage());
            }
            else
            {
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

    public void HideRightRay(bool _isHide)
    {
        if (_isHide) // ������ ���Ӹ��
        {
            isFFFGameStart = true;
            SetRightRay(0, 0.3f, 0);
        }
        else
        {
            isFFFGameStart = false;
            SetRightRay(2, 3, 1);
        }
    }

    private void SetRightRay(float _moveSpeed, float _maxRaycastDistance, float _alphaKey)
    {
        ContinuousMoveProviderBase.moveSpeed = _moveSpeed; // ������ ����
        rayInteractor[1].maxRaycastDistance = _maxRaycastDistance; // ���̱��� ����

        Color blueColor = Color.HSVToRGB(196, 62, 100);
        Color whiteColor = Color.HSVToRGB(0, 0, 97);
        XRInteractorLineVisual[1].validColorGradient = new Gradient
        {
            colorKeys = new[] { new GradientColorKey(blueColor, 0f), new GradientColorKey(blueColor, 1f) },
            alphaKeys = new[] { new GradientAlphaKey(_alphaKey, 0f), new GradientAlphaKey(_alphaKey, 1f) },
        };
        XRInteractorLineVisual[1].invalidColorGradient = new Gradient
        {
            colorKeys = new[] { new GradientColorKey(whiteColor, 0f), new GradientColorKey(whiteColor, 1f) },
            alphaKeys = new[] { new GradientAlphaKey(_alphaKey, 0.5f), new GradientAlphaKey(0f, 0.6f) },
        };
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
            yield return new WaitForSecondsRealtime(300f);
            if (FlyDragonDataBase.Instance.CheckCooltime(0)) // ��Ÿ��
            {
                GameManager.Instance.PlayerData.PaperSwanData.beRewarded = 0;
                DataBase.Instance.sqlcmdall($"UPDATE {FlyDragonTableInfo.table_name} SET " +
                            $"{FlyDragonTableInfo.be_rewarded} = {playerData.PaperSwanData.beRewarded} " +
                            $"WHERE {FlyDragonTableInfo.user_id} = '{playerData.ID}'");
                RewardManager.Instance.OpenRewardMessage();
                break;
            }
        }
    }
}