using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using Photon.Realtime;
using TMPro;
using Oculus.Interaction.PoseDetection.Debug;

public class InvitationVoiceTalkUI : MonoBehaviourPun
{
    // 2.5���� �ȿ� ��� �Ǽ� �߱� 
    // 2.5���� ����� �Ǽ� �������� 
    public GameObject GetHandShakeImage { get { return HandShakeImage.gameObject; } }

    [SerializeField] GameObject DialogUI;
    [SerializeField] GameObject InvitationUI;
    [SerializeField] GameObject ConfirmUI; 
   // [SerializeField] GameObject SucessPopUI;

    [SerializeField] Button HandShakeImage;
    [SerializeField] Button Talking;
    [SerializeField] Button Yes;
    [SerializeField] Button No;
    [SerializeField] Button Okay;
    //  [SerializeField] Button Accept_SucessUI; 

    [SerializeField] TextMeshProUGUI text;

    int actNumber; 
    GameObject targetObject;
    Player player;

    // �����׸� ���̰� ������ ���� 
    private void Start()
    {
        // ���� ������ �� ���� 
        DialogUI.SetActive(false);
        InvitationUI.SetActive(false);
        ConfirmUI.SetActive(false);
        //SucessPopUI.SetActive(false);

        HandShakeImage.gameObject.SetActive(false);

        Talking.onClick.AddListener(InvitationPopUI);
        Yes.onClick.AddListener(AcceptButton);
        No.onClick.AddListener(RefuseButton);
        HandShakeImage.onClick.AddListener(DialogPopUI);
        // confirm okay ��ư ������ �� ���濡�� �޼��� ����
    }

    private void Update()
    {
        //ConnectionVoice();
    }

    [PunRPC]
    public void DialogPopUI()
    {
        DialogUI.SetActive(true);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine == false) return;

        if (other.gameObject.tag == "Player")
        {
            InvitationVoiceTalkUI talkUI = other.gameObject.GetComponent<InvitationVoiceTalkUI>();

            if (talkUI != null)
            {
                talkUI.GetHandShakeImage.SetActive(true);
            }
            else
            {
                Debug.Log("�� Null�� �޾ƶ�~");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (photonView.IsMine == false) return;

        if (other.gameObject.tag == "Player")
        {
            InvitationVoiceTalkUI talkUI = other.gameObject.GetComponent<InvitationVoiceTalkUI>();

            if (talkUI != null)
            {
                talkUI.GetHandShakeImage.SetActive(false);
            }
            else
            {
                Debug.Log("�� Null�� �޾ƶ�~");
            }
        }
    }

    [PunRPC]
    public void InvitationPopUI()
    {
        InvitationUI.SetActive(true);
    }
    [PunRPC]
    public void AcceptButton()
    {
        InvitationUI.SetActive(false);
        ConfirmUI.SetActive(true);
      //  SucessPopUI.SetActive(true);
    }

    [PunRPC]
    public void RefuseButton()
    {     
        InvitationUI.SetActive(false);
    }
}


