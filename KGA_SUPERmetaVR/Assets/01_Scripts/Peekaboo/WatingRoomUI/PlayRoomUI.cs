using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class PlayRoomUI : MonoBehaviour
{
    public RoomInfo RoomInfo { get; private set; }

    [SerializeField] Button exitButton;

    private void Awake()
    {
        exitButton.onClick.AddListener(OnClickExitButton);
    }
    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }
    public void SetRoomInfo(RoomInfo _roomInfo)
    {
        RoomInfo = _roomInfo;
        // RoomNameText.text = _roomInfo.Name;
        // memberText.text = $"{_roomInfo.PlayerCount} / {_roomInfo.MaxPlayers}";
    }
}
