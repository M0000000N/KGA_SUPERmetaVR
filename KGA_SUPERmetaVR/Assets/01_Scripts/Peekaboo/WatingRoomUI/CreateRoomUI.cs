using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class CreateRoomUI : MonoBehaviourPunCallbacks
{
    [Header("��ư")]
    [SerializeField] Button createButton;
    [SerializeField] Button exitButton;

    [Header("�� ����")]
    [SerializeField] Toggle isPrivateRoom;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] GameObject grayText;
    private string password = null; // ��ǲ�� ��������

    private void Awake()
    {
        createButton.onClick.AddListener(OnClickCreateButton);
        exitButton.onClick.AddListener(OnClickExitButton);

        isPrivateRoom.isOn = false;
        passwordInput.interactable = false;
        grayText.SetActive(true);
    }

    private void Update()
    {
        if (passwordInput.text.Length > 0) // ��ȣ �Է� ���� �� �� set����
        {
            SetPassword();
        }

        if (isPrivateRoom.isOn)
        {
            // publicRoom
            grayText.SetActive(true);
            passwordInput.interactable = false;
        }
        else
        {
            // privateRoom
            grayText.SetActive(false);
            passwordInput.interactable = true;
        }
    }

    public void SetPassword() // TODO : ��ȣ ��Ģ
    {
        // if()
        password = passwordInput.text;
        Debug.Log(password);
    }

    public void OnClickCreateButton()
    {
        gameObject.SetActive(false);

        if (isPrivateRoom.isOn)
        {
            // publicRoom
            LobbyManager.Instance.CreateRoom(CustomRoomOptions(false, null));
        }
        else
        {
            // privateRoom
            if(false)// ��й�ȣ ��Ģ�� �ٸ� ���
            {
                // TODO : ���߿� �����ͷ� ������
                Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "��й�ȣ�� 1�ڸ� �ִ� 8�ڸ�\n���ڸ� ��� �����մϴ�.", "Ȯ��");
                passwordInput.text = "";
                return;
            }
            LobbyManager.Instance.CreateRoom(CustomRoomOptions(true, password));
        }
    }

    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }


    public RoomOptions CustomRoomOptions(bool _isprivate, string _password)
    {
        RoomOptions roomOptions = new RoomOptions()
        {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = 14
        };

        roomOptions.CustomRoomProperties = new Hashtable()
        {
            { "isPrivate", _isprivate },
            { "password", _password }
        };

        roomOptions.CustomRoomPropertiesForLobby = new string[]
        {
            "isPrivate", "password"
        };
        return roomOptions;
    }
}


