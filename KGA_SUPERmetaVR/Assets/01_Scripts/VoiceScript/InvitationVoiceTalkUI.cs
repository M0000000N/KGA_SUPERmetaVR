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
    // 2.5미터 안에 들면 악수 뜨기 
    // 2.5미터 벗어나면 악수 없어지기 
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
      
        // 초대장 확인 버튼 클릭 
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

            //player 정보 
            otherPlayer = other.gameObject.GetPhotonView().Owner; // 부딪친 상대방 포톤뷰 정보

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
                Debug.Log("야 Null값 받아라~");
            }
        }
    }

    public void DialogPopUI()
    {
        DialogUI.SetActive(true);
    }

    // 초대장 
    public void InvitationPopUI()
    {
        InvitationUI.SetActive(true);
    }

    public void AcceptButton()
    {
        InvitationUI.SetActive(false);
        ConfirmUI.SetActive(true); // 확인UI창 띄우기 - 상대방에게 초대장 보내야함 
    }

    public void RefuseButton()
    {     
        InvitationUI.SetActive(false);
    }

    //sendRequest 
    // A가 B에게 요청하는 부분 
    //내가 초대장 보냈으니 상대방도 받아야함
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
            // 수락창 띄움
            //ConfirmTalkingCheckUI_Yes.onClick.AddListener(ApproveTalking);
            Approve();
        }
        else
        {
            // 거절창 띄움
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


