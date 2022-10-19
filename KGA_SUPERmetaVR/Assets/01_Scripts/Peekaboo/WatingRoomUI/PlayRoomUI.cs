using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayRoomUI : MonoBehaviourPunCallbacks
{
    public RoomInfo RoomInfo { get; private set; }

    [SerializeField] Button exitButton;
    [SerializeField] Button startButton;
    [SerializeField] TextMeshProUGUI RoomNameText;

    [SerializeField] Button kickButton;

    Player player;
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

    public void initialize()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient && !player.IsLocal)
        {
            kickButton.gameObject.SetActive(true);
        }
        else
        {
            kickButton.gameObject.SetActive(false);
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

    public void kickPlayerButton()
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Add("isKicked", true);
        // player.SetCustomProperties(hashtable);
    }

    public void refreshPlayerNameList()
    {
        // destroyAllPlayerNameRecord();
        Dictionary<int, Player> lists = PhotonNetwork.CurrentRoom.Players;

        if (lists == null || lists.Count == 0)
        {
            return;
        }

        foreach (int key in lists.Keys)
        {
            Player player = lists[key];
           // GameObject obj = Instantiate(playerNameRecord);
           // PlayerNameRecord pnr = obj.GetComponent<PlayerNameRecord>();
           // pnr.setPlayer(player);
           // obj.transform.SetParent(scrollRect.content.transform, false);
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == PhotonNetwork.LocalPlayer)
        {
            // isKicked property가 존재할경우
            if (changedProps["isKicked"] != null)
            {
                // 이게 true인 경우에만 진행
                if ((bool)changedProps["isKicked"])
                {
                    string[] _removeProperties = new string[1];
                    _removeProperties[0] = "isKicked";
                    PhotonNetwork.RemovePlayerCustomProperties(_removeProperties);
                    PhotonNetwork.LeaveRoom();
                }
            }
        }
    }
}
