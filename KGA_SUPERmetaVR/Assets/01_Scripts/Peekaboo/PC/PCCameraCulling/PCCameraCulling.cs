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
        // �ڱ� �ڽſ��Ը� ���̵���      
        for (int i = 0; i < Hands.Length; ++i)
        {
            Hands[i].SetActive(false);
        }
        //StartCoroutine("DisappearHand");

    }

    public void CullingBody()
    {
        // �ڱ� �ڽſ��� ���� �� ���̵���    
        PCModel.SetActive(false);
    }

}