using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.XR;

// PC���� ���� �޾��� ��
public class PCAppearPeekaboo : MonoBehaviourPun
{
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;

    [SerializeField]
    private XRRaycast raycastHit;

    [SerializeField]

   // private Peekaboo_XRPlayerMovement rxPlay;
    private InputDevice controller;
   
    private void Start()
    {
        raycastHit = PeekabooGameManager.Instance.OVRCamera.GetComponent<XRRaycast>();
    }

    private void Update()
    {
        ShowPeekaboo();
    }

    public void ShowPeekaboo()
    {

        //XR�� �ٲ� 
        if (device.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) &&
        triggerValue > 0.1f)
        {
            if (raycastHit.InteractCharacter() == null) return;
            if (raycastHit.InteractCharacter().GetComponent<PeekabooCharacter>() == null) return;

            PeekabooCharacter targetCharacter = raycastHit.InteractCharacter().GetComponent<PeekabooCharacter>();

            if (targetCharacter != null)
            {
                targetCharacter.TakeDamage(gameObject);
            }


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
