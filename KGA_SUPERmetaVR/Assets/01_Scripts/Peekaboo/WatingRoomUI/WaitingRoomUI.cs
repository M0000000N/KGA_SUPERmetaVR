using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class WaitingRoomUI : MonoBehaviourPunCallbacks
{
    [Header("DB에서 가져올 것")]
    [SerializeField] TextMeshProUGUI coin;
    [SerializeField] TextMeshProUGUI nickname;

    [Header("버튼")]
    [SerializeField] Button exitButton;
    [SerializeField] Button findRoomButton;
    [SerializeField] Button customizingButton;
    [SerializeField] Button randomJoinButton;
    [SerializeField] Button createRoomButton;

    [Header("방 찾기")]
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

        // 마스터 서버에 접속중이라면 룸 접속 실행
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
        // 포톤 PhotonNetwork.JoinRandomRoom() 중에 멈출 수 있는 방안 모색
        //PhotonNetwork.LeaveRoom();
    }
}
