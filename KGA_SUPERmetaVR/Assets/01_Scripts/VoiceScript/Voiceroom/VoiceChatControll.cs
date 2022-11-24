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

    //메세지 박스 생성
    //셀렉트 박스 선택시 보이스채널 변경 및 상대클라이언트에 전달
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

    //    channelText.text = recorder.InterestGroup.ToString() + "번 채널 접속";
    //}

    ////수락 버튼 선택 시 보이스채널 변경
    //public void ChangedAudioGroup(string valueText)
    //{
    //    int value = int.Parse(valueText);

    //    byte _InterestGroup = 0;

    //    if (value != 0)
    //    {
    //        _InterestGroup = (byte)value;

    //    }
    //    PhotonVoiceNetwork.Instance.Client.GlobalInterestGroup = _InterestGroup;
    //    channelText.text = recorder.InterestGroup.ToString() + "번 채널 접속";
    }
}
