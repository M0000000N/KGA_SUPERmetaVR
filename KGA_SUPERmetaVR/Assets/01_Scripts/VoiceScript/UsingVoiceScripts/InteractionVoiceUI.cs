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
        VoiceInvitationUI.Instance.Set(OtherNickname + "님과 1:1 대화를 하시겠습니까?", OnClickYes, OnClickNo);
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

    // =============== 구현확인작업 : 1:1 대화 확인 팝업 메세지는 뜸 
    // =============== *구현해야 할 작업 : 
    // 상대방에게 수락/거절 메세지 보내기 - 수락/거절 누르면 나에게 수락/거절했다는 팝업창 뜨기 - 수락할 시 둘다 보이스패널 띄우기(같은관심그룹)

    // 상대방에게 1:1 대화신청 보냈다는 팝업창 활성화 함수 
    public void ConfirmVoicePop()
    {
        VoiceConfrimOkayBtn.Instance.gameObject.SetActive(true);
        VoiceInvitationUI.Instance.Set(OtherNickname + "1:1 대화 확인", SendRequest);
    }

    // Onclick = sendRequset
    // 버튼을 눌러 내가 상대방에게 대화신청을 했음을 알려줌과 동시에 상대방에게 수락/거절 팝업창 보내기 
    [PunRPC]
    void SendRequest()
    {
        photonView.RPC("confrimTalkingCheck", otherPlayer, clickedUserView, true);
    }

    // 1:1 대화를 할 것인지 묻는 팝업창 - onYes : Approve / onNo : Reject
    // Approve & Reject 버튼을 클릭하면 수락 / 거절 팝업창이 떠야함 
    [PunRPC]
    public void confrimTalkingCheck(PhotonView view, bool _value)
    {
        clickedUserView = view;

        VoiceTalkingCheckUI.Instance.gameObject.SetActive(_value);
        VoiceTalkingCheckUI.Instance.Set(OtherNickname + "1:1 대화를 하시겠습니까?", Approve, Reject);
    }
    //RPC 타켓 선정하여 뿌리기 
    public void TalkingRequest()
    {
        photonView.RPC("ConfirmTalkingCheck", RpcTarget.Others, true);
    }

    // 수락할 시 수락팝압창 띄우는 함수 
    public void Approve()
    {
        photonView.RPC("voiceApprove", RpcTarget.Others, clickedUserView, true);
    }

    // 거절할 시 거절팝압창 띄우는 함수 
    public void Reject()
    {
        photonView.RPC("voiceReject", RpcTarget.Others, clickedUserView, true);
    }

    // 수락할시 수락/거절팝업창 비활성화 - myvoicePanel 띄우기 
    [PunRPC]
    public void voiceApprove(bool _Value)
    {
        VoiceTalkingApprove.Instance.gameObject.SetActive(!_Value);
        myVoicepanel.SetActive(_Value);
    }

    // 거절할시 수락/거절팝업창 비활성화 
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



