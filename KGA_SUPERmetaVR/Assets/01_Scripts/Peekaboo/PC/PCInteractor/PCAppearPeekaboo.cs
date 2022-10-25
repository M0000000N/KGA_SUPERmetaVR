using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// PC에게 공격 받았을 때
public class PCAppearPeekaboo : MonoBehaviourPun
{
  
    //[SerializeField]
    //private GameObject Peekaboo;

    //[SerializeField]
    //private LayserPointer layser;

    [SerializeField]
    private XRRaycast raycastHit;

    private void Start()
    {
        // Peekaboo.SetActive(false);
        raycastHit = PeekabooGameManager.Instance.OVRCamera.GetComponent<XRRaycast>();
    }

    private void Update()
    {
        ShowPeekaboo();
    }

    public void ShowPeekaboo()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) || Input.GetKeyDown(KeyCode.Space))
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
