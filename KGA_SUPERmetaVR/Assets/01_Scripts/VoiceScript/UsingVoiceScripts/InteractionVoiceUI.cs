using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice;
using Photon.Voice.PUN;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using WebSocketSharp;
using static DebugUIBuilder;

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
    [SerializeField] PhotonView photonView;  

    Player otherPlayer;
    PhotonVoiceNetwork voiceNetwork;
    PhotonView clickedUserView = null;
    VoiceClient voiceClient;

    int actorNumber; // == int channel 
    int ViewID;
    string OtherNickname;
    byte interestGroup;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        voiceNetwork = GetComponent<PhotonVoiceNetwork>();
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            //player = photonView.Owner;         
           // OwnerNickname = photonView.Owner.NickName;
           // myActorNum = photonView.Owner.ActorNumber;
        }
        else
        {
            otherPlayer = photonView.Owner;
            OtherNickname = photonView.Owner.NickName;
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
        VoiceInvitationUI.Instance.GetVoiceInvitatinoUI.SetActive(false);
        VoiceTalkingCheckUI.Instance.GetVoiceInvitatinoUI.SetActive(false);
        VoiceConfrimOkayBtn.Instance.GetVoiceComfirm.SetActive(false); 
        VoiceTalkingApprove.Instance.GetVoiceApprove.SetActive(false);

        // ���ɱ׷��� ���ͳѹ��� ���� 
        PhotonVoiceNetwork.Instance.Client.GlobalInterestGroup = interestGroup;
        interestGroup = (byte)photonView.Owner.ActorNumber;
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

    public void DialogPopUI(int _ViewID, string _targetnickname)
    {
        ViewID = _ViewID; // �ٸ� ��� �����ID 
        OtherNickname = _targetnickname;
        SpeechBubble.SetActive(true);
        TalkingNickName.text = _targetnickname;
    }

    public void VoiceCanvasPopUI()
    {
        VoiceInvitationUI.Instance.GetVoiceInvitatinoUI.SetActive(true);
        VoiceInvitationUI.Instance.Set(OtherNickname + "�԰� 1:1 ��ȭ�� �Ͻðڽ��ϱ�?", OnClickYes, OnClickNo);
    }

    void OnClickYes()
    {
        VoiceInvitationUI.Instance.GetVoiceInvitatinoUI.SetActive(false);
        ConfirmVoicePop();
    }

    void OnClickNo()
    {
        VoiceInvitationUI.Instance.GetVoiceInvitatinoUI.SetActive(false);
    }

    public void ConfirmVoicePop()
    {
        VoiceConfrimOkayBtn.Instance.GetVoiceComfirm.SetActive(true);
        VoiceInvitationUI.Instance.Set(OtherNickname + "�Բ� 1:1 ��ȭ ��ûȮ��", SendRequest);
    }

    // Onclick = sendRequset
    // ��ư�� ���� ���� ���濡�� ��ȭ��û�� ������ �˷��ܰ� ���ÿ� ���濡�� ����/���� �˾�â ������ 
    void SendRequest()
    {
        photonView.RPC("confrimTalkingCheck", otherPlayer, ViewID, true);
    }
    // 1:1 ��ȭ�� �� ������ ���� �˾�â - onYes : Approve / onNo : Reject
    // Approve & Reject ��ư�� Ŭ���ϸ� ���� / ���� �˾�â�� ������ 
    [PunRPC]
    public void confrimTalkingCheck(int _viewID, bool _value)
    {
        ViewID = _viewID;

        if (ViewID == _viewID)
        {
            VoiceTalkingCheckUI.Instance.GetVoiceInvitatinoUI.SetActive(_value);
            VoiceTalkingCheckUI.Instance.Set(OtherNickname + "1:1 ��ȭ�� �Ͻðڽ��ϱ�?", Approve, Reject);
        }
    }

    // ������ �� �����˾�â ���� �Լ� 
    public void Approve()
    {
        photonView.RPC(nameof(voiceApprove), PhotonNetwork.PlayerList[actorNumber-1], ViewID, interestGroup,true); 
       // photonView.RPC("voiceApprove", RpcTarget.All, ViewID, interestGroup, true);
        VoiceTalkingApprove.Instance.Set(OtherNickname + "�Բ��� ��ȭ�� ����");
    }

    // ������ �� �����˾�â ���� �Լ� 
    public void Reject()
    {
        photonView.RPC("voiceReject", RpcTarget.All, ViewID, true);
        VoiceTalkingApprove.Instance.Set(OtherNickname + "�Բ��� ��ȭ�� ����");
    }

    // �����ҽ� ����/�����˾�â ��Ȱ��ȭ - myvoicePanel ����
    [PunRPC]
    public void voiceApprove(int _ViewID, int _ActorNumber, byte _interestGroup, bool _Value)
    {
        // �������� 
        actorNumber = _ActorNumber; 
        interestGroup = _interestGroup;
        _interestGroup = (byte)(_ActorNumber + actorNumber); 
        ViewID = _ViewID; 

        VoiceTalkingApprove.Instance.gameObject.SetActive(!_Value);
        myVoicepanel.SetActive(_Value);
    }

    // �����ҽ� ����/�����˾�â ��Ȱ��ȭ 
    [PunRPC]
    public void voiceReject(int _ViewID, bool _Value)
    {
        ViewID= _ViewID;
        VoiceTalkingApprove.Instance.gameObject.SetActive(_Value);
    }

    public void ExitvoiceChannel()
    {
        PhotonVoiceNetwork.Instance.Client.GlobalInterestGroup = 0;
    }

    //[PunRPC]
    //public void AcceptTalking(byte numberGruop)
    //{
    //    PhotonVoiceNetwork.Instance.Client.GlobalInterestGroup = numberGruop;

    //}

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



