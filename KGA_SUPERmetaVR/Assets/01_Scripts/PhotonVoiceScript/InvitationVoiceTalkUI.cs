using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;

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

            // 태그가 플레이어 / 둘 사이의 거리가 2.5m 이내면 악수 띄움
            if (hit.transform.gameObject.tag == "Player"/* && Vector3.Distance(hit.transform.position, transform.position) < Distance*/)
            {
                Debug.Log("찍히나");
                if (targetObject == null) return;

                if (targetObject != null)
                { 
                    if (photonView.IsMine == true) return;
                    HandShakeImage.gameObject.SetActive(true);
                    // 악수 클릭하면 상호작용할 수 있는 UI 출력 
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


