using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class CreateRoomUI : MonoBehaviourPunCallbacks
{
    [Header("버튼")]
    [SerializeField] Button createButton;
    [SerializeField] Button exitButton;

    [Header("방 정보")]
    [SerializeField] Toggle isPrivateRoom;
    [SerializeField] TMP_InputField roomNumInput;
    [SerializeField] GameObject grayText;
    private string roomNum = null;

    private void Awake()
    {
        createButton.onClick.AddListener(OnClickCreateButton);
        exitButton.onClick.AddListener(OnClickExitButton);

        isPrivateRoom.isOn = false;
        grayText.SetActive(true);

        roomNum = roomNumInput.text;
    }

    private void Update()
    {
        if (roomNum.Length > 0 && Input.GetKeyDown(KeyCode.Return))
        {
            RooomName();
        }

        if(isPrivateRoom.isOn)
        {
            grayText.SetActive(false);
        }
        else
        {
            grayText.SetActive(true);
        }
    }

    public void RooomName()
    {
        roomNum = roomNumInput.text;
        PlayerPrefs.SetString("CurrentRoomName", roomNum);
        Debug.Log(roomNum);
    }
    public void OnClickCreateButton()
    {
        // 방 생성할거임
    }

    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }
}
