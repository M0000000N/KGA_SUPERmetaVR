using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine.Events;

public class XR_Move : MonoBehaviourPun
{
    [Header("PC Speed")]
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float runSpeed = 2f;

    [Header("XR")]
    [SerializeField] private XRNode xRNode = XRNode.LeftHand;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;

    [SerializeField]
    private Camera camera;

    private bool triggerIsPressed;
    private bool primaryButtonIsPressed; // X / A 버튼
    private bool secondaryButtonIsPressed; // Y / B 버튼 
    private bool primary2DAxisIsChosen;
    private Vector2 primary2DAxisValue = Vector2.zero;
    private Vector2 prevPrimary2DAxisValue;

    private float applySpeed;

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
        applySpeed = walkSpeed;
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
            direction = camera.transform.TransformDirection(direction);
            direction.y = 0f;
            transform.position += direction * applySpeed * Time.deltaTime;
  
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
            Running();
            primaryButtonIsPressed = true;
        }
        else if (!primaryButtonValue && primaryButtonIsPressed)
        {
            Debug.Log("버튼 out");
            RunningCancle();
            primaryButtonIsPressed = false;
        }
    }

    public void Running()
    {    
        applySpeed = runSpeed;
    }

    public void RunningCancle()
    {
        applySpeed = walkSpeed;
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


