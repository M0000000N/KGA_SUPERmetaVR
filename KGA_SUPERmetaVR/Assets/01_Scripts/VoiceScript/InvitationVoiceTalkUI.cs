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
using UnityEngine.Animations;
using UnityEngine.XR.Interaction.Toolkit;

public class InvitationVoiceTalkUI : MonoBehaviourPun
{
    // 2.5���� �ȿ� ��� �Ǽ� �߱� 
    // 2.5���� ����� �Ǽ� �������� 
    public GameObject GetHandShakeImage { get { return HandShakeImage.gameObject; } }
    public GameObject GetDialog {  get { return DialogUI.gameObject;  } }

    [SerializeField] XRRayInteractor RightRayInteractor;
    private RaycastHit RightRayHit;
    GameObject targetObject;

    [SerializeField] GameObject DialogUI;
    [SerializeField] GameObject InvitationUI;
    [SerializeField] GameObject ConfirmUI;
    [SerializeField] GameObject ConfirmTalkingCheckUI;
   // [SerializeField] GameObject SucessPopUI;

    [SerializeField] Button HandShakeImage;
    [SerializeField] Button Talking;
    [SerializeField] Button Yes;
    [SerializeField] Button No;
    [SerializeField] Button Confirm_Okay;
    [SerializeField] Button ConfirmTalkingCheckUI_Yes;
    [SerializeField] Button ConfirmTalkingCheckUI_No;
    //  [SerializeField] Button Accept_SucessUI; 

    [SerializeField] TextMeshProUGUI text;

    int actNumber;
    string Nickname; 
    PhotonView otherplayer;

    private void Start()
    {     

        // ���� ������ �� ���� 
        HandShakeImage.gameObject.SetActive(false);
        DialogUI.SetActive(false);
        InvitationUI.SetActive(false);
        ConfirmUI.SetActive(false);
        ConfirmTalkingCheckUI.SetActive(false);

        HandShakeImage.onClick.AddListener(DialogPopUI);
        Talking.onClick.AddListener(InvitationPopUI);
        Yes.onClick.AddListener(AcceptButton);
        No.onClick.AddListener(RefuseButton);
        Confirm_Okay.onClick.AddListener(ConfirmTalkingOkay);
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

            // 1:1 ��ȭ����� ���� 
           //  actNumber = other.gameObject.GetComponent<PhotonView>().ViewID;
           //  Nickname = other.gameObject.GetComponent<PhotonView>().Owner.NickName;
            otherplayer = other.gameObject.GetPhotonView();
            Debug.Log(otherplayer);
            Debug.Log(otherplayer.ViewID);

            if (talkUI != null)
            {               
                talkUI.GetHandShakeImage.SetActive(true);
                talkUI.transform.LookAt(this.transform.position);
            }
            else
            {
                return;
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
                talkUI.GetDialog.SetActive(false);
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
        ConfirmUI.SetActive(true); // Ȯ��UIâ ���� - ���濡�� �ʴ��� �������� 
        //  SucessPopUI.SetActive(true);
    }

    [PunRPC]
    public void RefuseButton()
    {     
        InvitationUI.SetActive(false);
    }

    //���� �ʴ��� �������� ���浵 �޾ƾ��� 
    [PunRPC]
    public void ConfirmTalkingOkay()
    {
        // ���濡�� ������ ��
        // ���濡�� �޼����� �ִ´� 

        //Player player;
        // player = RightRayHit.transform.gameObject.GetP
        //player = RightRayHit.collider.gameObject.GetComponentInParent<PhotonView>().Owner;
        // Debug.Log("target : " + player.ActorNumber);

        otherplayer.RPC("ConfirmTalkingCheck", RpcTarget.All, true);

       //hotonView.RPC("ConfirmTalkingCheck", otherplayer);         

    }

    [PunRPC]
    public void ConfirmTalkingCheck(bool value)
    {
        ConfirmTalkingCheckUI.SetActive(value);
    }
}


