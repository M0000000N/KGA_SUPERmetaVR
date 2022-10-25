using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.XR;
using UnityEngine.Events;
using System.Net.NetworkInformation;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
public class PrimaryButtonEvent : UnityEvent<bool> { }

public class Peekaboo_XRPlayerMovement : MonoBehaviourPun
{
    
    // XR version controller
    [SerializeField]
    private XRNode xrNode = XRNode.LeftHand;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;

    private bool primary2DAxisIsChosen;
    private bool triggerIsPressed;
    private Vector2 primary2DAxisValue = Vector2.zero;
    private Vector2 prevPrimary2DAxisValue;

    // PC Speed
    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float runSpeed;

    [SerializeField]
    private Stamina stamina;

    [SerializeField]
    private Transform myCharacter;

    private NavMeshAgent navMeshAgent;
    private float applySpeed;
    private bool isRun = false;

    Vector3 setPos;
    Quaternion setRot;

    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(xrNode, devices);
        device = devices.FirstOrDefault();
    }

    private void OnEnable()
    {
        if (device.isValid == false)
        {
            GetDevice();
        }
    }

    private void Start()
    {
        //navMeshAgent = GetComponent<NavMeshAgent>();
        GetDevice();
        applySpeed = walkSpeed;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            // if (PeekabooGameManager.Instance.IsGameOver == false)
            TryRun();
            Move();
        }
    }


    private void Move()
    {
        Vector2 primary2dValue;
        InputFeatureUsage<Vector2> primary2DVector = CommonUsages.primary2DAxis;

        if (device.TryGetFeatureValue(primary2DVector, out primary2dValue))
        {
            //Vector3 dircetion = new Vector3(primary2dValue.x, 0, primary2dValue.y).normalized;
            //dircetion = Camera.main.transform.TransformDirection(dircetion);

            //transform.position += dircetion * applySpeed * Time.deltaTime;
            //navMeshAgent.SetDestination(transform.position);
            primary2DAxisIsChosen = false;
            var xAxis = primary2dValue.x * applySpeed * Time.deltaTime;
            var zAxis = primary2dValue.y * applySpeed * Time.deltaTime;

            Vector3 right = transform.TransformDirection(Vector3.right);
            Vector3 forward = transform.TransformDirection(Vector3.forward);

            transform.position += right * xAxis;
            transform.position += forward * zAxis;

            // navMeshAgent.SetDestination(transform.position);
        }
    }

    // 버튼 입력
    public void TryRun()
    {
        bool triggerButtonValue = false;
        InputFeatureUsage<bool> triggerButton = CommonUsages.secondaryButton;

        Vector2 primary2dValue;
        InputFeatureUsage<Vector2> primary2DVector = CommonUsages.primary2DAxis;

        if (device.TryGetFeatureValue(triggerButton, out triggerButtonValue) && device.TryGetFeatureValue(primary2DVector, out primary2dValue))
        {
            Debug.Log("뛰어라");
            triggerIsPressed = true;
            Running();
        }
        else if (triggerButtonValue == false && triggerIsPressed)
        {
            triggerIsPressed = false;
            RunningCancle();
        }
    }

    public void Running()
    {
        isRun = true;
        //stamina.DecreaseProgress();
        applySpeed = runSpeed;
    }

    public void RunningCancle()
    {
        isRun = false;
        //stamina.IncreaseProgress();
        applySpeed = walkSpeed;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(myCharacter.rotation);
        }
        else if (stream.IsReading)
        {
            setPos = (Vector3)stream.ReceiveNext();
            setRot = (Quaternion)stream.ReceiveNext();
        }
    }
}



