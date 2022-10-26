using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;
using UnityEditor;
using UnityEngine.Events;

// PC에게 공격 받았을 때
public class PCAppearPeekaboo : MonoBehaviourPun
{
    [SerializeField]
    public XRNode XRNode = XRNode.LeftHand;

    [SerializeField]
    private XRRaycast raycastHit;

    private bool triggerButton;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;

    private bool TriggerButton { get { return triggerButton; } }

    private void Start()
    {   
        raycastHit = PeekabooGameManager.Instance.OVRCamera.GetComponent<XRRaycast>();
    }

    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(XRNode, devices);
        device = devices.FirstOrDefault();
    }

    private void Update()
    {
        if (!device.isValid)
        {
            GetDevice();
        }

        ShowPeekaboo();
    }

    public void ShowPeekaboo()
    {
        //XR로 바꿈 
        bool isTrigger; 
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out isTrigger))
        {
            if (raycastHit.InteractCharacter() == null) return;
            if (raycastHit.InteractCharacter().GetComponent<PeekabooCharacter>() == null) return;

            PeekabooCharacter targetCharacter = raycastHit.InteractCharacter().GetComponent<PeekabooCharacter>();

            if (isTrigger != true)
            {
                if (targetCharacter != null)
                {
                    targetCharacter.TakeDamage(gameObject);
                }
                isTrigger = true;
            }
            else
            { isTrigger = false; }
            //if (layser.CreateRaycast().transform.gameObject == gameObject)
            //    StartCoroutine("FadeOutPeekaboo");
        }
    }

    //// 첫번째 피카부 두번째 NPC 
    //public IEnumerator FadeOutPeekaboo()
    //{
    //    Peekaboo.SetActive(true);
    //    for (float f = 1f; f > 0; f -= 0.02f)
    //    {
    //        Color c = Peekaboo.GetComponent<Image>().color;
    //        c.a = f;
    //        Peekaboo.GetComponent<Image>().color = c;
    //        yield return null;
    //    }
    //}
}
