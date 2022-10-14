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
    [SerializeField] Toggle isPrivateRoom;
    [SerializeField] TMP_InputField passwordInput;
    private string password = null;
    [SerializeField] GameObject grayText;

    public CustomProperties CustomProperties;
    private void Awake()
    {
        createButton.onClick.AddListener(OnClickCreateButton);
        exitButton.onClick.AddListener(OnClickExitButton);

        isPrivateRoom.isOn = false;
        grayText.SetActive(true);
        passwordInput.interactable = false;
    }

    private void Update()
    {
        if (passwordInput.text.Length > 0)
        {
            SetPassword();
        }

        if (isPrivateRoom.isOn)
        {
            // private 방을 만든다.
            grayText.SetActive(false); // 비밀번호 설정 가능
        passwordInput.interactable = true;
        }
        else
        {
            // public방을 만든다.
            grayText.SetActive(true);
        passwordInput.interactable = false;
        }
    }

    public void SetPassword()
    {
        password = passwordInput.text;
        Debug.Log(password);
    } 


    public void OnClickCreateButton()
    {
        gameObject.SetActive(false);

        if (isPrivateRoom.isOn)
        {
            //LobbyManager.Instance.CreateRoom(LobbyManager.Instance.SetRoomName() + "_" + password);;
        }
        else
        {
            //LobbyManager.Instance.CreateRoom(LobbyManager.Instance.SetRoomName());
        }
       // LobbyManager.Instance.OnCreatedRoom();
    }

    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }
}
