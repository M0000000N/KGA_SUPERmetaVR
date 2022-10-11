using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class WaitingRoomUI : MonoBehaviourPunCallbacks
{
    [Header("DB���� ������ ��")]
    [SerializeField] TextMeshProUGUI coin;
    [SerializeField] TextMeshProUGUI nickname;

    [Header("��ư")]
    [SerializeField] Button exitButton;
    [SerializeField] Button findRoomButton;
    [SerializeField] Button customizingButton;
    [SerializeField] Button randomJoinButton;
    [SerializeField] Button createRoomButton;

    [Header("�� ã��")]
    [SerializeField] GameObject findingRoomImage;
    private Button XButton;

    private void Awake()
    {
        // coin = DB.Coin
        // nickname = DB.Nickname
        findingRoomImage.SetActive(false);

        XButton = findingRoomImage.GetComponentInChildren<Button>();
    }

    private void Start()
    {
        exitButton.onClick.AddListener(OnClickExitButton);
        findRoomButton.onClick.AddListener(OnClickFindRoomButton);
        customizingButton.onClick.AddListener(OnClickCustomizingButton);
        randomJoinButton.onClick.AddListener(OnClickRandomJoinButton);
        createRoomButton.onClick.AddListener(OnClickCreateRoomButton);
        XButton.onClick.AddListener(OnClickXButton);        
    }

    public void OnClickExitButton()
    {
        Peekaboo_WatingRoomUIManager.Instance.ExitUI.gameObject.SetActive(true);
    }

    public void OnClickFindRoomButton()
    {
        Peekaboo_WatingRoomUIManager.Instance.FindRoomUI.gameObject.SetActive(true);
    }

    public void OnClickCustomizingButton()
    {
        Peekaboo_WatingRoomUIManager.Instance.CustomizingUI.gameObject.SetActive(true);
    }

    public void OnClickCreateRoomButton()
    {
        Peekaboo_WatingRoomUIManager.Instance.CreateRoomUI.gameObject.SetActive(true);
    }

    public void OnClickRandomJoinButton()
    {
        findingRoomImage.SetActive(true);

        // ������ ������ �������̶�� �� ���� ����
        //if (PhotonNetwork.IsConnected)
        //{
        //    //Data data = FindObjectOfType<Data>();
        //    //data.Nickname = nickname.text;
        //    //data.Coin = Coin.text;
        //    PhotonNetwork.JoinRandomRoom();
        //}
        //else
        //{
        //    PhotonNetwork.ConnectUsingSettings();
        //}
    }

    public void OnClickXButton()
    {
        findingRoomImage.SetActive(false);
        // ���� PhotonNetwork.JoinRandomRoom() �߿� ���� �� �ִ� ��� ���
        //PhotonNetwork.LeaveRoom();
    }
}
