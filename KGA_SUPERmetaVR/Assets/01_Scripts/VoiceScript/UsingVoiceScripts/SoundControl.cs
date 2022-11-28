using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Photon.Voice.Unity.UtilityScripts;
using Photon.Voice.PUN.UtilityScripts;
using UnityEngine.SocialPlatforms;
using JetBrains.Annotations;
using OVRSimpleJSON;

// ����ũȰ��ȭ / ��Ȱ��ȭ
// ���� ����
// ����ũ ���� - �� �͸� �غ��� �� �Ǹ� ���� 
public class SoundControl : MonoBehaviourPun
{

    [SerializeField]
    Recorder recorder;

    MicAmplifier MicAmplifier; 
    PhotonVoiceView photonvoice;
    PhotonView view;

    [SerializeField] Toggle micActivate;  // ����ũ Ȱ��ȭ, ��Ȱ��ȭ 
    [SerializeField] Button soundSetting; // ���� ������ �� 
    [SerializeField] Button closeButton; // X ��ư ������ �� 
    [SerializeField] GameObject soundSettingImage; // ���� ���� �̹��� 

    private float micVolum;

    private void Start()
    {    
            MicAmplifier = GetComponent<MicAmplifier>();
            MicAmplifier = new MicAmplifier();

            micVolum = PlayerPrefs.GetFloat("micVolum", 0f);
            MicAmplifier.BoostValue = micVolum;

            soundSettingImage.SetActive(false);
            soundSetting.onClick.AddListener(clickSoundSetting);
            closeButton.onClick.AddListener(CloseSoundSetting);

            micActivate.onValueChanged.AddListener(SetTransmitSound);
            TurnOffMute(); // ����ũ�� �����ִ� �⺻ ���� 
    }

    // ����â ����
    private void clickSoundSetting()
    {
        soundSettingImage.SetActive(true);
    }

    // X ��ư ������ ����â ������ 
    public void CloseSoundSetting()
    {
        soundSettingImage.SetActive(false);
    }

    //���̽� ũ������
    public void SetSoundVolme(float sound)
    {
        AudioListener.volume = sound;
    }

    //������������
    public void SetVoiceDetected(bool isOn)
    {
        recorder.VoiceDetection = isOn;
    }

    // ����ũ ���Ұ�
    private void SetTransmitSound(bool isOn)
    {
        if (isOn)
        {
            TurnOffMute();      
        }
        else
        {
            TurnonMute();
        }
    }

    public void TurnonMute()
    {
        // ����ũ ���� 
        recorder.TransmitEnabled = false;
        Debug.Log("����ũ����");
    }

    public void TurnOffMute()
    {
        // ����ũ ����
        recorder.TransmitEnabled = true;
        Debug.Log("����ũ����");
    }


}
