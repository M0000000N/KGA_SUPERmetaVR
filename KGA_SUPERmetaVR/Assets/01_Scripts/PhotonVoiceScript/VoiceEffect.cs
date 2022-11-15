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
    Recorder recorder;

    [SerializeField] Animator Speaker;
    [SerializeField] GameObject Mute; 

    private void Awake()
    {
        Mute.SetActive(false);
        voiceView = GetComponent<PhotonVoiceView>();
    }

    //보이스 채팅 유무
    public bool GetSoundDetection()
    { 
        return voiceView.IsRecording;
    }

    // 보이스 전송 유무 
    public void SetTransmitSound(bool isOn)
    {
        recorder.TransmitEnabled = isOn;
    }
     
    private void Update()
    {
                  
        if (GetSoundDetection())
            {
                // speakerIcon.SetActive(true);
                Speaker.gameObject.SetActive(true);
                Speaker.SetBool("VocieTalk", true);
                Mute.SetActive(false);

            }
            else
            {
                Speaker.SetBool("VocieTalk", false);
                Speaker.gameObject.SetActive(false);
                Mute.SetActive(true);
            }
        
    }
}
