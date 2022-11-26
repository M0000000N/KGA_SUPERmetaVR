using Photon.Pun;
using Photon.Realtime;
using Photon.Voice;
using Photon.Voice.PUN;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class InvitationVoiceTalkUI : MonoBehaviourPunCallbacks
{
    public GameObject GetHandShakeImage => InteractionTalking.gameObject;
    public GameObject GetDialog {  get { return SpeechBubble.gameObject;  } }
    public GameObject GetVoiceTalkingCanvas {  get { return VoiceTalkingCanvas.gameObject; } }

    // ���� ����, ���� ����
    public Player GetotherPlayerPhotonview { get { return otherPlayer; } }

    [Header("��ȣ�ۿ� ���� ����")]
    [SerializeField] GameObject SpeechBubble;
    [SerializeField] Button InteractionTalking;
    [SerializeField] Button TalkingTogether;

    [Header("���̽�ê ��ȣ�ۿ� ����")]
    [SerializeField] private GameObject VoiceTalkingCanvas;
    [SerializeField] private Button ConfirmButton;
    [SerializeField] private Button RejectButton;
    [SerializeField] Button OkayButton;
    [SerializeField] private GameObject VoiceTalkingCheck; 

    [SerializeField] private TextMeshProUGUI TalkingNickName; 
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private TextMeshProUGUI Check_ContentText;
    [SerializeField] PhotonView photonView;  

    string OtherNickname;

    Player player; 
    Player otherPlayer;
    
    private PhotonView otherPhotonview; 
    private PhotonVoiceNetwork voiceNetwork;

    XRRightRaycast XRRightRaycast; 
    GameObject targetObject;

    VoiceClient voiceClient; 
    int myViewID; 
    int voiceChannel;
 
    private void Awake()
    {
        XRRightRaycast = GetComponent<XRRightRaycast>(); 
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
        VoiceTalkingCanvas.SetActive(false);
        OkayButton.gameObject.SetActive(false);
        VoiceTalkingCheck.SetActive(false);

        InteractionTalking.onClick.AddListener(() => { DialogPopUI(photonView); });
        TalkingTogether.onClick.AddListener(VoiceCanvasPopUI);
       // ConfirmButton.onClick.AddListener(AskTalkingConfirm);
        RejectButton.onClick.AddListener(RejectCanvasPopUI);
       //OkayButton.onClick.AddListener(TalkingRequest);

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
            InvitationVoiceTalkUI talkUI = other.gameObject.GetComponent<InvitationVoiceTalkUI>();

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
            InvitationVoiceTalkUI talkUI = other.gameObject.GetComponent<InvitationVoiceTalkUI>();

            Debug.Assert(talkUI != null);

            talkUI.GetDialog.SetActive(false);
            talkUI.GetHandShakeImage.SetActive(false);
        }
    }

    private PhotonView clickedUserView = null;

    public void DialogPopUI(PhotonView view)
    {
        clickedUserView = view;
        SpeechBubble.SetActive(true);
        TalkingNickName.text = OtherNickname;
    }

    public void VoiceCanvasPopUI()
    {
        // VoiceTalkingCanvas.SetActive(true);
        
        contentText.text = clickedUserView.Owner.NickName + "�Կ��� ��ȭ�� ��û�Ͻðڽ��ϱ�";
    }

    public void RejectCanvasPopUI()
    {
        VoiceTalkingCanvas.SetActive(false);
    }
    
    public void onClickDialog()
    {
        
    }

    public void AskTalkingConfirm()
    {
        ConfirmButton.gameObject.SetActive(false);
        RejectButton.gameObject.SetActive(false);
        OkayButton.gameObject.SetActive(true);
        contentText.text = OtherNickname + "�Կ��� 1:1 ��ȭ�� ��û�Ͽ����ϴ�";
        //��û 
    }

    public void TalkingRequest()
    {
        photonView.RPC("ConfirmTalkingCheck", otherPlayer, true);
    }

    [PunRPC]
    public void confrimTalkingCheck(int _ViewID, bool _value)
    {
        myViewID = _ViewID;
        VoiceTalkingCheck.SetActive(_value);
        Check_ContentText.text = otherPlayer.NickName + "ä�η� �����Ͻðڽ��ϱ�??";
    }

    private void OnclickAccept()
    {

    }

    private void OnclickReject()
    {

    }

    [PunRPC]
    public void AcceptTalking(byte numberGruop)
    {
        PhotonVoiceNetwork.Instance.Client.GlobalInterestGroup = numberGruop;

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



