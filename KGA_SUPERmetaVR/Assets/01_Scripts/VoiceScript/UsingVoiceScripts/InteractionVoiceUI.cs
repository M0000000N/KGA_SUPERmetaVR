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

    [Header("상호작용 범위 감지")]
    [SerializeField] GameObject SpeechBubble;
    [SerializeField] Button InteractionTalking;
    [SerializeField] Button TalkingTogether;
    [SerializeField] Button Exit; // 대화종료 버튼 

    [Header("보이스챗 상호작용 시작")]
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

        // 포톤뷰 자기자신 것 
        int ViewID = photonView.ViewID;

        //Part1.
        InteractionTalking.gameObject.SetActive(false);
        SpeechBubble.SetActive(false);
        myVoicepanel.SetActive(false);
    
        InteractionTalking.onClick.AddListener(() => { DialogPopUI(ViewID, OtherNickname); });
        TalkingTogether.onClick.AddListener(VoiceCanvasPopUI); // 1:1 대화 버튼 누름 
        Exit.onClick.AddListener(ExitvoiceChannel);

        // UI 비활성화 
        VoiceInvitationUI.Instance.GetVoiceInvitatinoUI.SetActive(false);
        VoiceTalkingCheckUI.Instance.GetVoiceInvitatinoUI.SetActive(false);
        VoiceConfrimOkayBtn.Instance.GetVoiceComfirm.SetActive(false); 
        VoiceTalkingApprove.Instance.GetVoiceApprove.SetActive(false);

        // 관심그룹을 액터넘버로 지정 
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

    public void DialogPopUI(int _ViewID, string _targetnickname)
    {
        ViewID = _ViewID; // 다른 사람 포톤뷰ID 
        OtherNickname = _targetnickname;
        SpeechBubble.SetActive(true);
        TalkingNickName.text = _targetnickname;
    }

    public void VoiceCanvasPopUI()
    {
        VoiceInvitationUI.Instance.GetVoiceInvitatinoUI.SetActive(true);
        VoiceInvitationUI.Instance.Set(OtherNickname + "님과 1:1 대화를 하시겠습니까?", OnClickYes, OnClickNo);
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
        VoiceInvitationUI.Instance.Set(OtherNickname + "님께 1:1 대화 신청확인", SendRequest);
    }

    // Onclick = sendRequset
    // 버튼을 눌러 내가 상대방에게 대화신청을 했음을 알려줌과 동시에 상대방에게 수락/거절 팝업창 보내기 
    void SendRequest()
    {
        photonView.RPC("confrimTalkingCheck", otherPlayer, ViewID, true);
    }
    // 1:1 대화를 할 것인지 묻는 팝업창 - onYes : Approve / onNo : Reject
    // Approve & Reject 버튼을 클릭하면 수락 / 거절 팝업창이 떠야함 
    [PunRPC]
    public void confrimTalkingCheck(int _viewID, bool _value)
    {
        ViewID = _viewID;

        if (ViewID == _viewID)
        {
            VoiceTalkingCheckUI.Instance.GetVoiceInvitatinoUI.SetActive(_value);
            VoiceTalkingCheckUI.Instance.Set(OtherNickname + "1:1 대화를 하시겠습니까?", Approve, Reject);
        }
    }

    // 수락할 시 수락팝압창 띄우는 함수 
    public void Approve()
    {
        photonView.RPC(nameof(voiceApprove), PhotonNetwork.PlayerList[actorNumber-1], ViewID, interestGroup,true); 
       // photonView.RPC("voiceApprove", RpcTarget.All, ViewID, interestGroup, true);
        VoiceTalkingApprove.Instance.Set(OtherNickname + "님께서 대화를 수락");
    }

    // 거절할 시 거절팝압창 띄우는 함수 
    public void Reject()
    {
        photonView.RPC("voiceReject", RpcTarget.All, ViewID, true);
        VoiceTalkingApprove.Instance.Set(OtherNickname + "님께서 대화를 거절");
    }

    // 수락할시 수락/거절팝업창 비활성화 - myvoicePanel 띄우기
    [PunRPC]
    public void voiceApprove(int _ViewID, int _ActorNumber, byte _interestGroup, bool _Value)
    {
        // 정보전달 
        actorNumber = _ActorNumber; 
        interestGroup = _interestGroup;
        _interestGroup = (byte)(_ActorNumber + actorNumber); 
        ViewID = _ViewID; 

        VoiceTalkingApprove.Instance.gameObject.SetActive(!_Value);
        myVoicepanel.SetActive(_Value);
    }

    // 거절할시 수락/거절팝업창 비활성화 
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



