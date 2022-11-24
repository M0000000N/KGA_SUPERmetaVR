using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;
using Photon.Pun;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class VoiceManager : MonoBehaviourPun
{
    [SerializeField]
    VoiceChatControll chatControll;

    [SerializeField]
    BasicMessageBox singleChatMessageBox;

    private void Start()
    {
        chatControll.SetVoiceDetected(true);
    }

    public void RPCTypeProcess(string type, string value)
    {
        switch (type)
        {
            case "Chat":
                ShowVoiceChatMessage(value);
                break;
        }
    }

    void ShowVoiceChatMessage(string Nickname)
    {
        string contentText = Nickname + "ä�η� �����Ͻðڽ��ϱ�??";
      //  singleChatMessageBox.SetBtn(() => { chatControll.ChangedAudioGroup(Nickname); }, () => { }, contentText);

    }
}
