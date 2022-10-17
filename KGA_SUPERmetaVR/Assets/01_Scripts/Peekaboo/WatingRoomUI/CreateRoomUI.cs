using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


public class CreateRoomUI : MonoBehaviourPunCallbacks
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
            if(false) //  TODO : 유니티 다이얼 키패드 있는지 봐야함
            {
                // TODO : 나중에 데이터로 빼야함
                Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "비밀번호는 1자리 최대 8자리\n숫자만 사용 가능합니다.", "확인");
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


