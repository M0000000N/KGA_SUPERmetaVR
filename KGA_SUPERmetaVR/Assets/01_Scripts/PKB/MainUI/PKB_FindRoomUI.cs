using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PKB_FindRoomUI : MonoBehaviourPunCallbacks
{
    [Header("방찾기")]
    [SerializeField] TMP_InputField roomNameInput;
    [SerializeField] Button findButton;
    [SerializeField] Button exitButton;

    [Header("비밀번호 입력")]
    [SerializeField] GameObject pwUI;
    [SerializeField] Button pwFindButton;
    [SerializeField] Button pwExitButton;
    [SerializeField] TMP_InputField pwInput;

    private void Awake()
    {
        findButton.onClick.AddListener(OnClickFindButton);
        exitButton.onClick.AddListener(OnClickExitButton);
        pwFindButton.onClick.AddListener(OnClickPwFindButton);
        pwExitButton.onClick.AddListener(OnClickPwExitButton);
    }

    private void Start()
    {
        Initionalize();
    }

    private void Update()
    {
        if (pwUI.activeSelf)
        {
            SetPassword();
        }
    }

    private void Initionalize()
    {
        roomNameInput.text = "";
        roomNameInput.interactable = false;
        SetPasswordInputUI(false);
    }

    private void SetPasswordInputUI(bool _isActive)
    {
        pwUI.SetActive(_isActive);
        pwInput.text = "";
        pwFindButton.interactable = false;
    }

    private void SetPassword()
    {
        if (pwInput.text.Length == 0)
        {
            pwFindButton.interactable = false;
            pwInput.interactable = true;
        }
        else if (pwInput.text.Length <= 6)
        {
            pwFindButton.interactable = true;
        }
        else
        {
            // TODO : 나중에 데이터로 빼야함
            PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "비밀번호는 최대 6자리\n숫자만 사용 가능합니다.", "확인");
            pwInput.text = "";
            return;
        }
    }

    public void OnClickFindButton()
    {
        if (roomNameInput.text.Equals("00")) // 로비찾기 금지
        {
            // TODO : 나중에 데이터로 빼야함
            PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("알림",
                "존재하지 않는 방 번호입니다.\n다시 한번 확인해주세요.", "확인");
            return;
        }
        if (LobbyManager.Instance.NowRooms.Count != 0)
        {
            // 방이 존재할 시 모든 방 탐색하면서 찾기
            for (int i = 0; i < LobbyManager.Instance.NowRooms.Count; i++)
            {
                if (LobbyManager.Instance.NowRooms[i].CustomProperties["RoomName"].ToString().Contains(roomNameInput.text))
                {
                    if (LobbyManager.Instance.NowRooms[i].CustomProperties["Password"] == null)
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
                    return;
                }
            }
            // TODO : 나중에 데이터로 빼야함
            PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("알림",
                "존재하지 않는 방 번호입니다.\n다시 한번 확인해주세요.", "확인");
        }
        else // 방이 하나도 없을 때는 없음 (로비에 마스터클라이언트가 남아있다는 가정)
        {
            // TODO : 나중에 데이터로 빼야함
            PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("알림",
                "존재하지 않는 방 번호입니다.\n다시 한번 확인해주세요.", "확인");
        }
    }

    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
        Initionalize();
    }

    public void OnClickPwFindButton()
    {
        for (int i = 0; i < LobbyManager.Instance.NowRooms.Count; i++)
        {
            if (LobbyManager.Instance.NowRooms[i].CustomProperties["Password"] == null) continue;
            if (LobbyManager.Instance.NowRooms[i].CustomProperties["Password"].ToString().Equals(pwInput.text))
            {
                PhotonNetwork.JoinRoom(roomNameInput.text);
                Initionalize();
            }
            else
            {
                // TODO : 나중에 데이터로 빼야함
                PKB_MainUIManager.Instance.NoticePopupUI.SetNoticePopup("알림",
                    "비밀번호가 일치하지 않습니다.\n다시 한번 확인해주세요.", "확인");
                SetPasswordInputUI(true);
            }
        }
    }

    public void OnClickPwExitButton()
    {
        SetPasswordInputUI(false);
    }
}
