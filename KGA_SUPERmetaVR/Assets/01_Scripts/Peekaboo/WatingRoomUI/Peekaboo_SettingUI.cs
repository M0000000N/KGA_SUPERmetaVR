using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Peekaboo_SettingUI : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] Slider bgmSlider;
    [SerializeField] TextMeshProUGUI bgmValue;
    float bgmPreviousSetting;

    [Header("SE")]
    [SerializeField] Slider seSlider;
    [SerializeField] TextMeshProUGUI seValue;
    float sePreviousSetting;

    [Header("MIC")]
    [SerializeField] Slider micSlider;
    [SerializeField] TextMeshProUGUI micValue;
    float micPreviousSetting;

    [Header("Voice")]
    [SerializeField] Slider voiceSlider;
    [SerializeField] TextMeshProUGUI voiceValue;
    float voicePreviousSetting;

    [Header("ApplyButton")]
    [SerializeField] GameObject applyButtonOn;
    [SerializeField] GameObject applyButtonOff;

    void Update()
    {
        SetVolume();
        RefreshUI();
    }

    public void SetVolume()
    {
        SoundManager.Instance.SetBGMVolume(bgmSlider.value);
        SoundManager.Instance.SetSEVolume(seSlider.value);
    }

    public void RefreshUI()
    {
        bgmValue.text = ((int)(SoundManager.Instance.BGMValue * 100)).ToString();
        seValue.text = ((int)(SoundManager.Instance.SEValue * 100)).ToString();

        if(IsChangeSetting())
        {
            applyButtonOn.SetActive(true);
            applyButtonOff.SetActive(false);
        }
        else
        {
            applyButtonOn.SetActive(false);
            applyButtonOff.SetActive(true);
        }
    }

    public bool IsChangeSetting()
    {
        if (bgmPreviousSetting == SoundManager.Instance.BGMValue
            && sePreviousSetting == SoundManager.Instance.SEValue
          //&& micPreviousSetting == SoundManager.Instance.MICValue
          //&& voicePreviousSetting == SoundManager.Instance.VoiceValue;
            )
        {
            return false;
        }
        return true;
    }

    public void SettingSave()
    {
        bgmPreviousSetting = SoundManager.Instance.BGMValue;
        sePreviousSetting = SoundManager.Instance.SEValue;
        //micPreviousSetting = SoundManager.Instance.MICValue;
        //voicePreviousSetting = SoundManager.Instance.VoiceValue;
    }

    public void OnPopupUI()
    {
        SettingSave();
        gameObject.SetActive(true);
    }

    public void OffPopupUI()
    {
        gameObject.SetActive(false);
    }
}
