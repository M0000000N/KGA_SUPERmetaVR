//using Assets.OVR.Scripts;
//using Facebook.WitAi.Events;
//using Photon.Pun;
//using Photon.Realtime;
//using Photon.Voice.PUN;
//using Photon.Voice.PUN.UtilityScripts;
//using Photon.Voice.Unity;
//using TMPro;
//using UnityEngine;
//using UnityEngine.Assertions.Must;
//using UnityEngine.UI;

//public class GetInvitationTalkUI : MonoBehaviourPun
//{
//    string OtherNickname;
//    private PhotonView clickedUserView = null;
//    InteractionVoiceUI interaction; 

//    private void Awake()
//    {
//        VoiceInvitationUI.Instance.gameObject.SetActive(false);
//        VoiceTalkingCheckUI.Instance.gameObject.SetActive(false);
//        VoiceTalkingApprove.Instance.gameObject.SetActive(false);
//    }

//    private void Start()
//    {
//        if (photonView.IsMine)
//        {
//           // player = photonView.Owner;
//            // OwnerNickname = photonView.Owner.NickName;
//            // myActorNum = photonView.Owner.ActorNumber;
//        }
//        else
//        {
//            //otherPlayer = photonView.Owner;
//            OtherNickname = photonView.Owner.NickName;
//            //  otherActorNum = photonView.Owner.ActorNumber;
//            if (OtherNickname.Equals(null))
//                return;
//        }

//        interaction.GetTalkingButton.GetComponent<Button>().onClick.AddListener(VoiceCanvasPopUI);

//    }

//    public void VoiceCanvasPopUI()
//    {
//        VoiceInvitationUI.Instance.gameObject.SetActive(true);
//        VoiceInvitationUI.Instance.Set(OtherNickname + "1:1 ��ȭ�� �Ͻðڽ��ϱ�?", OnClickYes, OnClickNo);
//    }

//    void OnClickYes()
//    {
//        VoiceInvitationUI.Instance.gameObject.SetActive(false);
//        VoiceInvitationUI.Instance.gameObject.SetActive(true);
//        //ConfirmCanvasUI(); 
//    }

//    void OnClickNo()
//    {
//        VoiceInvitationUI.Instance.gameObject.SetActive(false);
//    }

//    public void ConfirmCanvasUI()
//    {
//        //  VoiceInvitationUI.Instance.gameObject.SetActive(true);
//        VoiceInvitationUI.Instance.Set(OtherNickname + "��ȭ ��û �Ϸ�", SendRequest);
//    }

//    [PunRPC]
//    void SendRequest()
//    {
//        photonView.RPC("confrimTalkingCheck", RpcTarget.Others, clickedUserView, true);
//        //���濡�� ��ȭ��û�Ϸᰡ �� �� �˷���� �� 
//        // ���⼭ ���� ���� ��ģ �� ������� ���� 
//    }

//    //public void TalkingRequest()
//    //{
//    //    photonView.RPC("ConfirmTalkingCheck", RpcTarget.Others, true);
//    //}



//    [PunRPC]
//    public void confrimTalkingCheck(PhotonView view, bool _value)
//    {
//        clickedUserView = view;

//        VoiceTalkingCheckUI.Instance.gameObject.SetActive(_value);
//        VoiceTalkingCheckUI.Instance.Set(OtherNickname + "1:1 ��ȭ�� �Ͻðڽ��ϱ�?", Approve, Reject);
//    }

//    public void Approve()
//    {
//        photonView.RPC("voiceApprove", RpcTarget.Others, clickedUserView, true);
//    }

//    public void Reject()
//    {
//        photonView.RPC("voiceReject", RpcTarget.Others, clickedUserView, true);
//    }

//    [PunRPC]
//    public void voiceApprove(bool _Value)
//    {
//        VoiceTalkingApprove.Instance.gameObject.SetActive(!_Value);
//      //myVoicepanel.SetActive(_Value);
//    }

//    [PunRPC]
//    public void voiceReject()
//    {
//        VoiceTalkingApprove.Instance.gameObject.SetActive(false);
//    }



//}
