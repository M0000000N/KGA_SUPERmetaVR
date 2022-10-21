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
            gameStartButtonText.text = "게임시작";
        }
        else
        {
            gameStartButtonText.text = "준비";
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

    Hashtable playerCustomProperties = new Hashtable();
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
        playerCustomProperties["IsReady"] = playerIsReady;
        PhotonNetwork.SetPlayerCustomProperties(playerCustomProperties);
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
            base.photonView.RPC("RPC_ChangeReadyState", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer, playerIsReady);
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
}
