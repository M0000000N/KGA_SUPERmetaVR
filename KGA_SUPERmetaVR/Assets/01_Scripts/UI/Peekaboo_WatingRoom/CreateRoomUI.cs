using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;



public class CreateRoomUI : MonoBehaviourPunCallbacks
{
    [Header("��ư")]
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
            // ��й���
        }
    }

    public void OnClickCreateButton()
    {
        // �� �����Ұ���
    }

    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }
}
