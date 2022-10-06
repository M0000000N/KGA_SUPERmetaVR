using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class MiniminiCPlayerController : MonoBehaviour
{
    public MiniminiCBasket Basket;

    bool isLeftPick;
    public MiniminiCHand LeftHand;

    bool isRightPick;
    public MiniminiCHand RightHand;

    void Start()
    {

    }

    void Update()
    {
        OculusInput();
        PickBasket();
    }

    void OculusInput()
    {
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && LeftHand.CanPick)
        {
            // 왼 손 트리거 버튼
            isLeftPick = true;
        }
        else
        {
            isLeftPick = false;
        }

        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) && RightHand.CanPick)
        {
            // 오른 손 트리거 버튼
            isRightPick = true;
        }
        else
        {
            isRightPick = false;
        }
    }

    public void PickBasket()
    {
        if(isLeftPick && isRightPick)
        {
            Basket.rigidbody.isKinematic = false;
        }
        else
        {
            Basket.rigidbody.isKinematic = true;
        }
    }
}
