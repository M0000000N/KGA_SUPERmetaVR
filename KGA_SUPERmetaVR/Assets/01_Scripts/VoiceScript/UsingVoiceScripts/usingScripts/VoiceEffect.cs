using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine.Assertions.Must;


//PC 말하기 아이콘 
public class VoiceEffect : MonoBehaviourPun
{
    PhotonVoiceView voiceView;
    VoiceroomManager voiceManager;

    // 음소거 
    [SerializeField] GameObject TalkBell;
    [SerializeField] GameObject MuteBell; 

    bool isSpeaking = false;
    bool recentBool;

    private void Awake()
    {
        TalkBell.SetActive(false);
        MuteBell.SetActive(false);
        voiceView = GetComponentInParent<PhotonVoiceView>();
       
        recentBool = true;
    }

    public bool GetSoundDetection()
    {
        return voiceView.IsRecording;     
    }

    private void Update()
    {
        if (photonView.IsMine == false) return;
        SpeakIcon();
    }

    public void SpeakIcon()
    {
        if (GetSoundDetection())
        {
            if (recentBool) return;

            photonView.RPC(nameof(SetActive), RpcTarget.All, true);
        }
        else
        {
            if (recentBool == false) return;

            photonView.RPC(nameof(SetActive), RpcTarget.All, false);
        }
    }

    [PunRPC]
    private void SetActive(bool _state)
    {
        TalkBell.SetActive(_state);
        MuteBell.SetActive(!_state);
        recentBool = _state;
    }
}
