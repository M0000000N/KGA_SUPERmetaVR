using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class FindRoomUI : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField roomNumber;
    [SerializeField] Button FindButton;
    [SerializeField] Button ExitButton;
    private void Awake()
    {
        FindButton.onClick.AddListener(OnClickFindButton);
        ExitButton.onClick.AddListener(OnClickExitButton);
    }

    public void OnClickFindButton()
    {
        // �� ã������
    }

    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }
}
