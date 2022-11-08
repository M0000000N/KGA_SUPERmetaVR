using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using InputDevice = UnityEngine.XR.InputDevice;

public class CC_GrabClover : MonoBehaviour
{
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private Material fadeMaterial;

    private XRHandController xRLefttHand;
    private XRHandController xRRightHand;

    private XRRayInteractor leftRayInteractor;
    private XRRayInteractor rightRayInteractor;

    private GameObject targetObject;
    private GameObject respawnClover;

    private UnityEngine.XR.InputDevice inputDevice;

    private bool isCoroutine;
    private bool isRespawnCoroutine;
    private Color alphaColor;
    private float timeToFade = 2f;

    void Start()
    {
        xRLefttHand = leftHand.GetComponentInChildren<XRHandController>();
        xRRightHand = rightHand.GetComponentInChildren<XRHandController>();

        leftRayInteractor = leftHand.GetComponentInChildren<XRRayInteractor>();
        rightRayInteractor = rightHand.GetComponentInChildren<XRRayInteractor>();

    }

    void Update()
    {
        GetTriggerValue("ThreeLeafClover", 2f);
        GetTriggerValue("FourLeafClover", 0f);
    }


    public void GetTriggerValue(string _tag, float _time)
    {
        xRLefttHand.InputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out bool leftGrab);
        xRRightHand.InputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out bool rightGrab);


        string targetTag = "";
        if (targetObject != null)
        {
            targetTag = targetObject.tag;
        }

        if (leftGrab || rightGrab) // ����� ��
        {
            if (RayCastHit()) // hit ������ �޾ƿ�
            {

                if (targetTag == _tag) // ���� ���� Ŭ�ι���
                {
                    if (targetTag == "ThreeLeafClover") // ����Ŭ�ι���
                    {
                        if (isCoroutine)
                        {
                            isCoroutine = false;
                            StopCoroutine("DestroyObject");
                            StartCoroutine(DestroyObject(_time, targetObject)); // 2�ʵڿ� ����
                        }
                    }
                    if (targetTag == "FourLeafClover") // ����Ŭ�ι���
                    {
                        //  TODO : ���� ��ƼŬȿ��
                    }

                }
            }
        }
        else
        {
            isCoroutine = true;
            if (targetTag == "ThreeLeafClover" || targetTag == "FourLeafClover" && targetObject != null)
            {
                targetObject.SetActive(false); // ���ֹ���
                respawnClover = targetObject;
                targetObject = null;
                StartCoroutine("RespawnObject", respawnClover);
            }
        }
    }

    IEnumerator DestroyObject(float _time, GameObject _targetObject)
    {
        Debug.Log("Destroy");
        yield return new WaitForSeconds(_time); // ���ʵڿ�
        //FadeoutObject(_targetObject); // ���ֹ���
        targetObject = null;

        StartCoroutine("RespawnObject", _targetObject);
        // TODO : ���� ������ �ִٸ� ��Ұ� �ʿ�
        isCoroutine = true;
    }

    IEnumerator RespawnObject(GameObject _respawnClover)
    {
        Debug.Log("Respawn");

        yield return new WaitForSeconds(1);

        _respawnClover.SetActive(true);
        CloverSpawnManager.Instance.ReSpawnClover(_respawnClover.transform, _respawnClover.GetComponent<CloverInfo>().Area);

        isRespawnCoroutine = true;
    }

    //private void FadeoutObject(GameObject _object)
    //{
    //}
    //private IEnumerator DieCoroutine(GameObject _object)
    //{
    //    MeshRenderer myRenderer = _object.GetComponentInChildren<MeshRenderer>();

    //    Color myColor = myRenderer.material.color;
    //    myRenderer.material = fadeMaterial;
    //    float decreaseValue = 1 / _time;
    //    while (0 < myRenderer.material.color.a)
    //    {
    //        myColor.a -= decreaseValue * Time.deltaTime;
    //        myRenderer.material.color = myColor;

    //        yield return null;
    //    }
    //    PeekabooGameManager.Instance.PlayerGameOver();
    //    photonView.RPC("PlayerDie", RpcTarget.All);
    //    Destroy(gameObject);
    //}

    public bool RayCastHit()
    {
        if (leftRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit leftRayHit))
        {
            targetObject = leftRayHit.transform.gameObject;
        }
        else if (rightRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit RightRayHit))
        {
            targetObject = RightRayHit.transform.gameObject;
        }
        else
        {
            targetObject = null;
            return false;
        }
        return true;
    }
}