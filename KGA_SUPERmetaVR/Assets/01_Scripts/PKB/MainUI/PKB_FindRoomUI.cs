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
    [SerializeField] GameObject pwUI;
    [SerializeField] Button pwFindButton;
    [SerializeField] Button pwExitButton;
    [SerializeField] TMP_InputField pwInput;

    private void Awake()
    {
        findButton.onClick.AddListener(OnClickFindButton);
        exitButton.onClick.AddListener(OnClickExitButton);
        pwFindButton.onClick.AddListener(OnClickPwFindButton);
        pwExitButton.onClick.AddListener(OnClickPwExitButton);
    }

    private void Start()
    {
        Initionalize();
    }

    private void Update()
    {
        if (pwUI.activeSelf)
        {
            SetPassword();
        }
    }

    private void Initionalize()
    {
        roomNameInput.text = "";
        roomNameInput.interactable = false;
        SetPasswordInputUI(false);
    }

    private void SetPasswordInputUI(bool _isActive)
    {
        pwUI.SetActive(_isActive);
        pwInput.text = "";
        pwFindButton.interactable = false;
    }

    private void SetPassword()
    {
        if (pwInput.text.Length == 0)
        {
            pwFindButton.interactable = false;
            pwInput.interactable = true;
        }
        else if (pwInput.text.Length <= 6)
        {
            pwFindButton.interactable = true;
        }
        else
        {
            // TODO : ���߿� �����ͷ� ������
            PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "��й�ȣ�� �ִ� 6�ڸ�\n���ڸ� ��� �����մϴ�.", "Ȯ��");
            pwInput.text = "";
            return;
        }
    }

    public void OnClickFindButton()
    {
        if (roomNameInput.text.Equals("00")) // �κ�ã�� ����
        {
            // TODO : ���߿� �����ͷ� ������
            PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�",
                "�������� �ʴ� �� ��ȣ�Դϴ�.\n�ٽ� �ѹ� Ȯ�����ּ���.", "Ȯ��");
            return;
        }
        if (LobbyManager.Instance.NowRooms.Count != 0)
        {
            // ���� ������ �� ��� �� Ž���ϸ鼭 ã��
            for (int i = 0; i < LobbyManager.Instance.NowRooms.Count; i++)
            {
                if (LobbyManager.Instance.NowRooms[i].CustomProperties["RoomName"].ToString().Contains(roomNameInput.text))
                {
                    if (LobbyManager.Instance.NowRooms[i].CustomProperties["Password"] == null)
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
        else // ���� �ϳ��� ���� ���� ���� (�κ� ������Ŭ���̾�Ʈ�� �����ִٴ� ����)
        {
            // TODO : ���߿� �����ͷ� ������
            PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�",
                "�������� �ʴ� �� ��ȣ�Դϴ�.\n�ٽ� �ѹ� Ȯ�����ּ���.", "Ȯ��");
        }
    }

    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
        Initionalize();
    }

    public void OnClickPwFindButton()
    {
        for (int i = 0; i < LobbyManager.Instance.NowRooms.Count; i++)
        {
            if (LobbyManager.Instance.NowRooms[i].CustomProperties["Password"] == null) continue;
            if (LobbyManager.Instance.NowRooms[i].CustomProperties["Password"].ToString().Equals(pwInput.text))
            {
                PhotonNetwork.JoinRoom(roomNameInput.text);
                Initionalize();
            }
            else
            {
                // TODO : ���߿� �����ͷ� ������
                PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�",
                    "��й�ȣ�� ��ġ���� �ʽ��ϴ�.\n�ٽ� �ѹ� Ȯ�����ּ���.", "Ȯ��");
                SetPasswordInputUI(true);
            }
        }
    }

    public void OnClickPwExitButton()
    {
        SetPasswordInputUI(false);
    }
}
