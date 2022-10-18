using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class PKB_FindRoomUI : MonoBehaviourPunCallbacks
{
    [Header("��ã��")]
    [SerializeField] TMP_InputField roomNameInput;
    private string roomName = null; // ��ǲ�� ��������
    [SerializeField] Button findButton;
    [SerializeField] Button exitButton;

    [Header("��й�ȣ �Է�")]
    [SerializeField] GameObject passwordUI;
    [SerializeField] Button checkButton;
    [SerializeField] Button cancleButton;
    [SerializeField] TMP_InputField passwordInput;
    private string password = null; // ��ǲ�� ��������

    private void Awake()
    {
        findButton.onClick.AddListener(OnClickFindButton);
        exitButton.onClick.AddListener(OnClickExitButton);
        checkButton.onClick.AddListener(OnClickCheckButton);
        cancleButton.onClick.AddListener(OnClickCancleButton);
    }

    private void Start()
    {
        SetPasswordInputUI(false);
    }

    private void Update()
    {
        if (passwordInput.text.Length > 0)
        {
            SetPassword();
        }
    }

    public void SetPassword() // TODO : ��ȣ ��Ģ, CreateRoomUI �ڵ��ߺ� �����丵
    {
        if (passwordInput.text.Length > 8)
        {
            passwordInput.interactable = false;
        }
        password = passwordInput.text;
    }
    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }

    public void OnClickFindButton()
    {
        roomName = roomNameInput.text;
        // PhotonNetwork.GetCustomRoomList();

        if (roomName.Contains(LobbyManager.Instance.NowRooms[int.Parse(roomName)].CustomProperties["RoomName"].ToString()))
        {
            if (null == LobbyManager.Instance.NowRooms[int.Parse(roomName)].CustomProperties["Password"].ToString())
            {
                // publicRoom
                PhotonNetwork.JoinRoom(roomName);
                OnClickExitButton();
            }
            else
            {
                // privateRoom
                SetPasswordInputUI(true);
            }
        }
        else // ���� ��
        {
            // TODO : ���߿� �����ͷ� ������
            PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�",
                "�������� �ʴ� �� ��ȣ�Դϴ�.\n�ٽ� �ѹ� Ȯ�����ּ���.", "Ȯ��");
        }
    }

    public void OnClickCheckButton()
    {
        if (password.Equals(LobbyManager.Instance.NowRooms[int.Parse(roomName)].CustomProperties["Password"].ToString()))
        {
            PhotonNetwork.JoinRoom(roomNameInput.text);
        }
        else
        {
            // TODO : ���߿� �����ͷ� ������
            PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�",
                "��й�ȣ�� ��ġ���� �ʽ��ϴ�.\n��й�ȣ�� 1�ڸ� �ִ� 8�ڸ�\n���ڸ� ��� �����մϴ�.", "Ȯ��");
        }
    }

    public void OnClickCancleButton()
    {
        SetPasswordInputUI(false);
    }

    private void SetPasswordInputUI(bool _isActive)
    {
        passwordUI.SetActive(_isActive);
    }

}
