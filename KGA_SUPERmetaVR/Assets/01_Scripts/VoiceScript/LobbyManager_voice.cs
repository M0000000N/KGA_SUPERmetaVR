using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun.Demo.Cockpit;
using System.Linq;

//마스터서버를 먼저 생성한 다음 룸으로 접속하는 구조
//마스터 서버와 룸 접속 담당
public class LobbyManager_voice : MonoBehaviourPunCallbacks
{
    public Text connectInfoText;
    public Button joinButton;

    public List<Region> RegionsList;


    //포톤 접속 게임버전 설정
    string gameVersion = "1";

    // Start is called before the first frame update
    void Start()
    {
        SettingInit();

        //마스터 서버 접속
        string label = "마스터 서버에 접속중...";
        MasterServerConnect(label);
    }

    #region CustomFuntion

    //셋팅 초기화
    void SettingInit()
    {
        //접속 게임버전 설정
        PhotonNetwork.GameVersion = gameVersion;

        joinButton.onClick.AddListener(() =>
        {
            Connect();
        });
    }

    //마스터 서버 접속
    void MasterServerConnect(string _connectInfoText)
    {
        //마스터 서버 접속시도
        PhotonNetwork.ConnectUsingSettings();
        //PhotonNetwork.ConnectToBestCloudServer();
        joinButton.interactable = false;
        connectInfoText.text = _connectInfoText;
    }

    //룸 접속 시도
    public void Connect()
    {
        //룸 접속버튼 비활성화
        joinButton.interactable = false;

        //마스터 서버에 접속중이라면
        if (PhotonNetwork.IsConnected)
        {
            connectInfoText.text = "룸에 접속...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            //마스터 서버에 접속중이 아니라면, 마스터 서버에 접속 시도
            string label = "오프라인 : 마스터 서버와 연결되지 않음\n접속 재시도 중...";
            //마스터 서버로의 재접속 시도
            MasterServerConnect(label);
        }
    }

    #endregion
    #region override

    //마스터 서버 접속 연결
    public override void OnConnectedToMaster()
    {
        //룸 접속 버튼 활성화
        joinButton.interactable = true;
        connectInfoText.text = "온라인 : 마스터 서버와 연결됨";

        Debug.Log(PhotonNetwork.PlayerList.Length);
    }

    //마스터 서버와 연결이 끊어짐
    public override void OnDisconnected(DisconnectCause cause)
    {
        string label = "오프라인 : 마스터 서버와 연결되지 않음\n접속 재시도 중...";

        //서버 재접속 시도
        MasterServerConnect(label);
    }

    //랜덤 방 접속실패
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectInfoText.text = "새로운방을 만드는중...";
        //maxplayer 제한이 없는 방 생성
        //UserId 적용
        RoomOptions options = new RoomOptions();
        options.PublishUserId = true;
        PhotonNetwork.CreateRoom(null, options);
    }

    //랜덤 방 접속 성공
    public override void OnJoinedRoom()
    {
        connectInfoText.text = "방 참가 성공";

        PhotonNetwork.LoadLevel("Ver.1_Lobby");
    }
    #endregion
}
