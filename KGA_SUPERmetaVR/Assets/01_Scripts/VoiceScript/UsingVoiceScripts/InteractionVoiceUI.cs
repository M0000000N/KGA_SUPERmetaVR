using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice;
using Photon.Voice.PUN;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using static DebugUIBuilder;

public class InteractionVoiceUI : MonoBehaviourPunCallbacks
{
    public GameObject GetHandShakeImage => InteractionTalking.gameObject;
    public GameObject GetDialog {  get { return SpeechBubble.gameObject;  } }
    public GameObject GetTalkingButton { get { return GetTalkingButton.gameObject; } }

    // ���� ����, ���� ����
    public Player GetotherPlayerPhotonview { get { return otherPlayer; } }

    [Header("��ȣ�ۿ� ���� ����")]
    [SerializeField] GameObject SpeechBubble;
    [SerializeField] Button InteractionTalking;
    [SerializeField] Button TalkingTogether;

    [Header("���̽�ê ��ȣ�ۿ� ����")]
    [SerializeField] private GameObject myVoicepanel;

    [SerializeField] private TextMeshProUGUI TalkingNickName; 
    [SerializeField] PhotonView photonView;  
    private PhotonView otherPhotonview; 

    string OtherNickname;

    Player player; 
    Player otherPlayer;
    
    private PhotonVoiceNetwork voiceNetwork;

    VoiceClient voiceClient; 
    int myViewID; 
    int voiceChannel;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        voiceNetwork = GetComponent<PhotonVoiceNetwork>();
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            player = photonView.Owner;         
           // OwnerNickname = photonView.Owner.NickName;
           // myActorNum = photonView.Owner.ActorNumber;
        }
        else
        {
            otherPlayer = photonView.Owner;
            OtherNickname = photonView.Owner.NickName;
          //  otherActorNum = photonView.Owner.ActorNumber;
            if (OtherNickname.Equals(null))
                return;
        }
        //Part1.
        InteractionTalking.gameObject.SetActive(false);
        SpeechBubble.SetActive(false);
        myVoicepanel.SetActive(false);
    
        InteractionTalking.onClick.AddListener(() => { DialogPopUI(photonView); });
        TalkingTogether.onClick.AddListener(VoiceCanvasPopUI); // 1:1 ��ȭ ��ư ���� 

        VoiceInvitationUI.Instance.gameObject.SetActive(false);
        VoiceConfrimOkayBtn.Instance.gameObject.SetActive(false);
        VoiceTalkingCheckUI.Instance.gameObject.SetActive(false);
        VoiceTalkingApprove.Instance.gameObject.SetActive(false);
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

    private PhotonView clickedUserView = null;

    public void DialogPopUI(PhotonView view)
    {
        clickedUserView = view; // �ٸ� ��� ����� 
        SpeechBubble.SetActive(true);
        TalkingNickName.text = OtherNickname;
    }

    public void VoiceCanvasPopUI()
    {
        VoiceInvitationUI.Instance.gameObject.SetActive(true);
        VoiceInvitationUI.Instance.Set(OtherNickname + "�԰� 1:1 ��ȭ�� �Ͻðڽ��ϱ�?", OnClickYes, OnClickNo);
    }

    void OnClickYes()
    {
        VoiceInvitationUI.Instance.gameObject.SetActive(false);
        ConfirmVoicePop();
    }

    void OnClickNo()
    {
        VoiceInvitationUI.Instance.gameObject.SetActive(false);
    }

    // =============== ����Ȯ���۾� : 1:1 ��ȭ Ȯ�� �˾� �޼����� �� 
    // =============== *�����ؾ� �� �۾� : 
    // ���濡�� ����/���� �޼��� ������ - ����/���� ������ ������ ����/�����ߴٴ� �˾�â �߱� - ������ �� �Ѵ� ���̽��г� ����(�������ɱ׷�)

    // ���濡�� 1:1 ��ȭ��û ���´ٴ� �˾�â Ȱ��ȭ �Լ� 
    public void ConfirmVoicePop()
    {
        VoiceConfrimOkayBtn.Instance.gameObject.SetActive(true);
        VoiceInvitationUI.Instance.Set(OtherNickname + "1:1 ��ȭ Ȯ��", SendRequest);
    }

    // Onclick = sendRequset
    // ��ư�� ���� ���� ���濡�� ��ȭ��û�� ������ �˷��ܰ� ���ÿ� ���濡�� ����/���� �˾�â ������ 
    [PunRPC]
    void SendRequest()
    {
        photonView.RPC("confrimTalkingCheck", otherPlayer, clickedUserView, true);
    }

    // 1:1 ��ȭ�� �� ������ ���� �˾�â - onYes : Approve / onNo : Reject
    // Approve & Reject ��ư�� Ŭ���ϸ� ���� / ���� �˾�â�� ������ 
    [PunRPC]
    public void confrimTalkingCheck(PhotonView view, bool _value)
    {
        clickedUserView = view;

        VoiceTalkingCheckUI.Instance.gameObject.SetActive(_value);
        VoiceTalkingCheckUI.Instance.Set(OtherNickname + "1:1 ��ȭ�� �Ͻðڽ��ϱ�?", Approve, Reject);
    }
    //RPC Ÿ�� �����Ͽ� �Ѹ��� 
    public void TalkingRequest()
    {
        photonView.RPC("ConfirmTalkingCheck", RpcTarget.Others, true);
    }

    // ������ �� �����˾�â ���� �Լ� 
    public void Approve()
    {
        photonView.RPC("voiceApprove", RpcTarget.Others, clickedUserView, true);
    }

    // ������ �� �����˾�â ���� �Լ� 
    public void Reject()
    {
        photonView.RPC("voiceReject", RpcTarget.Others, clickedUserView, true);
    }

    // �����ҽ� ����/�����˾�â ��Ȱ��ȭ - myvoicePanel ���� 
    [PunRPC]
    public void voiceApprove(bool _Value)
    {
        VoiceTalkingApprove.Instance.gameObject.SetActive(!_Value);
        myVoicepanel.SetActive(_Value);
    }

    // �����ҽ� ����/�����˾�â ��Ȱ��ȭ 
    [PunRPC]
    public void voiceReject()
    {
        VoiceTalkingApprove.Instance.gameObject.SetActive(false);
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



