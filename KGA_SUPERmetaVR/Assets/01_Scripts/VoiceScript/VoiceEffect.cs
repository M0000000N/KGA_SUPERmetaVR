using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;


//PC ���ϱ� ������ 
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

    //���̽� ä�� ����
    public bool GetSoundDetection()
    {
        return voiceView.IsRecording;     
    }

    // ���̽� ���� ���� - ��Ʈ ���� 
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
            Debug.Log("����");
            Speaker.gameObject.SetActive(true);
            Speaker.SetBool("VocieTalk", true);
            photonView.RPC(nameof(SetMute), RpcTarget.All, true);
        }
        else
        {
            Debug.Log("�޷�");
            Speaker.SetBool("VocieTalk", false);
            Speaker.gameObject.SetActive(false);
            photonView.RPC(nameof(SetMute), RpcTarget.All, false);
        }
    }

    [PunRPC]
    private void SetMute(bool _state)
    {
        Mute.SetActive(_state);
    }
}
