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
        // GetComponent<XRRayInteractor>().enabled = false;
    }

    /// <summary>
    /// ������ �� ���̵�ƿ� ����
    /// </summary>
    public void GrabOutHand()
    {
        isGrabItem = false;
        if (isStartFadedout == false)
        {
            // �̰� �ϴ� ������ ���� Ŭ�ι����� ���̵�ƿ� �ڷ�ƾ�� �ι� �����ϴ� �� �����ϱ� �����̴�
                    // �ٵ� ���� �ٸ� Ŭ�ι��� ���̵�ƿ� �ڷ�ƾ�� ���ÿ� ������ �� ����
                // �׷� ���̵�ƿ��� ������ Ŭ�ι��� �����ϴ°� �´µ�
                // �׷� ���� Ŭ�ι��� ���̵�ƿ� �ڷ�ƾ�� �ι� ������ ���� ����
            StopCoroutine("DestroyObject"); // �̰ŋ����� ������ ����ִ� Ŭ�ι��� 2�������� ���� ���� ���̵�ƿ��� �ȵ�

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
        Debug.Log(targetObject.name + "���ֶ�");
    }

    private IEnumerator FadeoutRespawnClover(GameObject _targetClover)
    {
        string tag = _targetClover.tag;
        _targetClover.GetComponent<XRGrabInteractable>().enabled = false;
        _targetClover.tag = "Untagged";
        Debug.Log(_targetClover.name + "���ش�");
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
        Debug.Log(_targetClover.name + "��������");

        _targetClover.SetActive(false);
        myRenderer.material = defaultMaterial;

        yield return new WaitForSeconds(0.1f);
        CloverSpawnManager.Instance.ReSpawnClover(_targetClover.transform, _targetClover.GetComponent<CloverInfo>().Area);
        Debug.Log(_targetClover.name + "�������Ѵ�");
        _targetClover.tag = tag;
        _targetClover.GetComponent<XRGrabInteractable>().enabled = true;

    }

    IEnumerator ChangeTag(GameObject _item)
    {
        yield return new WaitForSeconds(3f);
        _item.tag = "Item";
    }
}