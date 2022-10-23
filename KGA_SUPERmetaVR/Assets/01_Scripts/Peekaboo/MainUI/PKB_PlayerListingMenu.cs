using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PKB_PlayerListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform content;
    [SerializeField] PKB_PlayerListing playerListing;
    private List<PKB_PlayerListing> listings = new List<PKB_PlayerListing>();

    [SerializeField] Button gameStartButton;
    private TextMeshProUGUI gameStartButtonText;

    [SerializeField] int MinPlayerCount;
    private bool playerIsReady = false;

    private void Awake()
    {
        gameStartButton.onClick.AddListener(OnClickStartButton);
        gameStartButtonText = gameStartButton.GetComponentInChildren<TextMeshProUGUI>();
        SetReadyUp(false);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        GetCurrentRoomPlayers();
        SetReadyUp(false);
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SetStartButton("대기중", false);
        }
        else
        {
            SetStartButton("준비", true);
        }
    }

    // 새로운 방에 맞지 않는 플레이어가 들어오는 버그
    public override void OnDisable()
    {
        for (int i = 0; i < listings.Count; i++)
        {
            Destroy(listings[i].gameObject);
        }
        listings.Clear();
    }

    private void Update()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (PhotonNetwork.PlayerList.Length > MinPlayerCount) // TODO : 전부 준비완료가 됐는지?
                {
                    SetStartButton("게임시작", true);
                }
                else
                {
                    SetStartButton("대기중", false);
                }
            }
        }
        else
        {
            SetStartButton("대기중", false);
        }
    }

    private void GetCurrentRoomPlayers()
    {
        if (PhotonNetwork.IsConnected)
        {
            for (int i = 1; i < PhotonNetwork.CurrentRoom.Players.Count; i++)
            {
                AddPlayerListing(PhotonNetwork.CurrentRoom.Players.ElementAt(i).Value);
            }
            AddPlayerListing(PhotonNetwork.CurrentRoom.Players.ElementAt(0).Value);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    private void AddPlayerListing(Player _newPlayer)
    {
        int index = listings.FindIndex(x => x.Player == _newPlayer);
        if (index != -1)
        {
            listings[index].SetPlayerInfo(_newPlayer);
        }
        else
        {
            PKB_PlayerListing listing = Instantiate(playerListing, content);
            if (listing != null)
            {
                listing.SetPlayerInfo(_newPlayer);
                listings.Add(listing);
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = listings.FindIndex(x => x.Player == otherPlayer);
        if (index != -1)
        {
            Destroy(listings[index].gameObject);
            listings.RemoveAt(index);
        }
    }

    private void SetReadyUp(bool _playerIsReady)
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("IsReady") == false)
        {
            PhotonNetwork.LocalPlayer.CustomProperties.Add("IsReady", false);
        }

        Hashtable newCustomProperty = new Hashtable() { { "IsReady", _playerIsReady } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(newCustomProperty);

        // PhotonNetwork.LocalPlayer.CustomProperties["IsReady"] = _playerIsReady;

        
    }

    public void OnClickStartButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
            {
                if (player.Value != PhotonNetwork.LocalPlayer)
                {
                    Hashtable customProperty = player.Value.CustomProperties;
                    ICollection valueColl = customProperty.Values;
                    foreach (bool _isReady in valueColl) //value가 string일 때
                    {
                        if (_isReady == false)
                        {
                            // TODO : 나중에 데이터로 빼야함
                            PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "모든 플레이어가 준비완료 상태가 아닙니다.", "확인");
                            return;
                        }
                    }
                }
            }
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            PhotonNetwork.CurrentRoom.IsOpen = false; // TODO : 방 시작하면 접근 못하도록 설정 - 테스트 필요
            PhotonNetwork.LoadLevel("Peekaboo_InGame");
        }
        else
        {
            SetReadyUp(!playerIsReady);
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value == PhotonNetwork.LocalPlayer)
            {
                Hashtable customProperty = player.Value.CustomProperties;
                ICollection valueColl = customProperty.Values;
                foreach (bool _isReady in valueColl) //value가 string일 때
                {
                    if (_isReady)
                    {
                        SetStartButton("준비완료", true);
                    }
                    else
                    {
                        SetStartButton("준비", true);
                    }
                    playerIsReady = _isReady;
                }
                return;
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        int index = listings.FindIndex(x => x.Player == targetPlayer);
        if (index != -1)
        {
            Hashtable customProperty = targetPlayer.CustomProperties;
            ICollection valueColl = customProperty.Values;
            foreach (bool _isReady in valueColl) //value가 string일 때
            {
                listings[index].ActiveReadyPanel(_isReady);
            }
        }
    }

    private void SetStartButton(string text, bool isInteractable)
    {
        gameStartButtonText.text = text;
        gameStartButton.interactable = isInteractable;
    }
}
