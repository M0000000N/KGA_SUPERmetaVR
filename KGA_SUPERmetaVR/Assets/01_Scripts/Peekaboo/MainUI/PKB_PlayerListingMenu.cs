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

    private Hashtable playerCustomProperties = new Hashtable();

    private void Awake()
    {
        gameStartButton.onClick.AddListener(OnClickStartButton);
        gameStartButtonText = gameStartButton.GetComponentInChildren<TextMeshProUGUI>();

        playerCustomProperties.Add("IsReady", false);
        PhotonNetwork.SetPlayerCustomProperties(playerCustomProperties);
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
            gameStartButtonText.text = "���ӽ���";
        }
        else
        {
            gameStartButtonText.text = "�غ�";
        }
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
                if (PhotonNetwork.PlayerList.Length > MinPlayerCount) // TODO : ���� �غ�Ϸᰡ �ƴ���?
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
        // readyPannel.SetActive(playerIsReady);
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("IsReady") == false)
        {
            PhotonNetwork.LocalPlayer.CustomProperties.Add("IsReady", false);
        }
        Hashtable newCustomProperty = new Hashtable() { { "IsReady", _playerIsReady } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(newCustomProperty);

        PhotonNetwork.LocalPlayer.CustomProperties["IsReady"] = _playerIsReady;
        // playerCustomProperties["IsReady"] = playerIsReady;

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value == PhotonNetwork.LocalPlayer)
            {
                Hashtable customProperty = player.Value.CustomProperties;
                ICollection valueColl = customProperty.Values;
                foreach (bool _isReady in valueColl) //value�� string�� ��
                {
                    if (_isReady)
                    {
                        gameStartButtonText.text = "�غ�Ϸ�";
                    }
                    else
                    {
                        gameStartButtonText.text = "�غ�";
                    }
                    playerIsReady = _isReady;
                }
            }
        }
    }

    public void OnClickStartButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (KeyValuePair<int,Player> player in PhotonNetwork.CurrentRoom.Players)
            {
                if (player.Value != PhotonNetwork.LocalPlayer)
                {
                    Hashtable customProperty = player.Value.CustomProperties;
                    ICollection valueColl = customProperty.Values;
                    foreach (bool _isReady in valueColl) //value�� string�� ��
                    {
                        if (_isReady == false)
                        {
                            // TODO : ���߿� �����ͷ� ������
                            PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "��� �÷��̾ �غ�Ϸ� ���°� �ƴմϴ�.", "Ȯ��");
                            return;
                        }
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
            // base.photonView.RPC("RPC_ChangeReadyState", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer, playerIsReady);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        int index = listings.FindIndex(x => x.Player == targetPlayer);
        if (index != -1)
        {

            Hashtable customProperty = targetPlayer.CustomProperties;
            ICollection valueColl = customProperty.Values;
            foreach (bool _isReady in valueColl) //value�� string�� ��
            {
                listings[index].ActiveReadyPanel(_isReady);
            }

            
        }
    }

    //[PunRPC]
    //private void RPC_ChangeReadyState(Player _player, bool _isReady)
    //{
    //    int index = listings.FindIndex(x => x.Player == _player);
    //    if (index != -1)
    //    {
    //        listings[index].Ready = _isReady;
    //    }
    //}
}
