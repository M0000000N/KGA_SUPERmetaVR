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
                UnityEngine.Debug.Log("방 입장.");
            }
            else
            {
                Peekaboo_WatingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "방에 입장할 수 없습니다.", "확인"); // TODO : 나중에 데이터로 빼야함
            }
            passwordInputUI.SetActive(false);
        }
        else
        {
            if(PhotonNetwork.JoinRoom(roomNumber.text))
            {
                UnityEngine.Debug.Log("방 입장.");
            }
            else
            {
                passwordInputUI.SetActive(true);
            }
        }
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        switch (returnCode) // TODO : 나중에 데이터로 빼야함
        {
            case -1:
                // 서버에 문제가 발생했습니다. 재생산을 시도하고 Exit Game에 문의하십시오.
                Peekaboo_WatingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "서버에 문제가 발생했습니다. 다시 시도해 주세요", "확인");
                break;
            case 32765:
                // 게임이 꽉 찼습니다. 참가가 완료되기 전에 일부 플레이어가 방에 참가한 경우에는 거의 발생하지 않습니다.
                Peekaboo_WatingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "게임이 꽉 찼습니다.", "확인");
                break;
            case 32764:
                // 게임이 종료되어 참가할 수 없습니다. 다른 게임에 참여하세요.
                Peekaboo_WatingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "게임이 종료되어 참가할 수 없습니다. 다른 게임에 참여하세요.", "확인");
                break;
            case 32760:
                // 무작위 매치메이킹은 닫히거나 꽉 차지 않은 방이 있는 경우에만 성공합니다. 몇 초 후에 반복하거나 새 방을 만드십시오.
                Peekaboo_WatingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "방에 들어갈 수 없습니다.", "확인");
                break;
            default:
                // 아무튼 방에 참가하지 못 했다.
                Peekaboo_WatingRoomUIManager.Instance.NoticePopupUI.SetNoticePopup("알림", "오류가 발생했습니다.", "확인");
                break;
        }
    }

    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }


}
