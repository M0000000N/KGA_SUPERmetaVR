using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject rightHand;
    private XRRayInteractor rightRayInteractor;
    
    void Start()
    {
        rightRayInteractor = rightHand.GetComponent<XRRayInteractor>();
    }
    public void HoverGet3DRayCastHit()
    {
        //raycastall vs raycast 
        if (rightRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit _rightRayHit))
        {

            string targetTag = _rightRayHit.transform.gameObject.tag;


            if (targetTag == "MenuUI")
            {
                Debug.Log("menuUIµé¾î¿È");
                _rightRayHit.transform.gameObject.transform.localScale *= 1.2f;
            }
        }
       
    }
}
