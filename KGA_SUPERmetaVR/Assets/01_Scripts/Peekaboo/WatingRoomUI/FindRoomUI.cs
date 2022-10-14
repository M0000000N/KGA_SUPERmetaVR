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
        if(true) // 실존하는 룸일 때
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
            Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "존재하지 않는 방 번호입니다.\n다시 한번 확인해주세요.", "확인"); // TODO : 나중에 데이터로 빼야함
        }
    }

    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }

    public void OnClickCheckButton()
    {
        if(true) // passWord.text == 커스텀프로퍼티 비번
        {
            PhotonNetwork.JoinRoom(roomNumber.text);
        }
        else
        {
            Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "비밀번호가 일치하지 않습니다.\n다시 한번 확인해주세요.", "확인"); // TODO : 나중에 데이터로 빼야함
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
