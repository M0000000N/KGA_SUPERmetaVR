using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;


//PC 말하기 아이콘 
public class VoiceEffect : MonoBehaviourPun
{
    PhotonVoiceView voiceView;
    // PhotonView photonView; 
   // Recorder recorder;

    [SerializeField] Animator Speaker;
    [SerializeField] GameObject Mute;

    bool isSpeaking = false; 

    private void Awake()
    {
      //  hotonView = GetComponent<PhotonView>();
        Mute.SetActive(false);
        voiceView = GetComponentInParent<PhotonVoiceView>();
       // recorder= GetComponent<Recorder>();
    }

    //보이스 채팅 유무
    public bool GetSoundDetection()
    {
        return voiceView.IsRecording;     
    }

    // 보이스 전송 유무 - 뮤트 관련 
    //public void SetTransmitSound(bool isOn)
    //{
    //    recorder.TransmitEnabled = isOn;
    //}

    private void Update()
    {
        if (photonView.IsMine == false) return;
        SpeakIcon();
    }

    public void SpeakIcon()
    {
        if (GetSoundDetection())
        {
            //Speaker.gameObject.SetActive(true);
            Speaker.SetBool("VocieTalk", true);
            photonView.RPC(nameof(SetActive), RpcTarget.All, false);
        }
        else
        {
            Speaker.SetBool("VocieTalk", false);
            photonView.RPC(nameof(SetActive), RpcTarget.All, true);
        }
    }

    [PunRPC]
    private void SetActive(bool _state)
    {
        Speaker.gameObject.SetActive(!_state);
        Mute.SetActive(_state);
    }
}
