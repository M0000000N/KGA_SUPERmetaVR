using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;

public class XRRaycast : MonoBehaviour
{

    [SerializeField] XRRayInteractor leftRayInteractor;
    [SerializeField] XRRayInteractor RightRayInteractor;

    private RaycastHit leftRayHit;
    private RaycastHit RightRayHit;

    GameObject targetObject;

    private void Update()
    {
        RayCastHit();
    }

    public void RayCastHit()
    {
        if (RightRayInteractor.TryGetCurrent3DRaycastHit(out RightRayHit))
        {
            targetObject = RightRayHit.transform.gameObject;
            Debug.Log("target : " + targetObject.name);
        }
        else if (leftRayInteractor.TryGetCurrent3DRaycastHit(out leftRayHit))
        {
            targetObject = leftRayHit.transform.gameObject;
            Debug.Log("target : " + targetObject.name);
        }
        else
        {
            targetObject = null;
        }
    }

    public GameObject InteractCharacter()
    {
        return targetObject;
    }

}
