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
        if (rightRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit _rightRayHit))
        {

            int targetLayer = _rightRayHit.transform.gameObject.layer;
            Debug.Log("menuUI확인");
            Debug.Log($"{targetLayer}");

            if (targetLayer == LayerMask.NameToLayer("MenuUI"))
            {
                Debug.Log("menuUI드러옴");
                _rightRayHit.transform.gameObject.transform.localScale *= 1.2f;
            }
        }

    }
}