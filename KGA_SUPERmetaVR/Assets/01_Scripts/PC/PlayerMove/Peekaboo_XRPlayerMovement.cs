using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine.Events;


// 2초 동안 누르면 레이케스트 쏴짐 다시 2초 동안 누르면 레이케스트 들어감
// 누른 왼손 또는 오른손만 나옴
// 트리거 버튼 한번 누르면 이동함 

public class Peekaboo_XRPlayerMovement : MonoBehaviourPun
{
    [Header("PC Speed")]
    [SerializeField] private float Speed = 1f;
 
    [Header("XR")]
    [SerializeField] private XRNode xRNode = XRNode.LeftHand;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;

    private Transform myCameraTransform;

    private bool triggerIsPressed;
    private bool primaryButtonIsPressed; // X / A 버튼
    private bool secondaryButtonIsPressed; // Y / B 버튼 
    private bool primary2DAxisIsChosen;
    private Vector2 primary2DAxisValue = Vector2.zero;
    private Vector2 prevPrimary2DAxisValue;

    private float applySpeed;
    private bool isRun = false;
    private Stamina stamina;
    private NavMeshAgent navMeshAgent;
    private Camera camera;

    Vector3 setPos;
    Quaternion setRot;

    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(xRNode, devices);
        device = devices.FirstOrDefault();
    }

    void OnEnable()
    {
        if (!device.isValid)
        {
            GetDevice();
        }
    }

    void Start()
    {
        if (photonView.IsMine == false) return;

        GetDevice();
    }

    void Update()
    {
        if (photonView.IsMine == false) return;

        if (!device.isValid)
        {
            GetDevice();
        }

        TryRun();
        Move();
    }

    private void Move()
    {
        Vector2 primary2dValue;
        InputFeatureUsage<Vector2> primary2DVector = CommonUsages.primary2DAxis;

        if (device.TryGetFeatureValue(primary2DVector, out primary2dValue) && primary2dValue != Vector2.zero)
        {
            var xAxis = primary2dValue.x;// * applySpeed * Time.deltaTime;
            var zAxis = primary2dValue.y;// * applySpeed * Time.deltaTime;

            Vector3 direction = new Vector3(xAxis, 0f, zAxis).normalized;
            direction = myCameraTransform.TransformDirection(direction);
            direction.y = 0f;
            transform.position += direction * applySpeed * Time.deltaTime;

            navMeshAgent.SetDestination(transform.position);
        }
    }

    public void TryRun()
    {
        bool primaryButtonValue = false;
        InputFeatureUsage<bool> primaryButtonUsage = CommonUsages.primaryButton;

        Vector2 primary2dValue;
        InputFeatureUsage<Vector2> primary2DVector = CommonUsages.primary2DAxis;

        // 빠른 이동 X 값으로 받는 중
        // 스태미나 함수 실행이 안 됨 
        if (device.TryGetFeatureValue(primaryButtonUsage, out primaryButtonValue) && primaryButtonValue && !primaryButtonIsPressed && device.TryGetFeatureValue(primary2DVector, out primary2dValue) && primary2dValue != Vector2.zero)
        {
            Debug.Log("버튼 입력");
         
            primaryButtonIsPressed = true;
        }
        else if (!primaryButtonValue && primaryButtonIsPressed)
        {
            Debug.Log("버튼 out");
          
            primaryButtonIsPressed = false;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else if (stream.IsReading)
        {
            setPos = (Vector3)stream.ReceiveNext();
            setRot = (Quaternion)stream.ReceiveNext();
        }
    }
}