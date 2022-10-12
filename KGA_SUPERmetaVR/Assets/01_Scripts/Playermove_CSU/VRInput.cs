using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VRInput : BaseInput
{
    [SerializeField]
    private Camera eventCamera = null;

    [SerializeField]
    private OVRInput.Button click;

    protected override void Awake()
    {
     
    }

    public override bool GetMouseButton(int button)
    {
        return true;  
    }

    public override bool GetMouseButtonDown(int button)
    {
        return true; 
    }

    public override bool GetMouseButtonUp(int button)
    {
        return true;
    }

    public override Vector2 mousePosition
    {
        get
        {
            return Vector2.zero; 
        }
    }


}
