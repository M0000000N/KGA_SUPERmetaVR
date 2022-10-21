using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class AppearPeekaboo : MonoBehaviourPun
{
    //PC가 PC를 공격했을 때 

    [SerializeField]
    private GameObject Peekaboo;

    [SerializeField]
    private LayserPointer layser;

    private void Start()
    {
        Peekaboo.SetActive(false);
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) || Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("FadeOutPeekaboo");
        }
    }

    public IEnumerator FadeOutPeekaboo()
    {
        if (layser.CreateFowardRaycast().collider.tag == "Player" || layser.CreateFowardRaycast().collider.tag == "Enemy")
        {
            Debug.Log("코루틴함수2");
            Peekaboo.SetActive(true);

            for (float f = 0f; f <= 0; f += 0.2f)
            {
                Color c = Peekaboo.GetComponent<Image>().color;
                c.a = f;
                Peekaboo.GetComponent<Image>().color = c;
                yield return null;
            }
        }
    }
}
