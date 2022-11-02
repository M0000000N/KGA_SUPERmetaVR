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
    // ������ư ������ sound setting ����
    // ����ũ ��ư ������ ��Ȱ��ȭ (������ ������)  
    [SerializeField] Toggle micActivate;  // ����ũ Ȱ��ȭ, ��Ȱ��ȭ 
    [SerializeField] Button soundSetting; // ���� ������ �� 
    [SerializeField] Button closeButton; // X ��ư ������ �� 
    [SerializeField] GameObject soundSettingImage; // ���� ���� �̹��� 


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

    // ����ũ ���Ұ�
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
