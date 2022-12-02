using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Voice.Unity;

public class PKB_SettingUI : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] Slider bgmSlider;
    [SerializeField] TextMeshProUGUI bgmValue;
    private float bgmPreviousSetting;

    [Header("SE")]
    [SerializeField] Slider seSlider;
    [SerializeField] TextMeshProUGUI seValue;
    private float sePreviousSetting;

    [Header("Voice")]
    [SerializeField] Recorder recorder;
    [SerializeField] Toggle micActivate;  // 마이크 활성화, 비활성화 

    //[Header("MIC")]
    //[SerializeField] Slider micSlider;
    //[SerializeField] TextMeshProUGUI micValue;
    //private float micPreviousSetting;

    //[Header("Voice")]
    //[SerializeField] Slider voiceSlider;
    //[SerializeField] TextMeshProUGUI voiceValue;
    //private float voicePreviousSetting;

    //[Header("ApplyButton")]
    //[SerializeField] GameObject applyButtonOn;
    //[SerializeField] GameObject applyButtonOff;

    private void Start()
    {
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        seSlider.onValueChanged.AddListener(SetSEVolume);
        micActivate.onValueChanged.AddListener(SetTransmitSound);
        TurnOffMute(); // 마이크가 켜져있는 기본 상태 
    }

    public void SetBGMVolume(float _volume)
    {
        SoundManager.Instance.SetBGMVolume(_volume);
        RefreshUI();
    }

    public void SetSEVolume(float _volume)
    {
        SoundManager.Instance.SetSEVolume(_volume);
        RefreshUI();
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
        recorder.TransmitEnabled = false;
        Debug.Log("마이크꺼짐");
    }

    public void TurnOffMute()
    {
        // 마이크 켜짐
        recorder.TransmitEnabled = true;
        Debug.Log("마이크켜짐");
    }

    public void RefreshUI()
    {
        bgmValue.text = ((int)(SoundManager.Instance.BGMValue * 100)).ToString();
        seValue.text = ((int)(SoundManager.Instance.SEValue * 100)).ToString();
        //voiceValue.text = ((int)(SoundManager.Instance.VoiceValue * 100)).ToString();

        //if (IsChangeSetting())
        //{
        //    applyButtonOn.SetActive(true);
        //    applyButtonOff.SetActive(false);
        //}
        //else
        //{
        //    applyButtonOn.SetActive(false);
        //    applyButtonOff.SetActive(true);
        //}
    }


    //public bool IsChangeSetting()
    //{
    //    if (bgmPreviousSetting == SoundManager.Instance.BGMValue
    //        && sePreviousSetting == SoundManager.Instance.SEValue
    //        && voicePreviousSetting == SoundManager.Instance.VoiceValue
    //      //&& micPreviousSetting == SoundManager.Instance.MICValue
    //        )
    //    {
    //        return false;
    //    }
    //    return true;
    //}

    public void SettingSave()
    {
        bgmPreviousSetting = SoundManager.Instance.BGMValue;
        sePreviousSetting = SoundManager.Instance.SEValue;
        //voicePreviousSetting = SoundManager.Instance.VoiceValue;
        //micPreviousSetting = SoundManager.Instance.MICValue;
    }

    public void OnPopupUI()
    {
        SettingSave();
        gameObject.SetActive(true);
    }

    public void OffPopupUI()
    {
        SettingSave();
        gameObject.SetActive(false);
    }
}
