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
using Photon.Pun.UtilityScripts;
using static UnityEngine.Rendering.DebugUI;
using Button = UnityEngine.UI.Button;
using System.Linq.Expressions;
using Photon.Voice.PUN.UtilityScripts;

public class InvitationVoiceTalkUI : MonoBehaviourPun
{
   /// [SerializeField] PlayerData_voice playerdatavoice; 
    // 2.5���� �ȿ� ��� �Ǽ� �߱� 
    // 2.5���� ����� �Ǽ� �������� 
    public GameObject GetHandShakeImage { get { return HandShakeImage.gameObject; } }
    public GameObject GetDialog {  get { return DialogUI.gameObject;  } }
    public GameObject GetConfirmTalkingCheckUI { get { return ConfirmTalkingCheckUI.gameObject; } }
    public Player otherplayerInfo { get { return otherPlayer; } }
    public Button GetConfirmTalkingCheckUI_Yes {  get { return ConfirmTalkingCheckUI_Yes; } }
    public Button GetConfirmTalkingCheckUI_No {  get { return ConfirmTalkingCheckUI_No; } }

    [SerializeField] GameObject DialogUI;
    [SerializeField] GameObject InvitationUI;
    [SerializeField] GameObject ConfirmUI;
    [SerializeField] GameObject ConfirmTalkingCheckUI;

    //[Header("BasicBoxMessage")]
    [SerializeField] GameObject TalkingOkayUI;
    [SerializeField] GameObject TalkingNoUI;
    //[SerializeField] GameObject MyVoicePanel;

    [SerializeField] Button HandShakeImage;
    [SerializeField] Button VoiceMute; 
    [SerializeField] Button Talking;
    [SerializeField] Button Yes;
    [SerializeField] Button No;
    [SerializeField] Button Confirm_Okay;
    [SerializeField] Button ConfirmTalkingCheckUI_Yes;
    [SerializeField] Button ConfirmTalkingCheckUI_No;

    [SerializeField] Button Okay_Confirm;
    [SerializeField] Button NO_Confirm;

    Player otherPlayer;
    PhotonView otherPhotonView = null;
    Player player; 
    Speaker speaker;
    PlayerData_voice playervoicedata; 

    int playerActorNumber; 
    int OtherplayeractNumber;
    string Nickname;

    private void Start()
    {

        HandShakeImage.gameObject.SetActive(false);
        DialogUI.SetActive(false);
        InvitationUI.SetActive(false);
        ConfirmUI.SetActive(false);
        ConfirmTalkingCheckUI.SetActive(false);
        TalkingOkayUI.SetActive(false);
        TalkingNoUI.SetActive(false);
        //MyVoicePanel.SetActive(false); 

        HandShakeImage.onClick.AddListener(DialogPopUI);

        Talking.onClick.AddListener(InvitationPopUI);

        Yes.onClick.AddListener(AcceptButton);
        No.onClick.AddListener(RefuseButton);
      
        // �ʴ��� Ȯ�� ��ư Ŭ�� 
        Confirm_Okay.onClick.AddListener(ConfirmTalkingOkay);
        //ConfirmTalkingCheckUI_Yes.onClick.AddListener(CondirmTalk);
        //ConfirmTalkingCheckUI_No.onClick.AddListener();
       // Nickname = photonView.A
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine == false) return;

        if (other.gameObject.tag == "Player")
        {
            InvitationVoiceTalkUI talkUI = other.gameObject.GetComponent<InvitationVoiceTalkUI>();

            //player ���� 
            otherPlayer = other.gameObject.GetPhotonView().Owner; // �ε�ģ ���� ����� ����

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

    public void DialogPopUI()
    {
        DialogUI.SetActive(true);
    }

    // �ʴ��� 
    public void InvitationPopUI()
    {
        InvitationUI.SetActive(true);
    }

    public void AcceptButton()
    {
        InvitationUI.SetActive(false);
        ConfirmUI.SetActive(true); // Ȯ��UIâ ���� - ���濡�� �ʴ��� �������� 
    }

    public void RefuseButton()
    {     
        InvitationUI.SetActive(false);
    }

    //sendRequest 
    // A�� B���� ��û�ϴ� �κ� 
    //���� �ʴ��� �������� ���浵 �޾ƾ���
    public void ConfirmTalkingOkay()
    {
        photonView.RPC("ConfirmTalkingCheck", otherPlayer, otherPlayer.NickName, true);
        Debug.Log(otherPlayer.NickName);
        ConfirmUI.SetActive(false);
    }

    [PunRPC]
    public void ConfirmTalkingCheck(string _nickname, bool _value)
    {
        player.NickName = _nickname;
        ConfirmTalkingCheckUI.SetActive(_value);
    }

    [PunRPC]
    public void checkApproveReject(bool Value)
    {
        if(Value)
        {
            ConfirmTalkingCheckUI_Yes.onClick.AddListener(Approve);
        }
        else
        {
            ConfirmTalkingCheckUI_No.onClick.AddListener(Reject);
        }
    }

    [PunRPC]
    public void Temp(bool isYes)
    {
        if (isYes)
        {
            // ����â ���
            //ConfirmTalkingCheckUI_Yes.onClick.AddListener(ApproveTalking);
            Approve();
        }
        else
        {
            // ����â ���
            //ConfirmTalkingCheckUI_No.onClick.AddListener(RejectTalking);
            Reject();
        }
    }
    
    public void Approve()
    {
        TalkingOkayUI.SetActive(true);
    }

    public void Reject()
    {
        TalkingNoUI.SetActive(false);
    }
    //public void ApproveTalking()
    //{
    //    photonView.RPC("ApproveTalkingMessage", RpcTarget.Others, playerActorNumber, player.NickName); 
    //}

    //public void RejectTalking()
    //{
    //    photonView.RPC("RejectTalkingMessage", RpcTarget.Others, OtherplayeractNumber, otherPlayer.NickName);
    //}

    //[PunRPC]
    //public void ApproveTalkingMessage(int actortNum, int targetNum, bool popUI)
    //{
    //    if(playerActorNumber == actortNum)
    //    {
    //        OtherplayeractNumber = targetNum; 
    //        TalkingOkayUI.SetActive(popUI); 
    //    }

    //}

    //[PunRPC]
    //public void RejectTalkingMessage(int actortNum, bool popUI)
    //{
    //    if(playerActorNumber == actortNum)
    //    {
    //        TalkingNoUI.SetActive(popUI);
    //    }
    //}

}


