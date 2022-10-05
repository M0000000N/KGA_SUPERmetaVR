using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomButton : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI RoomNameText;
    [SerializeField] TextMeshProUGUI memberText;
    
    Button roomButton;
    public RoomInfo RoomInfo { get; private set; }
    private void Start()
    {
        roomButton = GetComponent<Button>();
        roomButton.onClick.AddListener(OnClickRoomButton);
    }
    public void SetRoomInfo(RoomInfo _roomInfo)
    {
        RoomInfo = _roomInfo;
        RoomNameText.text = _roomInfo.Name;
        memberText.text = $"{_roomInfo.PlayerCount} / {_roomInfo.MaxPlayers}";
    }

    public void OnClickRoomButton()
    {
        //if (nickname.text.Length == 0)
        //{
        //    logText.text = "닉네임을 입력하세요";
        //    return;
        //}

        PhotonNetwork.JoinRoom(RoomInfo.Name);
    }
}
