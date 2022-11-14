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

// ����ũȰ��ȭ / ��Ȱ��ȭ
// ���� ����
// ����ũ ���� - �� �͸� �غ��� �� �Ǹ� ���� 
public class SoundControl : MonoBehaviourPun
{
    [SerializeField]
    public Slider MicVoulmSlider;
    [SerializeField]
    Recorder recorder;

    MicAmplifier MicAmplifier; 
    PhotonVoiceView photonvoice;
    PhotonView view;
    MicAmplifier micAmplifier;

    [SerializeField] Toggle micActivate;  // ����ũ Ȱ��ȭ, ��Ȱ��ȭ 
    [SerializeField] Button soundSetting; // ���� ������ �� 
    [SerializeField] Button closeButton; // X ��ư ������ �� 
    [SerializeField] GameObject soundSettingImage; // ���� ���� �̹��� 

    private float micVolum;

    private void Start()
    {
      
        micAmplifier = GetComponent<MicAmplifier>();
        micAmplifier = new MicAmplifier();

        micVolum = PlayerPrefs.GetFloat("micVolum", 0f);
        micAmplifier.AmplificationFactor = micVolum;

        soundSettingImage.SetActive(false);
        soundSetting.onClick.AddListener(clickSoundSetting);
        closeButton.onClick.AddListener(CloseSoundSetting);

        micActivate.onValueChanged.AddListener(SetTransmitSound);
        TurnOffMute(); // ����ũ�� �����ִ� �⺻ ���� 

    }

    private void Update()
    {
        micSlider();
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
    
    public void micSlider()
    {
        micAmplifier.AmplificationFactor = MicVoulmSlider.value;
        micVolum = MicVoulmSlider.value;
        PlayerPrefs.SetFloat("micVolum", micVolum);
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
       // PeekabooSoundManager.Instance.recorder.TransmitEnabled = true; 
        recorder.TransmitEnabled = false;
        Debug.Log("����ũ����");
    }

    public void TurnOffMute()
    {
        // ����ũ ����
        recorder.TransmitEnabled = true;
      //  PeekabooSoundManager.Instance.recorder.TransmitEnabled = false;
        Debug.Log("����ũ����");
    }


}
