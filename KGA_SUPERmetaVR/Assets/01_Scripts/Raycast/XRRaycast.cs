using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;

public class XRRaycast : MonoBehaviour
{
    [SerializeField] XRRayInteractor leftRayInteractor;
    [SerializeField] XRRayInteractor RightRayInteractor;

    private RaycastResult leftRayHit;
    private RaycastResult RightRayHit;

    GameObject targetObject;

    private void Update()
    {
        RayCastHit();
    }

    public void RayCastHit()
    {
        if (RightRayInteractor.TryGetCurrentUIRaycastResult(out RightRayHit))
        {
            targetObject = RightRayHit.gameObject;
        }
        else if (leftRayInteractor.TryGetCurrentUIRaycastResult(out leftRayHit))
        {
            targetObject = leftRayHit.gameObject;
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
