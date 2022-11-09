using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.EventSystems;
using UnityEngine.Animations.Rigging;

public class ItemSelect : MonoBehaviour
{
    [SerializeField] GameObject leftHand;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material fadeMaterial;
    
    private XRRayInteractor leftRayInteractor;

    private GameObject targetObject;
    
    private float fadeoutTime = 2f;

    private bool isGrabItem;
    private bool isStartdestroy;
    private bool isStartFadedout;

    void Start()
    {
        leftRayInteractor = leftHand.GetComponent<XRRayInteractor>();

        isGrabItem = false;
        isStartdestroy = false;
        isStartFadedout = false;
    }

    private void Update()
    {
        if (isStartdestroy)
        {
            StartCoroutine("DestroyObject");
        }
        if (isStartFadedout)
        {
            StartCoroutine("FadeoutRespawnClover", targetObject);
        }
    }

    /// <summary>
    /// 쥐고있지 않을 때 타겟오브젝트 실시간 설정
    /// </summary>
    public void HoverGet3DRayCastHit()
    {
        if (leftRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit _leftRayHit) && isGrabItem == false)
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

    /// <summary>
    /// 쥐었을 때 태그별 함수 실행
    /// </summary>
    public void GrabHand()
    {
        isGrabItem = true;
        string targetTag = "";
        if (targetObject != null)
        {
            targetTag = targetObject.tag;
        }
        if (targetTag == "ThreeLeafClover") // 세잎클로버면
        {
            if (isStartdestroy == false)
            {
                isStartdestroy = true;
            }
        }
        if (targetTag == "FourLeafClover") // 네잎클로버면
        {
            //  TODO : 멋진 파티클효과
        }
        // GetComponent<XRRayInteractor>().enabled = false;
    }

    /// <summary>
    /// 떼었을 때 페이드아웃 실행
    /// </summary>
    public void GrabOutHand()
    {
        isGrabItem = false;
        if (isStartFadedout == false)
        {
            // 이걸 하는 이유는 같은 클로버에서 페이드아웃 코루틴을 두번 실행하는 걸 방지하기 위함이다
                    // 근데 각기 다른 클로버는 페이드아웃 코루틴을 동시에 진행할 수 있음
                // 그럼 페이드아웃은 각각의 클로버가 실행하는게 맞는듯
                // 그럼 같은 클로버의 페이드아웃 코루틴을 두번 실항할 일이 없다
            StopCoroutine("DestroyObject"); // 이거떄문에 이전에 잡고있던 클로버를 2초지나기 전에 떼면 페이드아웃이 안됨

            isStartFadedout = true;
        }
        // GetComponent<XRRayInteractor>().enabled = true;
    }

    private IEnumerator DestroyObject()
    {
        isStartdestroy = false;
        yield return new WaitForSeconds(2f);
        if (isStartFadedout == false)
        {
            isStartFadedout = true;
        }
        Debug.Log(targetObject.name + "없애라");
    }

    private IEnumerator FadeoutRespawnClover(GameObject _targetClover)
    {
        string tag = _targetClover.tag;
        _targetClover.GetComponent<XRGrabInteractable>().enabled = false;
        _targetClover.tag = "Untagged";
        Debug.Log(_targetClover.name + "없앤다");
        isStartFadedout = false;

        MeshRenderer myRenderer = _targetClover.GetComponentInChildren<MeshRenderer>();
        myRenderer.material = fadeMaterial;
        Color myColor = myRenderer.material.color;
        // float decreaseValue = 1 / fadeoutTime;

        while (0 < myRenderer.material.color.a)
        {
            myColor.a -= 0.1f / fadeoutTime;
            myRenderer.material.color = myColor;
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log(_targetClover.name + "없어졌다");

        _targetClover.SetActive(false);
        myRenderer.material = defaultMaterial;

        yield return new WaitForSeconds(0.1f);
        CloverSpawnManager.Instance.ReSpawnClover(_targetClover.transform, _targetClover.GetComponent<CloverInfo>().Area);
        Debug.Log(_targetClover.name + "리스폰한다");
        _targetClover.tag = tag;
        _targetClover.GetComponent<XRGrabInteractable>().enabled = true;

    }

    IEnumerator ChangeTag(GameObject _item)
    {
        yield return new WaitForSeconds(3f);
        _item.tag = "Item";
    }
}