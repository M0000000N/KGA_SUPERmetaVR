using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class PKB_PlayRoomUI : MonoBehaviourPunCallbacks
{
    public RoomInfo RoomInfo { get; private set; }

    [Header("PlayRoom")]
    [SerializeField] TextMeshProUGUI roomNameText;
    [SerializeField] TextMeshProUGUI roomTypeText;

    [Header("ExitRoom")]
    [SerializeField] Button exitRoomButton;
    [SerializeField] GameObject exitRoomUI;
    [SerializeField] Button yesExitRoomButton;
    [SerializeField] Button noExitRoomButton;

    [SerializeField] PKB_PlayerListingMenu playerListingMenu;

    private void Awake()
    {
        exitRoomUI.gameObject.SetActive(false);
        exitRoomButton.onClick.AddListener(OnClickExitRoomButton);
        yesExitRoomButton.onClick.AddListener(OnClickYesExitRoomButton);
        noExitRoomButton.onClick.AddListener(OnClickNoExitRoomButton);
        playerListingMenu.gameObject.SetActive(false);
    }

    public void SetRoomInfo(RoomOptions _roomOptions)
    {
        // �� �̸�
        roomNameText.text = "# " + PhotonNetwork.CurrentRoom.CustomProperties["RoomName"].ToString();

        // �� Ÿ��
        if (PhotonNetwork.CurrentRoom.CustomProperties["Password"] == null)
        {
            roomTypeText.text = "������";
        }
        else
        {
            roomTypeText.text = "��й�";
        }
        playerListingMenu.gameObject.SetActive(true);
    }

    public void OnClickExitRoomButton()
    {
        SoundManager.Instance.PlaySE("popup_open.wav");
        exitRoomUI.gameObject.SetActive(true);
        Debug.Log($"{LobbyManager.Instance.IsRoom}");
    }

    public void OnClickYesExitRoomButton()
    {
        SoundManager.Instance.PlaySE("popup_close.wav");
        SoundManager.Instance.PlayBGM("PKBOO_Main_bgm.wav");
        exitRoomUI.gameObject.SetActive(false);
        gameObject.SetActive(false);
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public void OnClickNoExitRoomButton()
    {
        SoundManager.Instance.PlaySE("popup_close.wav");
        exitRoomUI.gameObject.SetActive(false);
    }
#if �߹�
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
            // isKicked property�� �����Ұ��
            if (changedProps["isKicked"] != null)
            {
                // �̰� true�� ��쿡�� ����
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
#endif
}