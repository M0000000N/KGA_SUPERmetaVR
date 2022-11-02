using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEditor;
using ExitGames.Client.Photon;
using Oculus.Interaction;
using Photon.Voice.Unity;

public class SoundSetting : MonoBehaviour
{
    // 설정버튼 누르면 sound setting 나옴
    // 마이크 버튼 누르면 비활성화 (눌려진 상태임)  
    [SerializeField] Toggle micActivate;  // 마이크 활성화, 비활성화 
    [SerializeField] Button soundSetting; // 설정 눌렀을 때 
    [SerializeField] Button closeButton; // X 버튼 눌렀을 때 
    [SerializeField] GameObject soundSettingImage; // 사운드 세팅 이미지 


    private void Start()
    {
        soundSettingImage.SetActive(false);
        closeButton.enabled = false; 
    }

    private void Update()
    {
        soundSetting.onClick.AddListener(clickSoundSetting);
        closeButton.onClick.AddListener(CloseSoundSetting);
        //micActivate.Invoke("CloseSoundSetting", 0.1f);
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

    // 마이크 음소거
    public void micMute(bool mute)
    {

        Recorder recorder = GetComponent<Recorder>();
        // recorder.TransmitEnabled = mute;       
        if (mute)
        {
            micActivate.isOn = recorder.TransmitEnabled;
        }
        else
        {
            micActivate.isOn = false;
        }
    }

}
