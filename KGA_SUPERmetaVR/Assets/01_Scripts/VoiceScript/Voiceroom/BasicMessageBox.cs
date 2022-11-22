using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime; 

public class BasicMessageBox : MonoBehaviourPun 
{
    
    [SerializeField] InvitationVoiceTalkUI voiceUI;

    [Header("BasicBoxMessage")]
    [SerializeField] GameObject TalkingOkayUI;
    [SerializeField] GameObject TalkingNoUI;
    [SerializeField] GameObject MyVoicePanel;

    Button okButton;
    Button cancelButton;
    Text text;

    public void Start()
    {
        TalkingOkayUI.SetActive(false);
        TalkingNoUI.SetActive(false);
    }

    public void Approve()
    {
        photonView.RPC(nameof(voiceUI.Temp), RpcTarget.All, true);
    }

    public void Reject()
    {
        photonView.RPC(nameof(voiceUI.Temp), RpcTarget.All, false);
    }


}
