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

    private bool playerIsReady = false;
    public int MinPlayerCount;

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
            SetStartButton("�����", false);
        }
        else
        {
            SetStartButton("�غ�", true);
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
        if (index != -1) // TODO : ���� ����ó������
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

            // �� ������ �� �غ���� ����ȭ
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
        }

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
                        SetStartButton("�غ�Ϸ�", true);
                    }
                    else
                    {
                        SetStartButton("�غ�", true);
                    }
                    playerIsReady = _isReady;
                }
                return;
            }
        }
    }

    // �÷��̾ ��� �غ�Ϸᰡ �Ǿ����� Ȯ��
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

        if (PhotonNetwork.IsMasterClient)
        {
            foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
            {
                if (player.Value != PhotonNetwork.LocalPlayer)
                {
                    Hashtable customProperty = player.Value.CustomProperties;
                    ICollection valueColl = customProperty.Values;
                    foreach (bool _isReady in valueColl) //value�� string�� ��
                    {
                        if (_isReady == false)
                        {
                            SetStartButton("�����", false);
                            return;
                        }
                    }
                }
            }

            // �÷��̾� �ּ� �ο� �޼��ߴ���
            if (PhotonNetwork.PlayerList.Length > MinPlayerCount)
            {
                SetStartButton("���ӽ���", true);
            }
            else
            {
                SetStartButton("�����", false);
            }
        }
    }

    private void SetStartButton(string _text, bool _isInteractable)
    {
        gameStartButtonText.text = _text;
        gameStartButton.interactable = _isInteractable;
    }
}
