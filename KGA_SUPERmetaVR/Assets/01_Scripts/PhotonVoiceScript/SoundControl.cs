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

// 마이크활성화 / 비활성화
// 음량 조절
// 마이크 조절 - 내 것만 해보고 안 되면 빼기 
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

    [SerializeField] Toggle micActivate;  // 마이크 활성화, 비활성화 
    [SerializeField] Button soundSetting; // 설정 눌렀을 때 
    [SerializeField] Button closeButton; // X 버튼 눌렀을 때 
    [SerializeField] GameObject soundSettingImage; // 사운드 세팅 이미지 

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
        TurnOffMute(); // 마이크가 켜져있는 기본 상태 

    }

    private void Update()
    {
        micSlider();
    }

    // 설정창 켜짐
    private void clickSoundSetting()
    {
        soundSettingImage.SetActive(true);
    }

    // X 버튼 누르면 설정창 꺼지기 
    public void CloseSoundSetting()
    {
        soundSettingImage.SetActive(false);
    }

    //보이스 크기조정
    public void SetSoundVolme(float sound)
    {
        AudioListener.volume = sound;
    }

    //음성감지유무
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

    // 마이크 음소거
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
        // 마이크 꺼짐 
       // PeekabooSoundManager.Instance.recorder.TransmitEnabled = true; 
        recorder.TransmitEnabled = false;
        Debug.Log("마이크꺼짐");
    }

    public void TurnOffMute()
    {
        // 마이크 켜짐
        recorder.TransmitEnabled = true;
      //  PeekabooSoundManager.Instance.recorder.TransmitEnabled = false;
        Debug.Log("마이크켜짐");
    }


}
