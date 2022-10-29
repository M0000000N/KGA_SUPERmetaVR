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
    public XRNode XrNode = XRNode.LeftHand;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;
    private bool triggerIsPressed;

    [SerializeField]
    private XRRaycast raycastHit;

    [SerializeField]
    private PeekabooPC myPC;

    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(XrNode, devices);
        device = devices.FirstOrDefault();
    }

    void OnEnable()
    {
        if (!device.isValid)
        {
            GetDevice();
        }
    }

    private void Start()
    {   
        raycastHit = PeekabooGameManager.Instance.OVRCamera.GetComponent<XRRaycast>();
    }

    private void Update()
    {
        if (photonView.IsMine == false) return;
        ShowPeekaboo();
    }

    public void ShowPeekaboo()
    {
        //XR로 바꿈 
        bool triggerButtonValue = false;
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerButtonValue) && triggerButtonValue && !triggerIsPressed)
        {
            if (raycastHit.InteractCharacter() == null) return;

            if (raycastHit.InteractCharacter().GetComponent<PeekabooCharacter>() == null) return;

            PeekabooCharacter targetCharacter = raycastHit.InteractCharacter().GetComponent<PeekabooCharacter>();

            if (targetCharacter != null)
            {
                myPC.Attack(targetCharacter.gameObject);
                targetCharacter.TakeDamage(gameObject);
            }
            triggerIsPressed = true;
        }

        else if (!triggerButtonValue && triggerIsPressed)
        {
            triggerIsPressed = false;
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
