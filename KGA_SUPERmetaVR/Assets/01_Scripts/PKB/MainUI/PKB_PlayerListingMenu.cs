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

    [SerializeField] Button[] StartButton;

    [SerializeField] int MinPlayerCount = 0;
    private bool playerIsReady = false;

    public override void OnEnable()
    {
        GetCurrentRoomPlayers();
        SetReadyUp(false);
        if (PhotonNetwork.IsMasterClient)
        {
            // 플레이어 최소 인원 달성했는지
            if (PhotonNetwork.PlayerList.Length > MinPlayerCount)
            {
                SetStartButton(1, true);
            }
            else
            {
                SetStartButton(0, false);
            }
        }
        else
        {
            SetStartButton(2, true);
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
        // Debug.Log($"{newPlayer.NickName}가 {PhotonNetwork.CurrentRoom.Name}에 들어옴. 총원 : {PhotonNetwork.CurrentRoom.PlayerCount}");
        AddPlayerListing(newPlayer);
    }

    private void AddPlayerListing(Player _newPlayer)
    {
        int index = listings.FindIndex(x => x.Player == _newPlayer);
        if (index != -1) // TODO : 무슨 예외처리인지
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

            // 방 접속할 때 준비상태 동기화
            int newIndex = listings.FindIndex(x => x.Player == _newPlayer);
            Hashtable customProperty = _newPlayer.CustomProperties;
            ICollection valueColl = customProperty.Values;
            foreach (bool _isReady in valueColl)
            {
                listings[newIndex].ActiveReadyPanel(_isReady);
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

            PhotonNetwork.LoadLevel("PKB_InGame");
            LobbyManager.Instance.CurrentSceneIndex = 3;
        }
        else
        {
            SetReadyUp(!playerIsReady);
        }
    }

    // 플레이어가 모두 준비완료가 되었는지 확인
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

        if (PhotonNetwork.IsMasterClient)
        {
            listings[index].ActiveReadyPanel(false); // TODO : 없애고 테스트 필요
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
                            SetStartButton(0, false);
                            return;
                        }
                    }
                }
            }

            // 플레이어 최소 인원 달성했는지
            if (PhotonNetwork.PlayerList.Length > MinPlayerCount)
            {
                SetStartButton(1, true);
            }
            else
            {
                SetStartButton(0, false);
            }
        }
        else
        {
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
                            SetStartButton(3, true);
                        }
                        else
                        {
                            SetStartButton(2, true);
                        }
                        playerIsReady = _isReady;
                    }
                    return;
                }
            }
        }
    }

    /// <summary>
    /// _index : 0-대기중, 1-게임시작, 2-준비, 3-준비완료
    /// </summary>
    /// <param name="_index"></param>
    /// <param name="_isInteractable"></param>
    private void SetStartButton(int _index, bool _isInteractable)
    {
        for (int i = 0; i < StartButton.Length; i++)
        {
            if (i == _index)
            {
                StartButton[i].gameObject.SetActive(true);
                StartButton[i].interactable = _isInteractable;
                continue;
            }
            StartButton[i].gameObject.SetActive(false);
        }
    }
}