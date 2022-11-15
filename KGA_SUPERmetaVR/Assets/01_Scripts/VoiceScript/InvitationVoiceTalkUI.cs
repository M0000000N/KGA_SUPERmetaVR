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
    [SerializeField] GameObject SucessPopUI;

    [SerializeField] Button HandShakeImage;
    [SerializeField] Button Talking;
    [SerializeField] Button Accept;
    [SerializeField] Button Refuse;
    [SerializeField] Button Accept_SucessUI; 

    GameObject targetObject;

    // �����׸� ���̰� ������ ���� 
    private void Start()
    {
        // ���� ������ �� ���� 
        if (!photonView.IsMine)
        {
            DialogUI.SetActive(false);
            InvitationUI.SetActive(false);
            SucessPopUI.SetActive(false);

            Talking.onClick.AddListener(InvitationPopUI);
            Accept.onClick.AddListener(AcceptButton);
            Refuse.onClick.AddListener(RefuseButton);
            HandShakeImage.onClick.AddListener(DialogPopUI);
        }
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

    //private void ConnectionVoice()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(transform.position, Vector3.forward, out hit, 25f))
    //    {
    //        targetObject = hit.transform.gameObject;

    //        // �±װ� �÷��̾� / �� ������ �Ÿ��� 2.5m �̳��� �Ǽ� ���
    //        if (hit.transform.gameObject.tag == "Player")
    //        {
    //            if (targetObject == null) return;

    //            if (targetObject != null)
    //            { 
    //                if (photonView.IsMine == true) return;
    //                HandShakeImage.gameObject.SetActive(true);
    //                // �Ǽ� Ŭ���ϸ� ��ȣ�ۿ��� �� �ִ� UI ��� 
    //               // HandShakeImage.onClick.AddListener(DialogPopUI);
    //            }
    //        }
    //        else
    //        {
    //            return; 
    //        }
    //    }
    //}
    [PunRPC]
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            HandShakeImage.gameObject.SetActive(true);
        }
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
        SucessPopUI.SetActive(true);
    }
    [PunRPC]
    private void RefuseButton()
    {     
        InvitationUI.SetActive(false);
    }

}


