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

    // 나의 정보, 상대방 정보
    public Player GetotherPlayerPhotonview { get { return otherPlayer; } }

    [Header("상호작용 범위 감지")]
    [SerializeField] GameObject SpeechBubble;
    [SerializeField] Button InteractionTalking;
    [SerializeField] Button TalkingTogether;

    [Header("보이스챗 상호작용 시작")]
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
        TalkingTogether.onClick.AddListener(VoiceCanvasPopUI); // 1:1 대화 버튼 누름 

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

            //player 정보 
            otherPlayer = other.gameObject.GetPhotonView().Owner; // 부딪친 상대방 포톤뷰 정보

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
        clickedUserView = view; // 다른 사람 포톤뷰 
        SpeechBubble.SetActive(true);
        TalkingNickName.text = OtherNickname;
    }

    public void VoiceCanvasPopUI()
    {
        VoiceInvitationUI.Instance.gameObject.SetActive(true);
        VoiceInvitationUI.Instance.Set(OtherNickname + "1:1 대화를 하시겠습니까?", OnClickYes, OnClickNo);
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

    public void ConfirmVoicePop()
    {
        VoiceConfrimOkayBtn.Instance.gameObject.SetActive(true);
        VoiceInvitationUI.Instance.Set(OtherNickname + "1:1 대화 확인", OnClickOkay);
    }

    //sendRequest == onOkayclick
    public void OnClickOkay()
    {
       // VoiceInvitationUI.Instance.gameObject.SetActive(true);
       // VoiceInvitationUI.Instance.Set(OtherNickname + "대화 신청 완료", SendRequest);
    }

    [PunRPC]
    void SendRequest()
    {
        photonView.RPC("confrimTalkingCheck", otherPlayer, clickedUserView, true);
        //상대방에게 대화신청완료가 된 걸 알려줘야 함 
        // 여기서 부터 제가 놓친 게 있을까요 센세 
    }

    //public void TalkingRequest()
    //{
    //    photonView.RPC("ConfirmTalkingCheck", RpcTarget.Others, true);
    //}

    [PunRPC]
    public void confrimTalkingCheck(PhotonView view, bool _value)
    {
        clickedUserView = view;

        VoiceTalkingCheckUI.Instance.gameObject.SetActive(_value);
        VoiceTalkingCheckUI.Instance.Set(OtherNickname + "1:1 대화를 하시겠습니까?", Approve, Reject);
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
    public void voiceApprove(bool _Value)
    {
        VoiceTalkingApprove.Instance.gameObject.SetActive(!_Value);
        myVoicepanel.SetActive(_Value);
    }

    //[PunRPC]
    //public void voiceReject()
    //{
    //    VoiceTalkingApprove.Instance.gameObject.SetActive(false);
    //}

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



