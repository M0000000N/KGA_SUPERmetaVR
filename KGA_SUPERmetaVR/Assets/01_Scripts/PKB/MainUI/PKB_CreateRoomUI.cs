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
        Initionalize();
    }
    private void Initionalize()
    {
        privateRoom.isOn = false;
        passwordInput.interactable = false;
        grayText.SetActive(true);
        passwordInput.text = "";
    }

    private void Update()
    {
        if(gameObject.activeSelf)
        {
            if (privateRoom.isOn)
            {
                // privateRoom
                grayText.SetActive(false);
                SetPassword();
            }
            else
            {
                // publicRoom
                Initionalize();
                createButton.interactable = true;
            }
        }
    }

    private void SetPassword()
    {
        if (passwordInput.text.Length == 0)
        {
            createButton.interactable = false;
            passwordInput.interactable = true;
        }
        else if(passwordInput.text.Length <= 6)
        {
            createButton.interactable = true;
        }
        else
        {
            // TODO : 나중에 데이터로 빼야함
            PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "비밀번호는 최대 6자리\n숫자만 사용 가능합니다.", "확인");
            passwordInput.text = "";
            return;
        }
        password = passwordInput.text;
    }

    public void OnClickCreateButton()
    {
        if (privateRoom.isOn)
        {
            LobbyManager.Instance.JoinOrCreateRoom(password);
        }
        else
        {
            LobbyManager.Instance.JoinOrCreateRoom();
        }
        gameObject.SetActive(false);
        Initionalize();
    }

    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
        Initionalize();
    }
}


