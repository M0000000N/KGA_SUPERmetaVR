using Photon.Pun; 
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine.Events; 

public class Peekaboo_XRPlayerMovement : MonoBehaviourPun
{
    [Header("PC Speed")]

    [SerializeField]
    private float walkSpeed = 1f;

    [SerializeField]
    private float runSpeed = 2f;

    [SerializeField]
    private Stamina stamina;

    [Header("XR")]
    [SerializeField]
    private XRNode controllerNode = XRNode.LeftHand;

    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;

    private bool buttonPressed;

    private float applySpeed;
    private bool isRun = false;
    private NavMeshAgent navMeshAgent;
 
    Vector3 setPos;
    Quaternion setRot;

    void Start()
    {
        if (photonView == false) return;

        GetDevice(); 
        applySpeed = walkSpeed;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void GetDevice()
    {
        //InputDevices.GetDevices(devices);
         InputDevices.GetDevicesAtXRNode(controllerNode, devices);
       // InputDevices.GetDevicesAtXRNode(controllerRight, devices);
       // InputDevices.GetDevices(devices);
        device = devices.FirstOrDefault();
    }

    void Update()
    {
        if (photonView == false) return;

        if (device == null)
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
            var xAxis = primary2dValue.x * applySpeed * Time.deltaTime;
            var zAxis = primary2dValue.y * applySpeed * Time.deltaTime;

            Vector3 right = transform.TransformDirection(Vector3.right);
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 left = transform.TransformDirection(Vector3.left);
            Vector3 back = transform.TransformDirection(Vector3.back);

            transform.position += right * xAxis;
            transform.position += forward * zAxis;
            transform.position -= left * xAxis;
            transform.position -= back * zAxis; 

            navMeshAgent.SetDestination(transform.position);
        }
    }

    public void TryRun()
    {
        bool pressbutton;
        InputFeatureUsage<bool> secondaryBbutoon = CommonUsages.secondaryButton;

        Vector2 primary2dValue;
        InputFeatureUsage<Vector2> primary2DVector = CommonUsages.primary2DAxis;

       if (device.TryGetFeatureValue(secondaryBbutoon, out pressbutton) && pressbutton && stamina.GetProgress() > 0 && device.TryGetFeatureValue(primary2DVector, out primary2dValue) && primary2dValue != Vector2.zero)
        {
            if (buttonPressed == false)
            {
                Debug.Log("´Þ¸®³ª?");
                Running();
                buttonPressed = true;
            }
        }
        else if (buttonPressed && device.TryGetFeatureValue(primary2DVector, out primary2dValue) && primary2dValue != Vector2.zero && stamina.GetProgress() > 0)
        {

            RunningCancle();
            buttonPressed = false;
        }
    }


    public void Running()
    {
        isRun = true;
        stamina.DecreaseProgress();
        applySpeed = runSpeed;
    }

    public void RunningCancle()
    {
        isRun = false;
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



