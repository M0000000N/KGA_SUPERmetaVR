using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// PC���� ���� �޾��� ��
public class PCAppearPeekaboo : MonoBehaviourPun
{
  
    //[SerializeField]
    //private GameObject Peekaboo;

    [SerializeField]
    private LayserPointer layser;

    private void Start()
    {
        // Peekaboo.SetActive(false);
    }

    private void Update()
    {
        ShowPeekaboo();
    }

    public void ShowPeekaboo()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) || Input.GetKeyDown(KeyCode.Space))
        {
            PeekabooCharacter targetCharacter = layser.CreateRaycast().transform.gameObject.GetComponent<PeekabooCharacter>();

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