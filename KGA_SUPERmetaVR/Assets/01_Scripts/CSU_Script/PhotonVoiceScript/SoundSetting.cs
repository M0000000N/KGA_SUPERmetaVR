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

public class SoundSetting : MonoBehaviourPun
{
    // ������ư ������ sound setting ����
    // ����ũ ��ư ������ ��Ȱ��ȭ (������ ������)  
    [SerializeField] Toggle micActivate;  // ����ũ Ȱ��ȭ, ��Ȱ��ȭ 
    [SerializeField] Button soundSetting; // ���� ������ �� 
    [SerializeField] Button closeButton; // X ��ư ������ �� 
    [SerializeField] GameObject soundSettingImage; // ���� ���� �̹��� 

    [Header("PC ���ϱ� ������")]
    [SerializeField] private Animator talking;
    [SerializeField] private GameObject mute;

    private void Start()
    {
        // PC ���ϱ� ������
            mute.SetActive(false);
            talking.SetBool("VocieTalk", false);

        if (photonView.IsMine)
        {
            soundSettingImage.SetActive(false);
            soundSetting.onClick.AddListener(clickSoundSetting);
            closeButton.onClick.AddListener(CloseSoundSetting);

            micActivate.onValueChanged.AddListener(Mute);
            TurnOffMute(); // ����ũ�� �����ִ� �⺻ ���� 
        }
    }

    private void Update()
    {
        if (photonView.IsMine) return;

        micAnimation(); // PC ������
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
        // ����ũ ���� 
        PeekabooSoundManager.Instance.recorder.TransmitEnabled = false; 
    }

    private void TurnOffMute()
    {
        // ����ũ ����
        PeekabooSoundManager.Instance.recorder.TransmitEnabled = true;
    }


    private void micAnimation()
    {
        // ���� �����Ǹ� 
        if (PeekabooSoundManager.Instance.recorder.VoiceDetectionThreshold > 0.2f)
        {
            talking.gameObject.SetActive(true);
            talking.SetBool("VocieTalk", true);
            Debug.Log("��������");
            mute.SetActive(false);
        }
        // �������� ������
        else
        {
            // ���Ұ� 
            if (PeekabooSoundManager.Instance.recorder.TransmitEnabled == false)
            {
                mute.SetActive(true);
                talking.gameObject.SetActive(false);
            }
            // ���Ұ� �ƴ� �� 
            else 
            {
                talking.gameObject.SetActive(true);
                talking.SetBool("VocieTalk", true);
                mute.SetActive(false);
            }
        }
    }

}