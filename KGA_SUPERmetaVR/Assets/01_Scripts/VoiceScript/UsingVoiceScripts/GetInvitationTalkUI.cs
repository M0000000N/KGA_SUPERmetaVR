using Assets.OVR.Scripts;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetInvitationTalkUI : MonoBehaviourPun
{
    [SerializeField] VoiceroomManager voiceroomManager; 

    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private GameObject TalkingCheckUI;
    [SerializeField] private Button Approve;
    [SerializeField] private Button Reject; 

    private PhotonView photonView;
    private PhotonVoiceNetwork voiceNetwork;
    Recorder recorder;

    int playerCount; 

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        recorder = voiceroomManager.recorder; 
    }

    //Approve Ȯ�ι�ư ������ �� ���̽� ä�� ���� 
    public void ChangeAudioGroup(int value)
    {
        byte _InterestGroup = 0;
        if (value != 0)
        {
            _InterestGroup = (byte)(PhotonNetwork.CurrentRoom.PlayerCount);

            if (_InterestGroup != (byte)(PhotonNetwork.CurrentRoom.PlayerCount))
            {
                ++_InterestGroup;
            }
        }
        else
            PhotonVoiceNetwork.Instance.Client.GlobalInterestGroup = _InterestGroup;
    }

    //���� ��ư ���� �� ���̽�ä�� ���� �� ���Ŭ���̾�Ʈ���� ���� 
    public void ChangedAudioGroup(int value)
    {
        byte _InterestGroup = 0;
        value = (byte)(PhotonNetwork.CurrentRoom.PlayerCount);
        playerCount = value; 

        if (value != 0)
        {
            _InterestGroup = (byte)playerCount; 

        }
        PhotonVoiceNetwork.Instance.Client.GlobalInterestGroup = _InterestGroup;
    }
}
