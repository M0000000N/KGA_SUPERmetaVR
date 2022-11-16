using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;

public class InvitationVoiceTalkUI : MonoBehaviourPun
{
    // 2.5���� �ȿ� ��� �Ǽ� �߱� 
    // 2.5���� ����� �Ǽ� �������� 

    [SerializeField] GameObject DialogUI;
    [SerializeField] GameObject InvitationUI; 
   // [SerializeField] GameObject SucessPopUI;

    [SerializeField] Button HandShakeImage;
    [SerializeField] Button Talking;
    [SerializeField] Button Yes;
    [SerializeField] Button No;
  //  [SerializeField] Button Accept_SucessUI; 

    GameObject targetObject;

    // �����׸� ���̰� ������ ���� 
    private void Start()
    {
        // ���� ������ �� ���� 
        DialogUI.SetActive(false);
        InvitationUI.SetActive(false);
        //SucessPopUI.SetActive(false);

        Talking.onClick.AddListener(InvitationPopUI);
        Yes.onClick.AddListener(AcceptButton);
        No.onClick.AddListener(RefuseButton);
        HandShakeImage.onClick.AddListener(DialogPopUI);

    }

    private void Update()
    {
        //ConnectionVoice();
    }

    [PunRPC]
    private void DialogPopUI()
    {
        DialogUI.SetActive(true);
    }

    [PunRPC]
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            HandShakeImage.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        HandShakeImage.gameObject.SetActive(false);
    }

    [PunRPC]
    private void InvitationPopUI()
    {
        InvitationUI.SetActive(true);
    }
    [PunRPC]
    private void AcceptButton()
    {
        InvitationUI.SetActive(false);
      //  SucessPopUI.SetActive(true);
    }
    [PunRPC]
    private void RefuseButton()
    {     
        InvitationUI.SetActive(false);
    }

}


