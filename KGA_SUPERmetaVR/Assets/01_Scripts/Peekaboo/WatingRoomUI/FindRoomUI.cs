using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class FindRoomUI : MonoBehaviourPunCallbacks
{
    [Header("��ã��")]
    [SerializeField] TMP_InputField roomNumber;
    [SerializeField] Button findButton;
    [SerializeField] Button exitButton;

    [Header("��й�ȣ �Է�")]
    [SerializeField] GameObject passwordInputUI;
    [SerializeField] Button checkButton;
    [SerializeField] Button cancleButton;
    private TMP_InputField passWord;

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

    public void OnClickFindButton()
    {
        if(true) // �����ϴ� ���� ��
        {
            if (true) // publicRoom
            {
                PhotonNetwork.JoinRoom(roomNumber.text);
                OnClickExitButton();
            }
            else // privateRoom
            {
                SetPasswordInputUI(true);
            }
        }
        else
        {
            Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "�������� �ʴ� �� ��ȣ�Դϴ�.\n�ٽ� �ѹ� Ȯ�����ּ���.", "Ȯ��"); // TODO : ���߿� �����ͷ� ������
        }
    }

    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }

    public void OnClickCheckButton()
    {
        if(true) // passWord.text == Ŀ����������Ƽ ���
        {
            PhotonNetwork.JoinRoom(roomNumber.text);
        }
        else
        {
            Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "��й�ȣ�� ��ġ���� �ʽ��ϴ�.\n�ٽ� �ѹ� Ȯ�����ּ���.", "Ȯ��"); // TODO : ���߿� �����ͷ� ������
        }
    }

    public void OnClickCancleButton()
    {
        SetPasswordInputUI(false);
    }

    private void SetPasswordInputUI(bool _isActive)
    {
        passwordInputUI.SetActive(_isActive);
    }

}
