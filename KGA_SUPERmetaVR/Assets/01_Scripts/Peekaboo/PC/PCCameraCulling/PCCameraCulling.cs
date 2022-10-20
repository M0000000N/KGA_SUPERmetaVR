using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PCCameraCulling : MonoBehaviourPun
{
    
    [SerializeField]
    private GameObject[] Hands;

    [SerializeField]
    private GameObject PCModel;

    private void Start()
    {
        CullingRay();
        CullingBody();
    }

    public void CullingRay()
    {
        if(!photonView.IsMine)
        {
            StartCoroutine("DisappearHand");
        }
    }

    public void CullingBody()
    {
        if(photonView.IsMine)
        {
            StartCoroutine("DisappearBody");
        }
    }

    public IEnumerator DisappearHand()
    {
        for(int i = 0; i < Hands.Length; ++i)
        {
            Hands[i].SetActive(false); 
        }
        //Color hands = Hand.GetComponent<Renderer>().material.color;
        //hands.a = 0;
        //Hand.GetComponent<Renderer>().material.color = hands; 

        yield return null; 
    }

    public IEnumerator DisappearBody()
    {
        PCModel.SetActive(false); 
        //Color PCModeling = PCModel.GetComponent<Renderer>().material.color;
        //PCModeling.a = 0;
        //PCModel.GetComponent<Renderer>().material.color = PCModeling;

        yield return null; 
    }


}
