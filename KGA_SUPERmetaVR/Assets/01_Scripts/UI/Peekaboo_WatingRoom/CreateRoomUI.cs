using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;



public class CreateRoomUI : MonoBehaviourPunCallbacks
{
    [Header("버튼")]
    [SerializeField] Button CreateButton;
    [SerializeField] Button ExitButton;

    [SerializeField] Toggle isPrivateRoom;

    private void Awake()
    {
        CreateButton.onClick.AddListener(OnClickCreateButton);
        ExitButton.onClick.AddListener(OnClickExitButton);

        isPrivateRoom.isOn = false;
    }

    private void Update()
    {
        if(isPrivateRoom.isOn)
        {
            // 비밀방임
        }
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
