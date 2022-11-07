
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.InteractionSubsystems;

public class XRControllerInput : MonoBehaviour
{
    // 왼손 오른손 선택 
    [SerializeField]
    private XRNode XRController = XRNode.LeftHand;

    private List<InputDevice> devices = new List<InputDevice>();

    private InputDevice device;

    public bool keyboardDebug = false;
    public float debugAxisValueIncrement = 0.1f;

    public float minAxisValue = 0.15f;

    bool _triggerButton = false;
    float _triggerValue = 0.0f;
    bool _gripButton = false;
    float _gripValue = 0.0f;
    bool _primary2DAxisButton = false;
    Vector2 _primary2DAxisValue = Vector2.zero;
    bool _secondary2DAxisButton = false;
    Vector2 _secondary2DAxisValue = Vector2.zero;
    bool _primaryButton = false;
    bool _secondaryButton = false;
    bool _menuButton = false;

    public bool triggerButton = false;
    [Range(0, 1)]
    public float triggerValue = 0.0f;
    public bool gripButton = false;
    [Range(0, 1)]
    public float gripValue = 0.0f;
    public bool primary2DAxisButton = false;
    [HideInInspector]
    public Vector2 primary2DAxisValue = Vector2.zero;
    [Range(-1, 1)]
    public float primary2DAxisXValue = 0.0f;
    [Range(-1, 1)]
    public float primary2DAxisYValue = 0.0f;
    public bool secondary2DAxisButton = false;
    [HideInInspector]
    public Vector2 secondary2DAxisValue = Vector2.zero;
    [Range(-1, 1)]
    public float secondary2DAxisXValue = 0.0f;
    [Range(-1, 1)]
    public float secondary2DAxisYValue = 0.0f;
    public bool primaryButton = false;
    public bool secondaryButton = false;
    public bool menuButton = false;

    // Events
  
    public UnityEvent OnTriggerPress;
    public UnityEvent OnTriggerRelease;

    public UnityEvent OnGripPress;
    public UnityEvent OnGripRelease;

    public UnityEvent OnPrimary2DAxisPress;
    public UnityEvent OnPrimary2DAxisRelease;

    public UnityEvent OnSecondary2DAxisPress;
    public UnityEvent OnSecondary2DAxisRelease;

    public UnityEvent OnPrimaryButtonPress;
    public UnityEvent OnPrimaryButtonRelease;

    public UnityEvent OnSecondaryButtonPress;
    public UnityEvent OnSecondaryButtonRelease;

    public UnityEvent OnMenuButtonPress;
    public UnityEvent OnMenuButtonRelease;


    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(XRController, devices);
        device = devices.FirstOrDefault();
    }

    void OnEnable()
    {
        if (!device.isValid)
        {
            GetDevice();
        }
    }

    private bool TriggerButtonAction
    {
        get { return triggerButton; }
        set
        {
            if (value == triggerButton) return;
            triggerButton = value;

            if (value == true) OnTriggerPress?.Invoke();
            else OnTriggerRelease?.Invoke();

            Debug.Log($"Trigger Press {triggerButton} on {XRController}");
        }
    }

    private bool GripButtonAction
    {
        get { return gripButton; }
        set
        {
            if (value == gripButton) return;
            gripButton = value;

            if (value == true) OnGripPress?.Invoke();
            else OnGripRelease?.Invoke();

            Debug.Log($"Grip Press {gripButton} on {XRController}");
           
        }
    }

    private bool Primary2DAxisButtonAction
    {
        get { return primary2DAxisButton; }
        set
        {
            if (value == primary2DAxisButton) return;
            primary2DAxisButton = value;

            if (value == true) OnPrimary2DAxisPress?.Invoke();
            else OnPrimary2DAxisRelease?.Invoke();

            Debug.Log($"Primary 2D Axis Button Press {primary2DAxisButton} on {XRController}");
           
        }
    }

    private bool Secondary2DAxisButtonAction
    {
        get { return secondary2DAxisButton; }
        set
        {
            if (value == secondary2DAxisButton) return;
            secondary2DAxisButton = value;

            if (value == true) OnSecondary2DAxisPress?.Invoke();
            else OnSecondary2DAxisRelease?.Invoke();

            Debug.Log($"Secondary 2D Axis Button Press {secondary2DAxisButton} on {XRController}");
           
        }
    }

    private bool PrimaryButtonAction
    {
        get { return primaryButton; }
        set
        {
            if (value == primaryButton) return;
            primaryButton = value;

            if (value == true) OnPrimaryButtonPress?.Invoke();
            else OnPrimaryButtonRelease?.Invoke();

            Debug.Log($"Primary Button Press {primaryButton} on {XRController}");
            
        }
    }

    private bool SecondaryButtonAction
    {
        get { return secondaryButton; }
        set
        {
            if (value == secondaryButton) return;
            secondaryButton = value;

            if (value == true) OnSecondaryButtonPress?.Invoke();
            else OnSecondaryButtonRelease?.Invoke();

            Debug.Log($"Secondary Button Press {secondaryButton} on {XRController}");
          
        }
    }

    private bool MenuButtonAction
    {
        get { return menuButton; }
        set
        {
            if (value == menuButton) return;
            menuButton = value;

            if (value == true) OnMenuButtonPress?.Invoke();
            else OnMenuButtonRelease?.Invoke();

            Debug.Log($"Menu Button Press {menuButton} on {XRController}");
            
        }
    }

    private float TriggerValueAction
    {
        get { return triggerValue; }
        set
        {
            if (value == triggerValue) return;
            triggerValue = value;

            ///Do something with the value

            Debug.Log($"Trigger Value {(Mathf.RoundToInt(triggerValue * 10f) / 10f)} on {XRController}"); //helps to keep values collapsed in console log
            
        }
    }

    private float GripValueAction
    {
        get { return gripValue; }
        set
        {
            if (value == gripValue) return;
            gripValue = value;

            ///Do something with the value

            Debug.Log($"Trigger Value {(Mathf.RoundToInt(gripValue * 10f) / 10f)} on {XRController}"); //helps to keep values collapsed in console log
           
        }
    }

    private Vector2 Primary2DAxisValueAction
    {
        get { return primary2DAxisValue; }
        set
        {
            if (value == primary2DAxisValue) return;
            primary2DAxisValue = value;
            primary2DAxisXValue = primary2DAxisValue.x;
            primary2DAxisYValue = primary2DAxisValue.y;

        }
    }

    private Vector2 Secondary2DAxisValueAction
    {
        get { return secondary2DAxisValue; }
        set
        {
            if (value == secondary2DAxisValue) return;
            secondary2DAxisValue = value;
            secondary2DAxisXValue = secondary2DAxisValue.x;
            secondary2DAxisYValue = secondary2DAxisValue.y;

           
        }
    }

    void Update()
    {
       

        if (!keyboardDebug)
        {
            if (!device.isValid)
            {
                GetDevice();
            }

            // These ranged, non-boolean inputs invoke the events above that are not targetable from the editor

            // Capture trigger value
            if (device.TryGetFeatureValue(CommonUsages.trigger, out _triggerValue))
            {
                if (_triggerValue > minAxisValue) TriggerValueAction = _triggerValue;
                else TriggerValueAction = 0f;
            }
            // Capture grip value
            if (device.TryGetFeatureValue(CommonUsages.grip, out _gripValue))
            {
                if (_gripValue > minAxisValue) GripValueAction = _gripValue;
                else GripValueAction = 0f;
            }
            //don't forget to use an absolute value for the axes

            // Capture primary 2D Axis
            if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out _primary2DAxisValue))
            {
                if (Mathf.Abs(_primary2DAxisValue.x) > minAxisValue || Mathf.Abs(_primary2DAxisValue.y) > minAxisValue) Primary2DAxisValueAction = _primary2DAxisValue;
                else Primary2DAxisValueAction = Vector2.zero;
            }

            // Capture secondary 2D Axis
            if (device.TryGetFeatureValue(CommonUsages.secondary2DAxis, out _secondary2DAxisValue))
            {
                if (Mathf.Abs(_secondary2DAxisValue.x) > minAxisValue || Mathf.Abs(_secondary2DAxisValue.y) > minAxisValue) Secondary2DAxisValueAction = _secondary2DAxisValue;
                else Secondary2DAxisValueAction = Vector2.zero;
            }


            // These press/release inputs invoke the public, editor-definable events above

            // Capture trigger button      
            if (device.TryGetFeatureValue(CommonUsages.triggerButton, out _triggerButton))
            {
                if (_triggerButton) TriggerButtonAction = true;
                else TriggerButtonAction = false;
            }

            // Capture grip button
            if (device.TryGetFeatureValue(CommonUsages.gripButton, out _gripButton))
            {
                if (_gripButton) GripButtonAction = true;
                else GripButtonAction = false;
            }

            // Capture primary 2d axis button
            if (device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out _primary2DAxisButton))
            {
                if (_primary2DAxisButton) Primary2DAxisButtonAction = true;
                else Primary2DAxisButtonAction = false;
            }

            // Capture secondary 2d axis button
            if (device.TryGetFeatureValue(CommonUsages.secondary2DAxisClick, out _secondary2DAxisButton))
            {
                if (_secondary2DAxisButton) Secondary2DAxisButtonAction = true;
                else Secondary2DAxisButtonAction = false;
            }

            // Capture primary button
            if (device.TryGetFeatureValue(CommonUsages.primaryButton, out _primaryButton))
            {
                if (_primaryButton) PrimaryButtonAction = true;
                else PrimaryButtonAction = false;
            }

            // Capture secondary button
            if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out _secondaryButton))
            {
                if (_secondaryButton) SecondaryButtonAction = true;
                else SecondaryButtonAction = false;
            }

            // Capture menu button
            if (device.TryGetFeatureValue(CommonUsages.menuButton, out _menuButton))
            {
                if (_menuButton) MenuButtonAction = true;
                else MenuButtonAction = false;
            }
        }
    }
}
