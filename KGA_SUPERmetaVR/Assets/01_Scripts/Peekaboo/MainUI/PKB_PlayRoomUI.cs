#define ȣ��Ʈ�Ǵ�
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PKB_PlayRoomUI : MonoBehaviourPunCallbacks, IPunObservable
{
    public RoomInfo RoomInfo { get; private set; }
    public int MinPlayercount;

    [Header("PlayRoom")]
    [SerializeField] TextMeshProUGUI RoomNameText;
    [SerializeField] Button gameStartButton;
    private TextMeshProUGUI gameStartButtonText;
    [SerializeField] Button exitRoomButton;
    [SerializeField] TextMeshProUGUI RoomTypeText;

    [Header("ExitRoom")]
    [SerializeField] GameObject exitRoomUI;
    [SerializeField] Button YesExitRoomButton;
    [SerializeField] Button NoExitRoomButton;

    [Header("PlayerList")]
    [SerializeField] PKB_PlayerPanel playerPanel;

    bool playerIsReady;
    bool hostIsReady;
    bool isGameStart;

    private void Awake()
    {
        gameStartButton.onClick.AddListener(OnClickStartButton);
        exitRoomButton.onClick.AddListener(OnClickExitButton);
        exitRoomUI.gameObject.SetActive(false);
        YesExitRoomButton.onClick.AddListener(OnClickYesExitRoomButton);
        NoExitRoomButton.onClick.AddListener(OnClickNoExitRoomButton);

        gameStartButtonText = gameStartButton.GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        playerIsReady = false;
        hostIsReady = false;
        isGameStart = false;

        if (PhotonNetwork.IsConnected)
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
        else
        {
            gameStartButtonText.text = "�����";

        }
    }
    private void Update()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                gameStartButtonText.text = "���ӽ���";

                if (PhotonNetwork.PlayerList.Length > MinPlayercount) // TODO : ���� �غ�Ϸᰡ �ƴ���?
                {
                    hostIsReady = true;
                }
                else
                {
                    hostIsReady = false;
                }
                // playerPanel.kickButton.gameObject.SetActive(true);
            }
            else
            {
                gameStartButton.interactable = true;
                // playerPanel.kickButton.gameObject.SetActive(false);
            }
        }
        else
        {
            gameStartButton.interactable = false;
        }

        if (isGameStart)
        {
        }
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // ����ȭ -> �������� �����͸� ������ ��
        if (stream.IsWriting)
        {
            stream.SendNext(isGameStart);
        }
        else // ������ȭ -> �����κ��� �����͸� ���� ��
        {
            isGameStart = (bool)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void OnClickStartButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (hostIsReady)
            {
                isGameStart = true;
            PhotonNetwork.LoadLevel("Peekaboo_InGame");
            }
            else
            {
                PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "��� �÷��̾ �غ�Ϸ� ���°� �ƴմϴ�.", "Ȯ��");
            }
        }
        else
        {
            if (playerIsReady)
            {
                gameStartButtonText.text = "�غ�";
                playerIsReady = false;
            }
            else
            {
                gameStartButtonText.text = "�غ�Ϸ�";
                playerIsReady = true;
            }
        }
    }

    public void SetRoomInfo(RoomOptions _roomOptions)
    {
        // �� �̸�
        RoomNameText.text = "# " + System.String.Format("{0:0000}", int.Parse(PhotonNetwork.CurrentRoom.CustomProperties["RoomName"].ToString()));

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
