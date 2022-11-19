using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine.Events;
// A 버튼으로 변경
public class Peekaboo_XRPlayerMovement : MonoBehaviourPun
{
    [Header("PC Speed")]
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float runSpeed = 2f;
    [SerializeField] UnityEngine.InputSystem.InputActionReference BReference;
    [SerializeField] Stamina stamina;
    [SerializeField] Transform camera; 

    [Header("XR")]
    [SerializeField] private XRNode xRNode = XRNode.LeftHand;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;

    private bool primaryButtonIsPressed; // X / A 버튼
    private Vector2 primary2DAxisValue = Vector2.zero;
    private Vector2 prevPrimary2DAxisValue;

    private float applySpeed;
    private NavMeshAgent navMeshAgent;

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
      //   if (photonView.IsMine == false) return;

        GetDevice();

        applySpeed = walkSpeed;
        navMeshAgent = GetComponent<NavMeshAgent>();
        // stamina = GameObject.Find("Stamina").GetComponent<Stamina>();
    }

    void Update()
    {
     //   if (photonView.IsMine == false) return;

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
            direction = camera.TransformDirection(direction);
            direction.y = 0f;
            transform.position += direction * applySpeed * Time.deltaTime;

            navMeshAgent.SetDestination(transform.position);
        }
    }

    public void TryRun()
    {
       
        Vector2 primary2dValue;
        InputFeatureUsage<Vector2> primary2DVector = CommonUsages.primary2DAxis;

        // 빠른 이동 X 값으로 받는 중
        // 스태미나 함수 실행이 안 됨 
        if (device.TryGetFeatureValue(primary2DVector, out primary2dValue) && primary2dValue != Vector2.zero && BReference.action.IsPressed() && stamina.CurrentValue != 0)
        {
            Debug.Log("버튼 입력");
            Running();
        }
        else if (!BReference.action.IsPressed()|| stamina.CurrentValue == 0)
        {
            Debug.Log("버튼 out");
            RunningCancle();
        }
    }

    public void Running()
    {
        stamina.DecreaseProgress();
        applySpeed = runSpeed;
    }

    public void RunningCancle()
    {    
        stamina.IncreaseProgress();
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

