using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class FindRoomUI : MonoBehaviourPunCallbacks
{
    [Header("방찾기")]
    [SerializeField] TMP_InputField roomNumber;
    [SerializeField] Button findButton;
    [SerializeField] Button exitButton;

    [Header("비밀번호 입력")]
    [SerializeField] GameObject passwordUI;
    [SerializeField] Button checkButton;
    [SerializeField] Button cancleButton;
    [SerializeField] TMP_InputField passwordInput;
    private string password = null; // 인풋값 저장위함

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

    public void SetPassword() // TODO : 번호 규칙, CreateRoomUI 코드중복 리펙토링
    {
        if (passwordInput.text.Length > 8)
        {
            passwordInput.interactable = false;
        }
        password = passwordInput.text;
    }

    public void OnClickFindButton()
    {
        if(true) // TODO : 있는 방
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
        else // 없는 방
        {
            // TODO : 나중에 데이터로 빼야함
            Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "존재하지 않는 방 번호입니다.\n다시 한번 확인해주세요.", "확인");
        }
    }

    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }

    public void OnClickCheckButton()
    {
        if(true) // TODO : passWord.text == 커스텀프로퍼티 비번
        {
            PhotonNetwork.JoinRoom(roomNumber.text);
        }
        else
        {
            // TODO : 나중에 데이터로 빼야함
            Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "비밀번호가 일치하지 않습니다.\n비밀번호는 1자리 최대 8자리\n숫자만 사용 가능합니다.", "확인");
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
