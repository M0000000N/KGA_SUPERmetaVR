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
    [SerializeField] Button findButton;
    [SerializeField] Button exitButton;

    [Header("��й�ȣ �Է�")]
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
        // �� ã������
    }

    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }
}
