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
 //   public GameObject GetVoiceTalkingCanvas {  get { return VoiceTalkingCanvas.gameObject; } }

    // ���� ����, ���� ����
    public Player GetotherPlayerPhotonview { get { return otherPlayer; } }

    [Header("��ȣ�ۿ� ���� ����")]
    [SerializeField] GameObject SpeechBubble;
    [SerializeField] Button InteractionTalking;
    [SerializeField] Button TalkingTogether;

    [Header("���̽�ê ��ȣ�ۿ� ����")]
 //   [SerializeField] private GameObject VoiceTalkingCanvas;
    [SerializeField] private Button ConfirmButton;
    [SerializeField] private Button RejectButton;
    [SerializeField] Button OkayButton;
   // [SerializeField] private GameObject VoiceTalkingCheck;
    [SerializeField] private GameObject myVoicepanel;
  //  [SerializeField] private GameObject ApproveReject; 


    [SerializeField] private TextMeshProUGUI TalkingNickName; 
   // [SerializeField] private TextMeshProUGUI contentText;
    //[SerializeField] private TextMeshProUGUI Check_ContentText;
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

        //Part2. - ��û�� 
       // VoiceTalkingCanvas.SetActive(false);
        OkayButton.gameObject.SetActive(false);

        VoiceInvtationUI.Instance.gameObject.SetActive(false);
        VoiceTalkingCheckUI.Instance.gameObject.SetActive(false);
        VoiceTalkingApprove.Instance.gameObject.SetActive(false);

       // VoiceTalkingCheck.SetActive(false);
        myVoicepanel.SetActive(false);
      //  ApproveReject.SetActive(false);

        InteractionTalking.onClick.AddListener(() => { DialogPopUI(photonView); });
        TalkingTogether.onClick.AddListener(VoiceCanvasPopUI); // 1:1 ��ȭ ��ư ���� 
      //  check_ConfirmBtn.onClick.AddListener(othervoiceChannel);
      //  OkayButton.onClick.AddListener(TalkingRequest);
       // ConfirmButton.onClick.AddListener(AskTalkingConfirm);
       //  RejectButton.onClick.AddListener(RejectCanvasPopUI);
      // OkayButton.onClick.AddListener(TalkingRequest);

    }

    private void Update() 
    {
        InteractionTalking.transform.forward = Camera.main.transform.forward;

        if (XRRightRaycast.Instance.InteractCharacter() != null)
        {
            if (XRRightRaycast.Instance.InteractCharacter().tag.Equals("Player"))
            {
                otherPhotonview = XRRightRaycast.Instance.InteractCharacter().gameObject.GetComponentInParent<PhotonView>();
                Debug.Log(otherPhotonview.Owner.NickName); 
            }
        }
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
        VoiceInvtationUI.Instance.gameObject.SetActive(true);
        VoiceInvtationUI.Instance.Set(OtherNickname + "1:1 ��ȭ�� �Ͻðڽ��ϱ�?", OnClickYes, OnClickNo);
    }

    void OnClickYes()
    {
        VoiceInvtationUI.Instance.gameObject.SetActive(false);
        ConfirmCanvasUI(); 
    }

    void OnClickNo()
    {
        VoiceInvtationUI.Instance.gameObject.SetActive(false);
    }

    public void ConfirmCanvasUI()
    {
        VoiceInvtationUI.Instance.gameObject.SetActive(true);
        VoiceInvtationUI.Instance.Set(OtherNickname + "��ȭ ��û �Ϸ�", OnClickOkay);
    }

    void OnClickOkay()
    {
       //ingRequest();
        VoiceInvtationUI.Instance.gameObject.SetActive(false);
        //���濡�� ��ȭ��û�Ϸᰡ �� �� �˷���� �� 
    }
  
    public void TalkingRequest()
    {
        photonView.RPC("ConfirmTalkingCheck", RpcTarget.Others, true);
    }

    [PunRPC]
    public void confrimTalkingCheck(PhotonView view, bool _value)
    {
        clickedUserView = view;

        VoiceTalkingCheckUI.Instance.gameObject.SetActive(_value);
        VoiceTalkingCheckUI.Instance.Set(OtherNickname + "1:1 ��ȭ�� �Ͻðڽ��ϱ�?", Approve, Reject);
    }

    public void Approve()
    {
        photonView.RPC("voiceApprove", RpcTarget.Others, clickedUserView, true);
    }

    public void Reject()
    {
        photonView.RPC("voiceReject", RpcTarget.Others, clickedUserView, true);
    }

    [PunRPC]
    public void voiceApprove()
    { 
        VoiceTalkingApprove.Instance.gameObject.SetActive(false);
        myVoicepanel.SetActive(true);
    }

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



