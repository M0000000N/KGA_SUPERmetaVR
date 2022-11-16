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
    // 2.5���� �ȿ� ��� �Ǽ� �߱� 
    // 2.5���� ����� �Ǽ� �������� 

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

        // �� ������ �ƴ� �� 
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

    // �浹�� ��ο��� ���̴� ���� 
    private void OnTriggerEnter(Collider other)
    {
        // ��ȣ�ۿ�â ����
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

    // �ʴ��� ���� 
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


