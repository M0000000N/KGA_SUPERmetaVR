using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceChatControll : MonoBehaviour
{
    [SerializeField]
    Recorder recorder;
    [SerializeField]
    Text text;
    [SerializeField]
    Text channelText;

    PhotonView view;

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

    //�޼��� �ڽ� ����
    //����Ʈ �ڽ� ���ý� ���̽�ä�� ���� �� ���Ŭ���̾�Ʈ�� ����
    //public void ChangedAudioGroup(int value)
    //{
    //    byte _InterestGroup = 0;
    //    if (value != 0)
    //    {
    //        _InterestGroup = (byte)(PhotonNetwork.LocalPlayer.ActorNumber + PhotonNetwork.PlayerList[value - 1].ActorNumber);
    //        string valueText = _InterestGroup.ToString();

    //        view.RPC("_ReceiveMessage", PhotonNetwork.PlayerList[value - 1], "Chat", valueText);

    //    }
    //    else
    //        PhotonVoiceNetwork.Instance.Client.GlobalInterestGroup = _InterestGroup;

    //    channelText.text = recorder.InterestGroup.ToString() + "�� ä�� ����";
    //}

    ////���� ��ư ���� �� ���̽�ä�� ����
    //public void ChangedAudioGroup(string valueText)
    //{
    //    int value = int.Parse(valueText);

    //    byte _InterestGroup = 0;

    //    if (value != 0)
    //    {
    //        _InterestGroup = (byte)value;

    //    }
    //    PhotonVoiceNetwork.Instance.Client.GlobalInterestGroup = _InterestGroup;
    //    channelText.text = recorder.InterestGroup.ToString() + "�� ä�� ����";
    }
}
