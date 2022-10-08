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
    [SerializeField] Button FindButton;
    [SerializeField] Button ExitButton;

    [Header("비밀번호 입력")]
    [SerializeField] GameObject passwordInputUI;
    [SerializeField] Button checkButton;
    [SerializeField] Button cancelButton;
    private TMP_InputField passWord;

    [Header("알림")]
    [SerializeField] GameObject alramUI;
    private Button yesButton;

    private void Awake()
    {
        FindButton.onClick.AddListener(OnClickFindButton);
        ExitButton.onClick.AddListener(OnClickExitButton);
    }

    public void OnClickFindButton()
    {
        // 방 찾을거임
    }

    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }
}
