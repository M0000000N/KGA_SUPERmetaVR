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
    Recorder recorder;

    [SerializeField] Animator Speaker;
    [SerializeField] GameObject Mute;

    bool isSpeaking = false; 

    private void Awake()
    {
      //  hotonView = GetComponent<PhotonView>();
        Mute.SetActive(false);
        voiceView = GetComponentInParent<PhotonVoiceView>();
        recorder= GetComponent<Recorder>();
    }

    //���̽� ä�� ����
    public bool GetSoundDetection()
    { 
        return voiceView.IsRecording;     
    }

    // ���̽� ���� ���� - ��Ʈ ���� 
    public void SetTransmitSound(bool isOn)
    {
        recorder.TransmitEnabled = isOn;
    }

    private void Update()
    {
        if (photonView.IsMine == false) return;

        SpeakIcon();
    }

    public void SpeakIcon()
    {
        photonView.RPC("VoiceConnection", RpcTarget.All);
    }

    [PunRPC]
    private void VoiceConnection()
    {
        if (GetSoundDetection())
        {

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
