using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PKB_FindRoomUI : MonoBehaviourPunCallbacks
{
    [Header("��ã��")]
    [SerializeField] TMP_InputField roomNameInput;
    [SerializeField] Button findButton;
    [SerializeField] Button exitButton;

    [Header("��й�ȣ �Է�")]
    [SerializeField] GameObject passwordUI;
    [SerializeField] Button checkButton;
    [SerializeField] Button cancleButton;
    [SerializeField] TMP_InputField passwordInput;

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
    }

    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }

    public void OnClickFindButton()
    {
        if (LobbyManager.Instance.NowRooms.Count != 0)
        {
            // ��� �� Ž���ϸ鼭 ã��
            for (int i = 0; i < LobbyManager.Instance.NowRooms.Count; i++)
            {
                if (roomNameInput.text.Contains(LobbyManager.Instance.NowRooms[i].CustomProperties["RoomName"].ToString()))
                {
                    if (null == LobbyManager.Instance.NowRooms[i].CustomProperties["Password"])
                    {
                        // publicRoom
                        PhotonNetwork.JoinRoom(roomNameInput.text);
                        OnClickExitButton();
                    }
                    else
                    {
                        // privateRoom
                        SetPasswordInputUI(true);
                    }
                    return;
                }
            }
            // TODO : ���߿� �����ͷ� ������
            PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�",
            "�������� �ʴ� �� ��ȣ�Դϴ�.\n�ٽ� �ѹ� Ȯ�����ּ���.", "Ȯ��");
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
        for (int i = 0; i < LobbyManager.Instance.NowRooms.Count; i++)
        {
            if(null != LobbyManager.Instance.NowRooms[i].CustomProperties["Password"])
            {
                if (passwordInput.text.Equals(LobbyManager.Instance.NowRooms[i].CustomProperties["Password"].ToString()))
                {
                    PhotonNetwork.JoinRoom(roomNameInput.text);
                }
                else
                {
                    // TODO : ���߿� �����ͷ� ������
                    PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�",
                        "��й�ȣ�� ��ġ���� �ʽ��ϴ�.\n�ٽ� �ѹ� Ȯ�����ּ���.", "Ȯ��");
                }
            }
        }
        passwordInput.text = "";
        SetPasswordInputUI(false);
    }

    public void OnClickCancleButton()
    {
        roomNameInput.text = "";
        SetPasswordInputUI(false);
    }

    private void SetPasswordInputUI(bool _isActive)
    {
        passwordUI.SetActive(_isActive);
    }
}
