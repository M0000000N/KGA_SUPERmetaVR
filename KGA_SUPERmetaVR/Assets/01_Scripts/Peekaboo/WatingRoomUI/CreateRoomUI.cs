using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoomUI : MonoBehaviourPunCallbacks
{
    [Header("��ư")]
    [SerializeField] Button createButton;
    [SerializeField] Button exitButton;

    [Header("�� ����")]
    [SerializeField] Toggle isPrivateRoom;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] GameObject grayText;
    private string password = null;
    public RoomInfo RoomInfo { get; private set; }
    private void Awake()
    {
        createButton.onClick.AddListener(OnClickCreateButton);
        exitButton.onClick.AddListener(OnClickExitButton);

        isPrivateRoom.isOn = false;
        grayText.SetActive(true);

        //password = passwordInput.text;
    }

    private void Update()
    {
        if (passwordInput.text.Length > 0)
        {
            SetPassword();
        }

        if (isPrivateRoom.isOn)
        {
            // private ���� �����.
            grayText.SetActive(false);
            Peekaboo_WatingRoomUIManager.Instance.IsPrivateRoom = true;

        }
        else
        {
            // public���� �����.
            grayText.SetActive(true);
            Peekaboo_WatingRoomUIManager.Instance.IsPrivateRoom = false;

        }
    }

    public void SetPassword()
    {
        password = passwordInput.text;
        Debug.Log(password);
    } 


    public void OnClickCreateButton()
    {
        if (isPrivateRoom.isOn)
        {
            LobbyManager.Instance.CreateRoom(LobbyManager.Instance.SetRoomName() + "_" + password);
        }
        else
        {
            LobbyManager.Instance.CreateRoom(LobbyManager.Instance.SetRoomName());
        }
       // LobbyManager.Instance.OnCreatedRoom();
    }

    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }
}
