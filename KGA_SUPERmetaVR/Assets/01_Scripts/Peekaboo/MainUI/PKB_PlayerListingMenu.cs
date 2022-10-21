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
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameStartButtonText.text = "���ӽ���";
        }
        else
        {
            gameStartButtonText.text = "�غ�";
        }
    }
    public override void OnEnable()
    {
        base.OnEnable();
        GetCurrentRoomPlayers();
        SetReadyUp(false);
    }

    // ���ο� �濡 ���� �ʴ� �÷��̾ ������ ����
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

                if (PhotonNetwork.PlayerList.Length > MinPlayercount) // TODO : ���� �غ�Ϸᰡ �ƴ���?
                {
                    gameStartButtonText.text = "���ӽ���";
                    gameStartButton.interactable = true;
                }
                else
                {
                    gameStartButtonText.text = "�����";
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
            gameStartButtonText.text = "�����";
            gameStartButton.interactable = false;
        }
    }

    private void GetCurrentRoomPlayers()
    {
        if (PhotonNetwork.IsConnected)
        {
            foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
            {
                AddPlayerListing(playerInfo.Value);
            }
        }
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

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        content.DestroyChildren();
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
            gameStartButtonText.text = "�غ�Ϸ�";
        }
        else
        {
            gameStartButtonText.text = "�غ�";
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
                        // TODO : ���߿� �����ͷ� ������
                        PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "��� �÷��̾ �غ�Ϸ� ���°� �ƴմϴ�.", "Ȯ��");
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