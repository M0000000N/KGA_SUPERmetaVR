using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice;
using Photon.Voice.PUN;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;


public class InteractionVoiceUI : MonoBehaviourPunCallbacks
{
    public GameObject GetHandShakeImage => InteractionTalking.gameObject;
    public GameObject GetDialog {  get { return SpeechBubble.gameObject;  } }
    public GameObject GetTalkingButton { get { return GetTalkingButton.gameObject; } }

    [Header("��ȣ�ۿ� ���� ����")]
    [SerializeField] GameObject SpeechBubble;
    [SerializeField] Button InteractionTalking;
    [SerializeField] Button TalkingTogether;
    [SerializeField] Button Exit; // ��ȭ���� ��ư 

    [Header("���̽�ê ��ȣ�ۿ� ����")]
    [SerializeField] private GameObject myVoicepanel;

    [SerializeField] private TextMeshProUGUI TalkingNickName;
    [SerializeField] private TextMeshProUGUI VoicePanel;
    [SerializeField] private TextMeshProUGUI otherVoicePanel;
    [SerializeField] PhotonView photonView;  

    Player otherPlayer;
    VoiceClient voiceClient;

    int actorNumber; // == int channel 
    int ViewID;
    string OtherNickname;
    byte interestGroup;

    private void Start()
    {
        if (photonView.IsMine)
        {
            VoicePanel.text = photonView.Owner.NickName;
            //player = photonView.Owner;         
            // OwnerNickname = photonView.Owner.NickName;
            // myActorNum = photonView.Owner.ActorNumber;
        }
        else
        {
            otherPlayer = photonView.Owner;
             OtherNickname = photonView.Owner.NickName;
           //  otherVoicePanel.text = OtherNickname;
            //  otherActorNum = photonView.Owner.ActorNumber;
        }

        // ����� �ڱ��ڽ� �� 
        int ViewID = photonView.ViewID;

        //Part1.
        InteractionTalking.gameObject.SetActive(false);
        SpeechBubble.SetActive(false);
        myVoicepanel.SetActive(false);
    
        InteractionTalking.onClick.AddListener(() => { DialogPopUI(ViewID, OtherNickname); });
        TalkingTogether.onClick.AddListener(VoiceCanvasPopUI); // 1:1 ��ȭ ��ư ���� 
        Exit.onClick.AddListener(ExitvoiceChannel);

        // UI ��Ȱ��ȭ 
        VoiceInvitationUI.Instance.ClosePopup();
        VoiceConfrimOkayBtn.Instance.ClosePopup();
        VoiceInvitationUI.Instance.TalkingClosePopUp(); 
        VoiceTalkingApprove.Instance.ClosePopup(); 

        // ���ɱ׷��� ���ͳѹ��� ���� 
        PhotonVoiceNetwork.Instance.Client.GlobalInterestGroup = interestGroup;
        interestGroup = 1;
    }

    private void Update() 
    {
        InteractionTalking.transform.forward = Camera.main.transform.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (photonView.IsMine == false) return;
            InteractionVoiceUI talkUI = other.gameObject.GetComponent<InteractionVoiceUI>();

            //player ���� 
            otherPlayer = other.gameObject.GetPhotonView().Owner; // �ε�ģ ���� ����� ����

            if (talkUI != null)
            {               
                talkUI.GetHandShakeImage.SetActive(true);
            }
            else
            {
                return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
        if (photonView.IsMine == false) return;
            InteractionVoiceUI talkUI = other.gameObject.GetComponent<InteractionVoiceUI>();

            Debug.Assert(talkUI != null);

            talkUI.GetDialog.SetActive(false);
            talkUI.GetHandShakeImage.SetActive(false);
        }
    }

    [PunRPC]
    public void DialogPopUI(int _ViewID, string _targetnickname)
    {
        ViewID = _ViewID; // �ٸ� ��� �����ID 
        OtherNickname = _targetnickname;
        SpeechBubble.SetActive(true);
        TalkingNickName.text = _targetnickname;
    }

    public void VoiceCanvasPopUI()
    {
        // �г��� ������ �� �� �� 
        VoiceInvitationUI.Instance.OpenPopup();
        VoiceInvitationUI.Instance.Set(OtherNickname+ "�԰� 1:1 ��ȭ�� �Ͻðڽ��ϱ�?", OnClickYes, OnClickNo);
    }

    public void OnClickYes()
    {
        VoiceInvitationUI.Instance.ClosePopup();
        ConfirmVoicePop();
    }

    public void OnClickNo()
    {
        VoiceInvitationUI.Instance.ClosePopup();
    }

    public void ConfirmVoicePop()
    {
        VoiceConfrimOkayBtn.Instance.OpenPopup();
        VoiceConfrimOkayBtn.Instance.Set(OtherNickname + "�Բ� 1:1 ��ȭ ��ûȮ��", SendRequest);
    }
    //==========��������� �г��� �� �� 

    public void SendRequest()
    {
        photonView.RPC("confrimTalkingCheck", otherPlayer , ViewID, OtherNickname);
    }
 
    [PunRPC]
    public void confrimTalkingCheck(int _viewID, string _targetNickname)
    {
        // ���⼭ ��û�� �г����� ������
         OtherNickname = photonView.Owner.NickName; 
        if (photonView.IsMine)
        {
            OtherNickname = _targetNickname; 
            VoiceInvitationUI.Instance.TalkingOpenPopUp();
            VoiceInvitationUI.Instance.Set(OtherNickname + "�԰� 1:1 ��ȭ�� �Ͻðڽ��ϱ�?", Approve, Reject); 
        }      
    }

    public void Approve()
    {
        PhotonVoiceNetwork.Instance.Client.GlobalInterestGroup = interestGroup;
        interestGroup = 1;
        myVoicepanel.SetActive(true);
        photonView.RPC(nameof(voiceApprove), otherPlayer, actorNumber, interestGroup, true);
    }

    public void Reject()
    {
        photonView.RPC("voiceReject", otherPlayer);
        VoiceTalkingApprove.Instance.Set(OtherNickname + "�Բ��� ��ȭ�� ����");
    }

 // ��ȭ �������� �г��� ���� 
 // ��ȭ ��û�� ����� �г��� �����ϴµ� �� �� �߳� ������ 

    [PunRPC]
    public void voiceApprove(int _ActorNumber, byte _interestGroup, bool _Value)
    {
        if (!photonView.IsMine)
        {
            PhotonVoiceNetwork.Instance.Client.GlobalInterestGroup = interestGroup;
            interestGroup = 1; 
            actorNumber = _ActorNumber;
            interestGroup = _interestGroup;
 
            VoiceTalkingApprove.Instance.OpenPopup();
            VoiceTalkingApprove.Instance.Set(OtherNickname + "�Բ��� ��ȭ�� ����");
        }
            myVoicepanel.SetActive(_Value);
    }

    [PunRPC]
    public void voiceReject()
    {
        if (!photonView.IsMine)
        {
            VoiceTalkingApprove.Instance.OpenPopup();
            VoiceTalkingApprove.Instance.Set(OtherNickname + "�Բ��� ��ȭ�� ����");
        }
    }

    public void ExitvoiceChannel()
    {
        PhotonVoiceNetwork.Instance.Client.GlobalInterestGroup = 0;
    }

   

    //public void OnStateChangeVoiceClient(ClientState fromState, ClientState state)
    //{
    //     if (fromState == ClientState.Joined)
    //        { 
    //            this.voiceClient.onLeaveChannel(voiceChannel);
    //        } 
    //        else if (state == ClientState.Joined)
    //        {
    //            this.voiceClient.onJoinChannel(voiceChannel);
    //            if (this.voiceClient.GlobalInterestGroup != 0)
    //            {
    //                this.LoadBalancingPeer.OpChangeGroups(new byte[0], new byte[] { this.voiceClient.GlobalInterestGroup });
    //            }
    //        }
    //}

}



