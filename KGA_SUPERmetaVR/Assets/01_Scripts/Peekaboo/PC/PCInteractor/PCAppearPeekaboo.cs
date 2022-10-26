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

// PC���� ���� �޾��� ��
public class PCAppearPeekaboo : MonoBehaviourPun
{
    [SerializeField]
    public XRNode XRNode = XRNode.LeftHand;
    [SerializeField]
    private XRNode controllerRight = XRNode.RightHand;

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
        InputDevices.GetDevicesAtXRNode(controllerRight, devices);

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
        //XR�� �ٲ� 
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

    //// ù��° ��ī�� �ι�° NPC 
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
