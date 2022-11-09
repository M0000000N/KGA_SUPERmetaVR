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
    private bool isStartRespawn;

    void Start()
    {
        leftRayInteractor = leftHand.GetComponent<XRRayInteractor>();

        isGrabItem = false;
        isStartdestroy = false;
        isStartFadedout = false;
        isStartRespawn = false;
    }

    private void Update()
    {
        if (isStartdestroy)
        {
            StartCoroutine("DestroyObject"); // 2�ʵڿ� ����
        }
        if (isStartFadedout)
        {
            StartCoroutine("FadeoutObject");
        }
        if (isStartRespawn)
        {
            StartCoroutine("RespawnObject");
        }
    }

    /// <summary>
    /// ������� ���� �� Ÿ�ٿ�����Ʈ �ǽð� ����
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
    /// ����� �� �±׺� �Լ� ����
    /// </summary>
    public void GrabHand()
    {
        isGrabItem = true;
        string targetTag = "";
        if (targetObject != null)
        {
            targetTag = targetObject.tag;
        }
        if (targetTag == "ThreeLeafClover") // ����Ŭ�ι���
        {
            if (isStartdestroy == false)
            {
                isStartdestroy = true;
            }
        }
        if (targetTag == "FourLeafClover") // ����Ŭ�ι���
        {
            //  TODO : ���� ��ƼŬȿ��
        }
    }

    /// <summary>
    /// ������ �� ���̵�ƿ� ����
    /// </summary>
    public void GrabOutHand()
    {
        if (isStartFadedout == false)
        {
            isStartFadedout = true;
        }
    }

    private IEnumerator DestroyObject()
    {
        isStartdestroy = false;
        yield return new WaitForSeconds(2f);
        isStartFadedout = true;
    }

    private IEnumerator FadeoutObject()
    {
        isStartFadedout = false;

        MeshRenderer myRenderer = targetObject.GetComponentInChildren<MeshRenderer>();
        myRenderer.material = fadeMaterial;
        Color myColor = myRenderer.material.color;
        float decreaseValue = 1 / fadeoutTime;

        while (0 < myRenderer.material.color.a)
        {
            myColor.a -= decreaseValue * Time.deltaTime;
            myRenderer.material.color = myColor;
            yield return null;
        }
        targetObject.SetActive(false);
        myRenderer.material = defaultMaterial;
        // photonView.RPC("PlayerDie", RpcTarget.All);

        if (isStartRespawn == false)
        {
            isStartRespawn = true;
        }
        isGrabItem = false;
    }

    IEnumerator RespawnObject()
    {
        isStartRespawn = false;
        yield return new WaitForSeconds(1f);
        CloverSpawnManager.Instance.ReSpawnClover(targetObject.transform, targetObject.GetComponent<CloverInfo>().Area);
        targetObject.SetActive(true);
    }

    IEnumerator ChangeTag(GameObject _item)
    {
        yield return new WaitForSeconds(3f);
        _item.tag = "Item";
    }
}