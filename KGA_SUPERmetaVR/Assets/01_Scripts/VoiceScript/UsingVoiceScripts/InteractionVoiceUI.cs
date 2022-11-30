using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using Photon.Voice;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;


public class InteractionVoiceUI : MonoBehaviourPunCallbacks
{
    public GameObject GetHandShakeImage => InteractionTalking.gameObject;
    public GameObject GetDialog { get { return SpeechBubble.gameObject; } }
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
    // byte interestGroup;

    List<int> channelList = new List<int>();
    int minXhannel = 1;
    int maxChannel = 255;
    int interestGroup;

    private void Start()
    {
        if (photonView.IsMine)
        {
            VoicePanel.text = photonView.Owner.NickName;
        }
        else
        {
            otherPlayer = photonView.Owner;
            OtherNickname = photonView.Owner.NickName;
            //  otherVoicePanel.text = OtherNickname;
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

        // ���̽�ä�� ����  
        // interestGroup = 1;
        CreatVoiceRooomChannel(1, 255);
        interestGroup = CreatVoiceRooomChannel(1, 255);
        PhotonVoiceNetwork.Instance.Client.GlobalInterestGroup = (byte)interestGroup;
    }

    public int CreatVoiceRooomChannel(int _min, int _max)
    {
       interestGroup = Random.Range(_min, _max);
        for (int i = 0; i < _max;)
        {
            if (channelList.Contains(interestGroup))
            {
                interestGroup = Random.Range(_min, _max);
            }
            else
            {
                channelList.Add(interestGroup);
                ++i;
                break;
            }
        }
        return interestGroup;
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
        VoiceInvitationUI.Instance.OpenPopup();
        VoiceInvitationUI.Instance.Set(OtherNickname + "�Կ��� 1:1 ��ȭ�� ��û�Ͻðڽ��ϱ�?", OnClickYes, OnClickNo);
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
        VoiceConfrimOkayBtn.Instance.Set(OtherNickname + "�Կ��� 1:1 ��ȭ�� ��û�Ͽ����ϴ�", SendRequest);
    }
    //==========��������� �г��� �� �� 

    public void SendRequest()
    {
        photonView.RPC("confrimTalkingCheck", otherPlayer, ViewID, OtherNickname);
    }

    [PunRPC]
    public void confrimTalkingCheck(int _viewID, string _targetNickname)
    {
        // ���⼭ ��û�� �г����� ������
        // OtherNickname = photonView.Owner.NickName;
        if (photonView.IsMine)
        {
            OtherNickname = _targetNickname;
            VoiceInvitationUI.Instance.TalkingOpenPopUp();
            VoiceInvitationUI.Instance.Set(OtherNickname + "���� 1:1 ��ȭ�� ��û�Ͽ����ϴ�\n �����Ͻðڽ��ϱ�?", Approve, Reject);
        }
    }

    public void Approve()
    {
        PhotonVoiceNetwork.Instance.Client.GlobalInterestGroup = (byte)interestGroup;
        // interestGroup = 1;
        //interestGroup; 
        myVoicepanel.SetActive(true);
        photonView.RPC(nameof(voiceApprove), otherPlayer, actorNumber, interestGroup, true);
    }

    public void Reject()
    {
        photonView.RPC("voiceReject", otherPlayer);
        VoiceTalkingApprove.Instance.Set(OtherNickname + "���� 1:1 ��ȭ�� �����Ͽ����ϴ�");
    }

    // ��ȭ �������� �г��� ���� 
    // ��ȭ ��û�� ����� �г��� �����ϴµ� �� �� �߳� ������ 

    [PunRPC]
    public void voiceApprove(int _ActorNumber, byte _interestGroup, bool _Value)
    {
        if (!photonView.IsMine)
        {
            PhotonVoiceNetwork.Instance.Client.GlobalInterestGroup = (byte)interestGroup;
            //interestGroup = 1;
            actorNumber = _ActorNumber;
            interestGroup = _interestGroup;

            VoiceTalkingApprove.Instance.OpenPopup();
            VoiceTalkingApprove.Instance.Set(OtherNickname + "���� 1:1 ��ȭ ��û�� �ź��Ͽ����ϴ�");
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