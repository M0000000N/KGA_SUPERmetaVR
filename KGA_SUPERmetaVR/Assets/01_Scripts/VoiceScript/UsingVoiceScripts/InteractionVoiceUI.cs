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
    public GameObject GetDialog { get { return SpeechBubble.gameObject; } }
    public GameObject GetTalkingButton { get { return GetTalkingButton.gameObject; } }

    [Header("상호작용 범위 감지")]
    [SerializeField] GameObject SpeechBubble;
    [SerializeField] Button InteractionTalking;
    [SerializeField] Button TalkingTogether;
    [SerializeField] Button Exit; // 대화종료 버튼 

    [Header("보이스챗 상호작용 시작")]
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
        VoiceInvitationUI.Instance.ClosePopup();
        VoiceConfrimOkayBtn.Instance.ClosePopup();
        VoiceInvitationUI.Instance.TalkingClosePopUp();
        VoiceTalkingApprove.Instance.ClosePopup();

        // 관심그룹을 액터넘버로 지정 
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

    [PunRPC]
    public void DialogPopUI(int _ViewID, string _targetnickname)
    {
        ViewID = _ViewID; // 다른 사람 포톤뷰ID 
        OtherNickname = _targetnickname;
        SpeechBubble.SetActive(true);
        TalkingNickName.text = _targetnickname;
    }

    public void VoiceCanvasPopUI()
    {
        // 닉네임 데이터 잘 못 들어감 
        VoiceInvitationUI.Instance.OpenPopup();
        VoiceInvitationUI.Instance.Set(OtherNickname + "님과 1:1 대화를 하시겠습니까?", OnClickYes, OnClickNo);
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
        VoiceConfrimOkayBtn.Instance.Set(OtherNickname + "님께 1:1 대화 신청확인", SendRequest);
    }
    //==========여기까지는 닉네임 잘 뜸 

    public void SendRequest()
    {
        photonView.RPC("confrimTalkingCheck", otherPlayer, ViewID, OtherNickname);
    }

    [PunRPC]
    public void confrimTalkingCheck(int _viewID, string _targetNickname)
    {
        // 여기서 요청자 닉네임이 떠야함
        OtherNickname = photonView.Owner.NickName;
        if (photonView.IsMine)
        {
            OtherNickname = _targetNickname;
            VoiceInvitationUI.Instance.TalkingOpenPopUp();
            VoiceInvitationUI.Instance.Set(OtherNickname + "님과 1:1 대화를 하시겠습니까?", Approve, Reject);
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
        VoiceTalkingApprove.Instance.Set(OtherNickname + "님께서 대화를 거절");
    }

    // 대화 참여중인 닉네임 띄우기 
    // 대화 신청한 사람도 패널이 떠야하는데 왜 안 뜨냔 말이지 

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
            VoiceTalkingApprove.Instance.Set(OtherNickname + "님께서 대화를 수락");
        }
        myVoicepanel.SetActive(_Value);
    }

    [PunRPC]
    public void voiceReject()
    {
        if (!photonView.IsMine)
        {
            VoiceTalkingApprove.Instance.OpenPopup();
            VoiceTalkingApprove.Instance.Set(OtherNickname + "님께서 대화를 거절");
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