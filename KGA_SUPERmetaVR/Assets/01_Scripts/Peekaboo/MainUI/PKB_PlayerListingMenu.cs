using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PKB_PlayerListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform content;
    [SerializeField] PKB_PlayerListing playerListing;
    private List<PKB_PlayerListing> listings = new List<PKB_PlayerListing>();

    [SerializeField] Button gameStartButton;
    private TextMeshProUGUI gameStartButtonText;

    public int MinPlayercount;
    private bool playerIsReady = false;
    private bool hostIsReady = false;

    private void Awake()
    {
        gameStartButton.onClick.AddListener(OnClickStartButton);
        gameStartButtonText = gameStartButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        GetCurrentRoomPlayers();
        SetReadyUp(false);

        if (PhotonNetwork.IsMasterClient)
        {
            gameStartButtonText.text = "게임시작";
        }
        else
        {
            gameStartButtonText.text = "준비";
        }
    }

    private void Update()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
            {

                if (PhotonNetwork.PlayerList.Length > MinPlayercount) // TODO : 전부 준비완료가 됐는지?
                {
                    gameStartButtonText.text = "게임시작";
                    gameStartButton.interactable = true;
                }
                else
                {
                    gameStartButtonText.text = "대기중";
                    gameStartButton.interactable = false;
                }
            }
            else
            {
                gameStartButton.interactable = true;
            }
        }
        else
        {
            gameStartButtonText.text = "대기중";
            gameStartButton.interactable = false;
        }
    }

    private void GetCurrentRoomPlayers()
    {
        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
    }

    private void AddPlayerListing(Player _player)
    {
        PKB_PlayerListing listing = Instantiate(playerListing, content);
        if (listing != null)
        {
            listing.SetPlayerInfo(_player);
            listings.Add(listing);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
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
        playerIsReady = _playerIsReady;
        if (playerIsReady)
        {
            gameStartButtonText.text = "준비완료";
        }
        else
        {
            gameStartButtonText.text = "준비";
        }
        // readyPannel.SetActive(playerIsReady);
        SetPlayerIsReady(playerIsReady); 
    }

    public void OnClickStartButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < listings.Count; i++)
            {
                if (listings[i].Player != PhotonNetwork.LocalPlayer)
                {
                    if (listings[i].Ready == false)
                    {
                        // TODO : 나중에 데이터로 빼야함
                        PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "모든 플레이어가 준비완료 상태가 아닙니다.", "확인");
                        return;
                    }
                }
            }
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel("Peekaboo_InGame");
        }
        else
        {
            SetReadyUp(!playerIsReady);
            base.photonView.RPC("RPC_ChangeReadyState", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer, hostIsReady);
        }
    }
    
    [PunRPC]
    private void RPC_ChangeReadyState(Player _player, bool _isReady)
    {
        int index = listings.FindIndex(x => x.Player == _player);
        if (index != -1)
        {
            listings[index].Ready = _isReady;
        }
    }

    Hashtable playerCustomProperties = new Hashtable();

    private void SetPlayerIsReady(bool _isReady)
    {
        playerCustomProperties["IsReady"] = _isReady;
        PhotonNetwork.SetPlayerCustomProperties(playerCustomProperties);
    }
}
