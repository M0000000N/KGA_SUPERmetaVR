using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// PC에게 공격 받았을 때
public class PCAppearPeekaboo : MonoBehaviourPun
{
    [SerializeField]
    private PeekabooPC PC;
    [SerializeField]
    private PeekabooNPC NPC;

    [SerializeField]
    private GameObject laysers; 

    [SerializeField]
    private GameObject Peekaboo;

    [SerializeField]
    private LayserPointer layser;

    private RaycastHit hitInfo;
    private int layerMask;

    private void Start()
    {
        Peekaboo.SetActive(false);
    }

    private void Update()
    {
        ShowPeekaboo(); 
    }

    public void ShowPeekaboo()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) || Input.GetKeyDown(KeyCode.Space))
        {
            if(layser.CreateRaycast().transform.gameObject == gameObject) 
            StartCoroutine("FadeOutPeekaboo");          
        }
    }

    //// 첫번째 피카부 두번째 NPC 
    public IEnumerator FadeOutPeekaboo()
    {      
            Peekaboo.SetActive(true);
            for (float f = 1f; f > 0; f -= 0.02f)
            {
                Color c = Peekaboo.GetComponent<Image>().color;
                c.a = f;
                Peekaboo.GetComponent<Image>().color = c;
                yield return null;
            }
     }
}
   
    
