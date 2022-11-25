using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VoiceManager : MonoBehaviourPun, IPunObservable
{
    [SerializeField]
    GetInvitationTalkUI GetInvitation; 

    [SerializeField]
    BasicMessageBox singleChatMessageBox;

    string Value;
    string Nickname; 

    private void Start()
    {
        Value = photonView.Owner.ActorNumber.ToString(); 

        if(photonView.IsMine.Equals(false))
        {
            Nickname = photonView.Owner.NickName; 
        }
        
    }

    //public void RPCTypeProcess(string type, int value)
    //{
    //    switch (type)
    //    {
    //        case "Chat":
    //            ShowVoiceChatMessage(value);
    //            break;
    //    }
    //}

    //void ShowVoiceChatMessage(int Value)
    //{
    //    string contentText = Nickname + "채널로 입장하시겠습니까??";
    //    singleChatMessageBox.SetBtn(() => { GetInvitation.ChangedAudioGroup(Value); }, () => { }, contentText);

    //}

    //[PunRPC]
    //public void _ReceiveMessage(string type, int value)
    //{
    //    RPCTypeProcess(type, value);
    //}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // throw new System.NotImplementedException();
    }
}
