using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


public class CreateRoomUI : MonoBehaviourPunCallbacks
{
    [Header("��ư")]
    [SerializeField] Button createButton;
    [SerializeField] Button exitButton;

    [Header("�� ����")]
    [SerializeField] Toggle privateRoom;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] GameObject grayText;
    private string password = null; // ��ǲ�� ��������

    private void Awake()
    {
        createButton.onClick.AddListener(OnClickCreateButton);
        exitButton.onClick.AddListener(OnClickExitButton);

        privateRoom.isOn = false;
        passwordInput.interactable = false;
        grayText.SetActive(true);
    }

    private void Update()
    {
        if (passwordInput.text.Length > 0)
        {
            SetPassword();
        }

        if (privateRoom.isOn)
        {
            // privateRoom
            grayText.SetActive(false);
            passwordInput.interactable = true;
        }
        else
        {
            // publicRoom
            grayText.SetActive(true);
            passwordInput.interactable = false;
        }
    }

    public void SetPassword()
    {
        if(passwordInput.text.Length > 8)
        {
            passwordInput.interactable = false;
        }
        password = passwordInput.text;
    }

    public void OnClickCreateButton()
    {
        gameObject.SetActive(false);

        if (privateRoom.isOn)
        {
            // privateRoom
            if(false) //  TODO : ����Ƽ ���̾� Ű�е� �ִ��� ������
            {
                // TODO : ���߿� �����ͷ� ������
                Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "��й�ȣ�� 1�ڸ� �ִ� 8�ڸ�\n���ڸ� ��� �����մϴ�.", "Ȯ��");
                passwordInput.text = "";
                return;
            }
            LobbyManager.Instance.CreateRoom(password);
        }
        else
        {
            // publicRoom
            LobbyManager.Instance.CreateRoom(null);
        }
    }

    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }

}


