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
    string MyNickname; 
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
            MyNickname = photonView.Owner.NickName;
            VoicePanel.text = photonView.Owner.NickName;
        }
        else
        {
            otherPlayer = photonView.Owner;
            OtherNickname = photonView.Owner.NickName;
            //  otherVoicePanel.text = OtherNickname;
        }

      // otherVoicePanel.text = otherPlayer.NickName; 
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
        CreatVoiceRooomChannel(1, 255);
    }

    //���̽��� ���� 
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

    public void SendRequest()
    {
        photonView.RPC("confrimTalkingCheck", otherPlayer, ViewID, OtherNickname);
    }

    [PunRPC]
    public void confrimTalkingCheck(int _viewID, string _targetNickname)
    {
        if (photonView.IsMine)
        {
            OtherNickname = _targetNickname;
            VoiceInvitationUI.Instance.TalkingOpenPopUp();
            VoiceInvitationUI.Instance.Set(otherPlayer.NickName + "���� 1:1 ��ȭ�� ��û�Ͽ����ϴ�\n �����Ͻðڽ��ϱ�?", Approve, Reject);
        }
    }

    public void Approve()
    {
        myVoicepanel.SetActive(true);
        photonView.RPC(nameof(voiceApprove), otherPlayer, interestGroup, true);
        PhotonVoiceNetwork.Instance.Client.GlobalInterestGroup = (byte)interestGroup;
    }

    public void Reject()
    {
        photonView.RPC("voiceReject", otherPlayer);
    }

    [PunRPC]
    public void voiceApprove(int _interestGroup, bool _Value)
    {
        if (!photonView.IsMine)
        {
            //interestGroup = 1;
            interestGroup = CreatVoiceRooomChannel(1, 255);
            interestGroup = (byte)_interestGroup;
            PhotonVoiceNetwork.Instance.Client.GlobalInterestGroup = (byte)interestGroup;

            VoiceTalkingApprove.Instance.OpenPopup();
            VoiceTalkingApprove.Instance.Set(OtherNickname + "���� 1:1 ��ȭ�� �����Ͽ����ϴ�");
            myVoicepanel.SetActive(_Value);
            // ��û�ڿ��� �г��� �����ϴµ� 
        }      
    }

    [PunRPC]
    public void voiceReject()
    {
        if (!photonView.IsMine)
        {
            VoiceTalkingApprove.Instance.OpenPopup();
            VoiceTalkingApprove.Instance.Set(OtherNickname + "���� 1:1 ��ȭ ��û�� �ź��Ͽ����ϴ�");
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