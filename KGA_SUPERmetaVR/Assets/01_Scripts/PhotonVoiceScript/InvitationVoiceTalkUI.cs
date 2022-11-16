using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using Photon.Realtime;

public class InvitationVoiceTalkUI : MonoBehaviourPun
{
    // 2.5미터 안에 들면 악수 뜨기 
    // 2.5미터 벗어나면 악수 없어지기 

    [SerializeField] GameObject DialogUI;
    [SerializeField] GameObject InvitationUI; 
    [SerializeField] GameObject MyVoicePanel;

    [SerializeField] Button HandShakeImage;
    [SerializeField] Button Talking;
    [SerializeField] Button Accept;
    [SerializeField] Button Refuse;

    Player player; 

    private void Start()
    {
        DialogUI.SetActive(false);
        InvitationUI.SetActive(false);
        MyVoicePanel.SetActive(false);
     
        Talking.onClick.AddListener(InvitationPopUI);
        Accept.onClick.AddListener(AcceptButton);
        Refuse.onClick.AddListener(RefuseButton);
        HandShakeImage.onClick.AddListener(DialogPopUI);

        // 내 권한이 아닐 때 
        if (!photonView.IsMine)
        {
            photonView.RPC("InvitationPopUI", player);
        }
    }

    [PunRPC]
    private void DialogPopUI()
    {
        DialogUI.SetActive(true);
    }

    // 충돌한 모두에게 보이는 거임 
    private void OnTriggerEnter(Collider other)
    {
        // 상호작용창 띄우기
        if(other.transform.tag == "Player")
        {
           // Player player = other.gameObject.GetPhotonView().Owner;
           // photonView.RPC("Hi", player);
            HandShakeImage.gameObject.SetActive(true);
        }
        else
        {
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        HandShakeImage.gameObject.SetActive(false);
    }

    // 초대장 띄우기 
    [PunRPC]
    private void InvitationPopUI()
    {
        InvitationUI.SetActive(true);
    }

    [PunRPC]
    private void AcceptButton()
    {
        InvitationUI.SetActive(false);
       // MyVoicePanel.SetActive(true);
    }

    [PunRPC]
    private void RefuseButton()
    {
       // MyVoicePanel.SetActive(false);
        InvitationUI.SetActive(false);
    }

}


