using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PKB_CreateRoomUI : MonoBehaviourPunCallbacks
{
    [Header("버튼")]
    [SerializeField] Button createButton;
    [SerializeField] Button exitButton;

    [Header("방 정보")]
    [SerializeField] Toggle privateRoom;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] GameObject grayText;
    private string password = null; // 인풋값 저장위함

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

            if (passwordInput.text.Length == 0)
            {
                createButton.interactable = false;
            }
            else
            {
                createButton.interactable = true;
            }
        }
        else
        {
            // publicRoom
            grayText.SetActive(true);
            passwordInput.text = "";
            passwordInput.interactable = false;
            createButton.interactable = true;
        }
    }

    public void OnClickCreateButton()
    {
        if (privateRoom.isOn)
        {
            // privateRoom
            if (false) //  TODO : 비밀번호 규칙에 어긋날 때(유니티 다이얼 키패드 있는지 봐야함)
            {
                // TODO : 나중에 데이터로 빼야함
                PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "비밀번호는 1자리 최대 8자리\n숫자만 사용 가능합니다.", "확인");
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
        gameObject.SetActive(false);
    }

    public void SetPassword()
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

}


