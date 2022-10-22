using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class AppearPeekaboo : MonoBehaviourPun
{
    [SerializeField]
    private GameObject[] Peekaboo;

    [SerializeField]
    private LayserPointer layser;

    private void Start()
    {
        for(int i = 0; i < Peekaboo.Length; i++)
        Peekaboo[i].SetActive(false);
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) || Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("FadeOutPeekaboo");
        }
    }

    // 첫번째 피카부 두번째 NPC 
    public IEnumerator FadeOutPeekaboo()
    {
        for (int i = 0; i < Peekaboo.Length; ++i)
        {
            if (i == 0)
            {
                if (layser.CreateFowardRaycast().transform.tag == "Player")
                {
                    Peekaboo[0].SetActive(true);

                    for (float f = 1f; f > 0; f -= 0.02f)
                    {
                        Color c = Peekaboo[0].GetComponent<Image>().color;
                        c.a = f;
                        Peekaboo[0].GetComponent<Image>().color = c;
                        yield return null;
                    }
                    yield return new WaitForSeconds(1);

                }

            }

            if (i == 1)
            {
                if (layser.CreateFowardRaycast().transform.tag == "Enemy")
                {
                    Debug.Log("1");

                    Peekaboo[1].SetActive(true);
                    for (float f = 1f; f > 0; f -= 0.02f)
                    {
                        Color c = Peekaboo[1].GetComponent<Image>().color;
                        c.a = f;
                        Peekaboo[1].GetComponent<Image>().color = c;
                        yield return null;
                    }
                    yield return new WaitForSeconds(1);
                }
            }
        }
    }
}
