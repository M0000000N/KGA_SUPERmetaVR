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
    [SerializeField] GameObject MyVoicePanel;

    [SerializeField] Button HandShakeImage;
    [SerializeField] Button Talking;
    [SerializeField] Button Accept;
    [SerializeField] Button Refuse; 


    [SerializeField] float Distance = 2.5f;

    GameObject targetObject;

    private void Start()
    {
        DialogUI.SetActive(false);
        InvitationUI.SetActive(false);
        MyVoicePanel.SetActive(false);
     
        Talking.onClick.AddListener(InvitationPopUI);
        Accept.onClick.AddListener(AcceptButton);
        Refuse.onClick.AddListener(RefuseButton);
        HandShakeImage.onClick.AddListener(DialogPopUI);
    }

    private void Update()
    {
        ConnectionVoice();
    }

    private void DialogPopUI()
    {
        DialogUI.SetActive(true);
    }

    private void ConnectionVoice()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, 25f))
        {
            targetObject = hit.transform.gameObject;

            // �±װ� �÷��̾� / �� ������ �Ÿ��� 2.5m �̳��� �Ǽ� ���
            if (hit.transform.gameObject.tag == "Player"/* && Vector3.Distance(hit.transform.position, transform.position) < Distance*/)
            {
                Debug.Log("������");
                if (targetObject == null) return;

                if (targetObject != null)
                { 
                    if (photonView.IsMine == true) return;
                    HandShakeImage.gameObject.SetActive(true);
                    // �Ǽ� Ŭ���ϸ� ��ȣ�ۿ��� �� �ִ� UI ��� 
                   // HandShakeImage.onClick.AddListener(DialogPopUI);
                }
            }
            else
            {
                return; 
            }
        }
    }

    private void InvitationPopUI()
    {
        InvitationUI.SetActive(true);
    }

    private void AcceptButton()
    {
        InvitationUI.SetActive(false);
        MyVoicePanel.SetActive(true);
    }

    private void RefuseButton()
    {
        MyVoicePanel.SetActive(false);
        InvitationUI.SetActive(false);
    }

}


