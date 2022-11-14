using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PCCameraCulling : MonoBehaviourPun
{

    [SerializeField]
    private GameObject PCModel;

    private void Awake()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) { return; }
    }

    private void Start()
    {
        if(photonView.IsMine == true && PhotonNetwork.IsConnected == true)
        CullingBody();
    }

    public void CullingBody()
    {
        PCModel.SetActive(false);
    }

}