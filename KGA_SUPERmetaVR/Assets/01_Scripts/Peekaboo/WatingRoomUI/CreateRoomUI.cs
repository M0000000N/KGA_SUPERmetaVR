using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class CreateRoomUI : MonoBehaviourPunCallbacks
{
    [Header("버튼")]
    [SerializeField] Button createButton;
    [SerializeField] Button exitButton;

    [Header("방 정보")]
    [SerializeField] Toggle isPrivateRoom;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] GameObject grayText;
    private string password = null; // 인풋값 저장위함

    private void Awake()
    {
        createButton.onClick.AddListener(OnClickCreateButton);
        exitButton.onClick.AddListener(OnClickExitButton);

        isPrivateRoom.isOn = false;
        passwordInput.interactable = false;
        grayText.SetActive(true);
    }

    private void Update()
    {
        if (passwordInput.text.Length > 0) // 번호 입력 시작 할 대 set시작
        {
            SetPassword();
        }

        if (isPrivateRoom.isOn)
        {
            // publicRoom
            grayText.SetActive(true);
            passwordInput.interactable = false;
        }
        else
        {
            // privateRoom
            grayText.SetActive(false);
            passwordInput.interactable = true;
        }
    }

    public void SetPassword() // TODO : 번호 규칙
    {
        // if()
        password = passwordInput.text;
        Debug.Log(password);
    }

    public void OnClickCreateButton()
    {
        gameObject.SetActive(false);

        if (isPrivateRoom.isOn)
        {
            // publicRoom
            LobbyManager.Instance.CreateRoom(CustomRoomOptions(false, null));
        }
        else
        {
            // privateRoom
            if(false)// 비밀번호 규칙과 다를 경우
            {
                // TODO : 나중에 데이터로 빼야함
                Peekaboo_WaitingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "비밀번호는 1자리 최대 8자리\n숫자만 사용 가능합니다.", "확인");
                passwordInput.text = "";
                return;
            }
            LobbyManager.Instance.CreateRoom(CustomRoomOptions(true, password));
        }
    }

    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }


    public RoomOptions CustomRoomOptions(bool _isprivate, string _password)
    {
        RoomOptions roomOptions = new RoomOptions()
        {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = 14
        };

        roomOptions.CustomRoomProperties = new Hashtable()
        {
            { "isPrivate", _isprivate },
            { "password", _password }
        };

        roomOptions.CustomRoomPropertiesForLobby = new string[]
        {
            "isPrivate", "password"
        };
        return roomOptions;
    }
}


