using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class FindRoomUI : MonoBehaviourPunCallbacks
{
    [Header("��ã��")]
    [SerializeField] TMP_InputField roomNumber;
    [SerializeField] Button FindButton;
    [SerializeField] Button ExitButton;

    [Header("��й�ȣ �Է�")]
    [SerializeField] GameObject passwordInputUI;
    [SerializeField] Button checkButton;
    [SerializeField] Button cancelButton;
    private TMP_InputField passWord;

    [Header("�˸�")]
    [SerializeField] GameObject alramUI;
    private Button yesButton;

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
