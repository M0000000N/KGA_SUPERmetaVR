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
    [SerializeField] Button cancelButton;
    private TMP_InputField passWord;

    private void Awake()
    {
        findButton.onClick.AddListener(OnClickFindButton);
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    private void Start()
    {
        passwordInputUI.SetActive(false);
    }

    public void OnClickFindButton()
    {
        if(passwordInputUI.activeSelf)
        {
            if (PhotonNetwork.JoinRoom(roomNumber.text + "_" + passWord.text))
            {
                UnityEngine.Debug.Log("�� ����.");
            }
            else
            {
                Peekaboo_WatingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "�濡 ������ �� �����ϴ�.", "Ȯ��"); // TODO : ���߿� �����ͷ� ������
            }
            passwordInputUI.SetActive(false);
        }
        else
        {
            if(PhotonNetwork.JoinRoom(roomNumber.text))
            {
                UnityEngine.Debug.Log("�� ����.");
            }
            else
            {
                passwordInputUI.SetActive(true);
            }
        }
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        switch (returnCode) // TODO : ���߿� �����ͷ� ������
        {
            case -1:
                // ������ ������ �߻��߽��ϴ�. ������� �õ��ϰ� Exit Game�� �����Ͻʽÿ�.
                Peekaboo_WatingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "������ ������ �߻��߽��ϴ�. �ٽ� �õ��� �ּ���", "Ȯ��");
                break;
            case 32765:
                // ������ �� á���ϴ�. ������ �Ϸ�Ǳ� ���� �Ϻ� �÷��̾ �濡 ������ ��쿡�� ���� �߻����� �ʽ��ϴ�.
                Peekaboo_WatingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "������ �� á���ϴ�.", "Ȯ��");
                break;
            case 32764:
                // ������ ����Ǿ� ������ �� �����ϴ�. �ٸ� ���ӿ� �����ϼ���.
                Peekaboo_WatingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "������ ����Ǿ� ������ �� �����ϴ�. �ٸ� ���ӿ� �����ϼ���.", "Ȯ��");
                break;
            case 32760:
                // ������ ��ġ����ŷ�� �����ų� �� ���� ���� ���� �ִ� ��쿡�� �����մϴ�. �� �� �Ŀ� �ݺ��ϰų� �� ���� ����ʽÿ�.
                Peekaboo_WatingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "�濡 �� �� �����ϴ�.", "Ȯ��");
                break;
            default:
                // �ƹ�ư �濡 �������� �� �ߴ�.
                Peekaboo_WatingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("�˸�", "������ �߻��߽��ϴ�.", "Ȯ��");
                break;
        }
    }

    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }


}
