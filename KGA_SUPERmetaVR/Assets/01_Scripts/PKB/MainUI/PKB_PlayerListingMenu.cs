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
            // �÷��̾� �ּ� �ο� �޼��ߴ���
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
        // Debug.Log($"{newPlayer.NickName}�� {PhotonNetwork.CurrentRoom.Name}�� ����. �ѿ� : {PhotonNetwork.CurrentRoom.PlayerCount}");
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

            PhotonNetwork.LoadLevel("PKB_InGame");
            LobbyManager.Instance.CurrentSceneIndex = 3;
        }
        else
        {
            SetReadyUp(!playerIsReady);
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
            listings[index].ActiveReadyPanel(false); // TODO : ���ְ� �׽�Ʈ �ʿ�
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
                            SetStartButton(0, false);
                            return;
                        }
                    }
                }
            }

            // �÷��̾� �ּ� �ο� �޼��ߴ���
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
                    foreach (bool _isReady in valueColl) //value�� string�� ��
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
    /// _index : 0-�����, 1-���ӽ���, 2-�غ�, 3-�غ�Ϸ�
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