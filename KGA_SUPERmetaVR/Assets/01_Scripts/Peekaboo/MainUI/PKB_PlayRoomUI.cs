#define �׽�Ʈ��
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PKB_PlayRoomUI : MonoBehaviourPunCallbacks
{
    public RoomInfo RoomInfo { get; private set; }
    public int MinPlayercount;

    [Header("PlayRoom")]
    [SerializeField] TextMeshProUGUI RoomNameText;
    [SerializeField] Button gameStartButton;
    private TextMeshPro gameStartButtonText;
    [SerializeField] Button exitRoomButton;
    [SerializeField] TextMeshProUGUI RoomTypeText;

    [Header("ExitRoom")]
    [SerializeField] GameObject exitRoomUI;
    [SerializeField] Button YesExitRoomButton;
    [SerializeField] Button NoExitRoomButton;

    [Header("PlayerList")]
    [SerializeField] PKB_PlayerPanel playerPanel;

    Player player; // TODO : ���� ������?

    private void Awake()
    {
        gameStartButton.onClick.AddListener(OnClickStartButton);
        exitRoomButton.onClick.AddListener(OnClickExitButton);
        exitRoomUI.gameObject.SetActive(false);
        YesExitRoomButton.onClick.AddListener(OnClickYesExitRoomButton);
        NoExitRoomButton.onClick.AddListener(OnClickNoExitRoomButton);

        gameStartButton.interactable = false;
        gameStartButtonText = gameStartButton.GetComponentInChildren<TextMeshPro>();
    }

    private void Update()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount > MinPlayercount)
        {
#if �׽�Ʈ��
            gameStartButton.interactable = true;
#endif

#if ȣ��Ʈ�Ǵ�
            // ȣ��Ʈ�� ���ӽ���, �Ϲ� �÷��̾�� �غ�
            if (PhotonNetwork.LocalPlayer.IsMasterClient && !player.IsLocal)
            {
                gameStartButtonText.text = "���ӽ���";
            }
            else
            {
                gameStartButtonText.text = "�غ�";
            }
#endif

        }
        else
        {
            gameStartButton.interactable = true;

        }
    }

    public void initialize()
    {
#if �׽�Ʈ��
            playerPanel.kickButton.gameObject.SetActive(true);
#endif

#if ȣ��Ʈ�Ǵ�
        if (PhotonNetwork.LocalPlayer.IsMasterClient && !player.IsLocal)
        {
            playerPanel.kickButton.gameObject.SetActive(true);
        }
        else
        {
            playerPanel.kickButton.gameObject.SetActive(false);
        }
#endif

    }

    [PunRPC]
    public void SetRoomInfo(RoomOptions _roomOptions)
    {
        // �� �̸�
        RoomNameText.text = "# " + PhotonNetwork.CurrentRoom.CustomProperties["RoomName"];

        // �� Ÿ��
        if (PhotonNetwork.CurrentRoom.CustomProperties["Password"] == null)
        {
            RoomTypeText.text = "������";
        }
        else
        {
            RoomTypeText.text = "��й�";
        }

        // ���ӽ��۹�ư
        gameStartButton.GetComponentInChildren<TextMeshPro>().text = "�����";
    }

    public void OnClickStartButton()
    {
#if �׽�Ʈ��
                PhotonNetwork.LoadLevel("Peekaboo_InGame");
#endif

#if ȣ��Ʈ�Ǵ�
        if (PhotonNetwork.LocalPlayer.IsMasterClient && !player.IsLocal)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount > MinPlayercount)
            {
                gameObject.SetActive(false);
                PhotonNetwork.LoadLevel("Peekaboo_InGame");
            }
            else
            {
                Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "��� �÷��̾ �غ�Ϸ� ���°� �ƴմϴ�.", "Ȯ��");
            }
        }
        else
        {
            gameStartButtonText.text = "�غ�Ϸ�";

        }
#endif
    }

    public void OnClickExitButton()
    {
        exitRoomUI.gameObject.SetActive(true);
    }

    public void OnClickYesExitRoomButton()
    {
        PhotonNetwork.LeaveRoom();

        exitRoomUI.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void OnClickNoExitRoomButton()
    {
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
