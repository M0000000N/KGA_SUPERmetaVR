using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class PKB_FindRoomUI : MonoBehaviourPunCallbacks
{
    [Header("방찾기")]
    [SerializeField] TMP_InputField roomNameInput;
    [SerializeField] Button findButton;
    [SerializeField] Button exitButton;

    [Header("비밀번호 입력")]
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

    public void SetPassword() // TODO : 번호 규칙, CreateRoomUI 코드중복 리펙토링
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
        // PhotonNetwork.GetCustomRoomList();

        if (roomNameInput.text.Contains(LobbyManager.Instance.NowRooms[int.Parse(roomNameInput.text) - 1].CustomProperties["RoomName"].ToString()))
        {
            if (null == LobbyManager.Instance.NowRooms[int.Parse(roomNameInput.text) - 1].CustomProperties["Password"].ToString())
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
        }
        else // 없는 방
        {
            // TODO : 나중에 데이터로 빼야함
            PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("알림",
                "존재하지 않는 방 번호입니다.\n다시 한번 확인해주세요.", "확인");
        }
    }

    public void OnClickCheckButton()
    {
        if (passwordInput.text.Equals(LobbyManager.Instance.NowRooms[int.Parse(roomNameInput.text) - 1].CustomProperties["Password"].ToString()))
        {
            PhotonNetwork.JoinRoom(roomNameInput.text);
        }
        else
        {
            // TODO : 나중에 데이터로 빼야함
            PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("알림",
                "비밀번호가 일치하지 않습니다.\n비밀번호는 1자리 최대 8자리\n숫자만 사용 가능합니다.", "확인");
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
