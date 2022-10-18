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
    [SerializeField] TMP_InputField roomNumber;
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

    public void OnClickFindButton()
    {
        // PhotonNetwork.GetCustomRoomList();

        // TypedLobby typedLobby = 3;
        if (true) // TODO : �ִ� ��
        {
            if (true) //TODO : public, private

            {
                // privateRoom
                SetPasswordInputUI(true);
            }
            else
            {
                // publicRoom
                PhotonNetwork.JoinRoom(roomNumber.text);
                OnClickExitButton();
            }
        }
        else // ���� ��
        {
            // TODO : ���߿� �����ͷ� ������
            Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "�������� �ʴ� �� ��ȣ�Դϴ�.\n�ٽ� �ѹ� Ȯ�����ּ���.", "Ȯ��");
        }
    }

    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }

    //public override void OnRoomListUpdate(List<RoomInfo> roomInfos)
    //{
    //    // get room list info

    //    roomInfos.c
    //    // RoomInfo[] roomInfos = PhotonNetwork.GetRoomList();
    //    RoomListTableViewController roomListCon = GameObject.Find("RoomListTableViewController").GetComponent<RoomListTableViewController>();
    //    roomListCon.m_numRows = roomInfos.Length;
    //    if (roomInfos.Length > 0)
    //    {
    //        foreach (RoomInfo roomInfo in roomInfos)
    //        {
    //            roomListCon.roomNames.Add(roomInfo.name);

    //            // I try access GameTime and NumLife here
    //        }
    //    }
    //}

    public void OnClickCheckButton()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties[password] == password) // TODO : passWord.text == Ŀ����������Ƽ ���
        {
            PhotonNetwork.JoinRoom(roomNumber.text);
        }
        else
        {
            // TODO : ���߿� �����ͷ� ������
            Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "��й�ȣ�� ��ġ���� �ʽ��ϴ�.\n��й�ȣ�� 1�ڸ� �ִ� 8�ڸ�\n���ڸ� ��� �����մϴ�.", "Ȯ��");
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
