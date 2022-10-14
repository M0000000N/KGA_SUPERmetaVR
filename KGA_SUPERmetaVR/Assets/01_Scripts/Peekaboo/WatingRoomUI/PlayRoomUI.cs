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
    [SerializeField] Button startButton;
    [SerializeField] TextMeshProUGUI RoomNameText;

    private void Awake()
    {
        exitButton.onClick.AddListener(OnClickExitButton);
        startButton.onClick.AddListener(OnClickStartButton);
        startButton.interactable = false;
    }
    private void Update()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            startButton.interactable = true;
        }
    }

    public void SetRoomInfo(RoomInfo _roomInfo)
    {
        RoomInfo = _roomInfo;
        RoomNameText.text = "# " + _roomInfo.Name;
        // memberText.text = $"{_roomInfo.PlayerCount} / {_roomInfo.MaxPlayers}";
    }


    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }

    public void OnClickStartButton()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            gameObject.SetActive(false);
            PhotonNetwork.LoadLevel("Peekaboo_InGame");
        }
    }
}
