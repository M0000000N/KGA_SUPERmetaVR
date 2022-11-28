using Assets.OVR.Scripts;
using Facebook.WitAi.Events;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using Photon.Voice.PUN.UtilityScripts;
using Photon.Voice.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class GetInvitationTalkUI : MonoBehaviourPun
{
   // VoiceroomManager voiceroomManager;
    [SerializeField] InteractionVoiceUI invitation;
    [SerializeField] BasicMessageBox messageBox;
    [SerializeField] Button OkayButton;

    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private GameObject VoiceTalkingCheck;
    [SerializeField] private Button Approve;
    [SerializeField] private Button Reject;

    private PhotonView photonView;
    private PhotonVoiceNetwork voiceNetwork;
  //  Recorder recorder;
    Player otherPlayer;

    string myNickname;
    string otherNickname;

    int value;

    //상대방 플레이어가 okay 버튼 누르면 실행 
    private void Start()
    {
        photonView = GetComponent<PhotonView>();
     //   recorder = voiceroomManager.recorder;

        otherPlayer = invitation.GetotherPlayerPhotonview;
        // otherNickname = invitation.GetotherPlayerPhotonview.NickName;
        VoiceTalkingCheck.SetActive(false);
       // OkayButton.onClick.AddListener(TalkingRequest);
    }

    //public void TalkingRequest()
    //{
    //    photonView.RPC("ConfirmTalkingCheck", otherPlayer, photonView.OwnerActorNr, true);
    //}

    //[PunRPC]
    //public void confrimTalkingCheck(Player player, int _actorNum, bool _value)
    //{
    //    if(photonView.Owner.NickName != otherNickname)
    //    {
    //        VoiceTalkingCheck.SetActive(_value);
    //        //  contentText.text = otherNickname + "1:1 Approve Or Reject?";
    //        string contentText = otherPlayer.NickName + "채널로 입장하시겠습니까??";
    //        messageBox.SetBtn(() => { ChangeAudioGroup(value); }, () => { }, contentText);
    //    }
    //}

    //public void ShowMessageBox()
    //{
    //    string contentText = otherPlayer.NickName + "채널로 입장하시겠습니까??";
    //    messageBox.SetBtn(() => { ChangeAudioGroup(value); }, () => { }, contentText);
    //}

    //[PunRPC]
    //public void RecieveMessage()
    //{
    //    ShowMessageBox();
    //}

    //Approve 확인버튼 눌렀을 때 보이스 채널 변경 
    //public void ChangeAudioGroup(int value)
    //{
    //    byte _InterestGroup = 0;
    //    if (value != 0)
    //    {
    //        _InterestGroup = (byte)PhotonNetwork.LocalPlayer.ActorNumber;
    //        Debug.Log($"InterestGrup : {_InterestGroup}"); 
    //        //if (_InterestGroup != (byte)(PhotonNetwork.CurrentRoom.PlayerCount))
    //        //{
    //        //    ++_InterestGroup;
    //        //}
    //    }
    //    else
    //        PhotonVoiceNetwork.Instance.Client.GlobalInterestGroup = _InterestGroup;
    //    PhotonVoiceNetwork.Instance.Client.
    //}

    ////수락 버튼 선택 시 보이스채널 변경 및 상대클라이언트에게 전달 
    //public void ChangedAudioGroup(int value)
    //{
    //    byte _InterestGroup = 0;
    //    value = (byte)(PhotonNetwork.CurrentRoom.PlayerCount);
    //    playerCount = value; 

    //    if (value != 0)
    //    {
    //        _InterestGroup = (byte)playerCount; 

    //    }
    //    PhotonVoiceNetwork.Instance.Client.GlobalInterestGroup = _InterestGroup;
    //}
}
