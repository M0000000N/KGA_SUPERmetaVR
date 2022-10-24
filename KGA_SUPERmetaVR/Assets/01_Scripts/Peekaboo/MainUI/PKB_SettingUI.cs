using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [Header("MIC")]
    [SerializeField] Slider micSlider;
    [SerializeField] TextMeshProUGUI micValue;
    private float micPreviousSetting;

    [Header("Voice")]
    [SerializeField] Slider voiceSlider;
    [SerializeField] TextMeshProUGUI voiceValue;
    private float voicePreviousSetting;

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
        SoundManager.Instance.SetVoiceVolume(voiceSlider.value);
    }

    public void RefreshUI()
    {
        bgmValue.text = ((int)(SoundManager.Instance.BGMValue * 100)).ToString();
        seValue.text = ((int)(SoundManager.Instance.SEValue * 100)).ToString();
        voiceValue.text = ((int)(SoundManager.Instance.VoiceValue * 100)).ToString();

        if (IsChangeSetting())
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
            && voicePreviousSetting == SoundManager.Instance.VoiceValue
          //&& micPreviousSetting == SoundManager.Instance.MICValue
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
        voicePreviousSetting = SoundManager.Instance.VoiceValue;
        //micPreviousSetting = SoundManager.Instance.MICValue;
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
