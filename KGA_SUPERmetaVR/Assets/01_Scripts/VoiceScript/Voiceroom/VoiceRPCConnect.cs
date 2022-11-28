using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VoiceRPCConnect : MonoBehaviourPunCallbacks/*, IPunObservable*/
{
    [SerializeField]
    VoiceManager voiceManager;

    //[PunRPC]
    //public void _ReceiveMessage(string type, string valueText)
    //{
    //    voiceManager.RPCTypeProcess(type, valueText);
    //}

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    //throw new System.NotImplementedException();
    //}
}
