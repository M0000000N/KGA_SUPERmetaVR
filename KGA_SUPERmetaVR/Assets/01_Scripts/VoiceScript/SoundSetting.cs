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
using Photon.Voice.Unity;
using Photon.Voice;
using Facebook.WitAi.Lib;
using Photon.Voice.Unity.Demos;

/// <summary>
/// 마이크 활성화 / 비활성화 
/// 말하기 아이콘 
/// </summary>
public class SoundSetting : MonoBehaviourPun
{
    // 설정버튼 누르면 sound setting 나옴
    // 마이크 버튼 누르면 비활성화 (눌려진 상태임)  
    [SerializeField] Toggle micActivate;  // 마이크 활성화, 비활성화 
    [SerializeField] Button soundSetting; // 설정 눌렀을 때 
    [SerializeField] Button closeButton; // X 버튼 눌렀을 때 
    [SerializeField] GameObject soundSettingImage; // 사운드 세팅 이미지 

    [Header("PC 말하기 아이콘")]
    [SerializeField] private Animator talking;
    [SerializeField] private GameObject mute;

    private void Start()
    {
        // PC 말하기 아이콘
            mute.SetActive(false);
            talking.SetBool("VocieTalk", false);

        if (photonView.IsMine)
        {
            soundSettingImage.SetActive(false);
            soundSetting.onClick.AddListener(clickSoundSetting);
            closeButton.onClick.AddListener(CloseSoundSetting);

            micActivate.onValueChanged.AddListener(Mute);
            TurnOffMute(); // 마이크가 켜져있는 기본 상태 
        }
    }

    private void Update()
    {
        if (photonView.IsMine) return;

        micAnimation(); // PC 아이콘
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
    private void Mute(bool isOn)
    {
        if(isOn)
        {
            TurnOffMute();
        }
        else
        {
            TurnonMute();
        }
    }

    private void TurnonMute()
    {
        // 마이크 꺼짐 
        VoiceroomManager.Instance.recorder.TransmitEnabled = false; 
    }

    private void TurnOffMute()
    {
        // 마이크 켜짐
        VoiceroomManager.Instance.recorder.TransmitEnabled = true;
    }


    private void micAnimation()
    {
        // 음성 감지되면 
        if (VoiceroomManager.Instance.recorder.VoiceDetectionThreshold > 0.2f)
        {
            talking.gameObject.SetActive(true);
            talking.SetBool("VocieTalk", true);
            Debug.Log("음성감지");
            mute.SetActive(false);
        }
        // 감지되지 않으면
        else
        {
            // 음소거 
            if (VoiceroomManager.Instance.recorder.TransmitEnabled == false)
            {
                mute.SetActive(true);
                talking.gameObject.SetActive(false);
            }
            // 음소거 아닐 때 
            else 
            {
                talking.gameObject.SetActive(true);
                talking.SetBool("VocieTalk", true);
                mute.SetActive(false);
            }
        }
    }

}