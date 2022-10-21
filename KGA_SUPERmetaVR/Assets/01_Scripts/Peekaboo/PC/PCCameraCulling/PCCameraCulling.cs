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

    private void Awake()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) { return; }
    }

    private void Start()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        CullingRay(); 

        if(photonView.IsMine == true && PhotonNetwork.IsConnected == true)
        CullingBody();
    }

    public void CullingRay()
    {
        // 자기 자신에게만 보이도록      
        for (int i = 0; i < Hands.Length; ++i)
        {
            Hands[i].SetActive(false);
        }
        //StartCoroutine("DisappearHand");

    }

    public void CullingBody()
    {
        // 자기 자신에게 모델이 안 보이도록    
        PCModel.SetActive(false);
    }

}