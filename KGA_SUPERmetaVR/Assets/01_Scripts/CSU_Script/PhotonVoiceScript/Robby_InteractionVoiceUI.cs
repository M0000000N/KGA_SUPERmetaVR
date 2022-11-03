//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using Photon.Pun;
//using TMPro;
//using Photon.Realtime;
//using Hashtable = ExitGames.Client.Photon.Hashtable;
//using System.Linq;
//using UnityEngine.EventSystems;
//using UnityEditor;
//using Photon.Voice.PUN;

//// 1:1 대화창 

//// 상대방에게만 보이는 것 
//public class Robby_InteractionVoiceUI : MonoBehaviourPun
//{
//    //손을 누르면 
//    [Header("범위 확인")]
//    [SerializeField] float talkDistance = 2.5f;
//    [SerializeField] GameObject handShakeImage;

//    [Header("버튼")]
//    [SerializeField] Button oneOnoneTalk; // 1:1 대화창
//    [SerializeField] Button accept; // 대화 할거임
//    [SerializeField] Button refuse; // 대화 안할거임 

//    [Header("대화UI")]
//    [SerializeField] GameObject DoyouwannaTalk; // 1:1 대화 고르기
//    [SerializeField] GameObject SuccessPopUI; // 대화 할꺼임?
//    [SerializeField] GameObject MyVociePanel; // 내꺼 뜨기 

//    private PunVoiceClient punVoiceClient;

//    Collider collider;

//    private void Awake()
//    {
//        this.punVoiceClient = GetComponent<PunVoiceClient>();
//    }

//    private void OnEnable()
//    {
//        this.punVoiceClient.Client.StateChanged += this.VoiceClientStateChanged;
//        PhotonNetwork.NetworkingClient.StateChanged += this.PunClientStateChanged;
//    }

//    private void OnDisable()
//    {
//        this.punVoiceClient.Client.StateChanged -= this.VoiceClientStateChanged;
//        PhotonNetwork.NetworkingClient.StateChanged -= this.PunClientStateChanged;
//    }

//    private void Start()
//    {
//        if (photonView.IsMine)
//        {
//            handShakeImage.SetActive(false);
//            DoyouwannaTalk.SetActive(false);
//        }

//        handShakeImage.SetActive(false);
//        DoyouwannaTalk.SetActive(false);
//        MyVociePanel.SetActive(false);
//        SuccessPopUI.SetActive(false);
//        collider = GetComponent<Collider>();
//    }
//    private void Update()
//    {
//        OneOnOneConversation();
//    }

//    public void OneOnOneConversation()
//    {

//        if (!photonView.IsMine) return;

//        // 1:1 대화창 눌렀으면
//        // 대화 할거니? yes / no 나옴 
//        // 누를 수 있느냐 없느냐 bool 
//        if (oneOnoneTalk.interactable == true) // 버튼 눌렀으면 
//        {
//            SuccessPopUI.SetActive(true); // 대화 조인할거말거? 
//            // 승인버튼 눌렀으면 
//            if (accept.interactable == true)
//            {
//                // 나의 패널 활성화 
//                accept.onClick.AddListener(makeVoiceRoom);
//                MyVociePanel.SetActive(true);

//                // 나머지 팝업창 꺼짐 
//                SuccessPopUI.SetActive(false);
//                DoyouwannaTalk.SetActive(false);
//            }
//            else if (refuse.interactable == true)
//            {
//                // 팝업창 꺼짐 
//                SuccessPopUI.SetActive(false);
//            }
//        }
//        else
//            return;
//    }

//    public void ConnectinoVoiceRoom()
//    {
//        accept.onClick = punVoiceClient.AutoConnectAndJoin;
//    }

//    public void DisConnectionVoiceRoom()
//    {
//        punVoiceClient.AutoLeaveAndDisconnect;
//    }

//    private void PunSwitchOnClick()
//    {
//        if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined)
//        {

//            PhotonNetwork.Disconnect();
//        }
//        else if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Disconnected ||
//            PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.PeerCreated)
//        {
//            PhotonNetwork.ConnectUsingSettings();
//        }
//    }

//    private void VoiceSwitchOnClick()
//    {
//        if (this.punVoiceClient.ClientState == Photon.Realtime.ClientState.Joined)
//        {
//            this.punVoiceClient.Disconnect();
//        }
//        else if (this.punVoiceClient.ClientState == Photon.Realtime.ClientState.PeerCreated
//                 || this.punVoiceClient.ClientState == Photon.Realtime.ClientState.Disconnected)
//        {
//            this.punVoiceClient.ConnectAndJoinRoom();
//        }
//    }

//    private void CalibrateButtonOnClick()
//    {
//        if (this.recorder.RecorderInUse && !this.recorder.RecorderInUse.VoiceDetectorCalibrating)
//        {
//            this.recorder.RecorderInUse.VoiceDetectorCalibrate(this.calibrationMilliSeconds);
//        }
//        // 클릭하면 악수손 뜨기  
//        public void OnPointerClick(PointerEventData eventData)
//        {
//            if (Vector3.Distance(collider.transform.position, transform.position) < talkDistance)
//            {
//                if (collider.tag == "Player")
//                {
//                    handShakeImage.SetActive(true);
//                    Destroy(handShakeImage, 2f); // 2초 뒤 사라짐 
//                    DoyouwannaTalk.SetActive(true); // 1:1 대화창 선택할 수 있는 UI 뜸 
//                }
//            }
//        }

//        // 클릭 벗어나면 악수손 빠이 
//        public void OnPointerExit(PointerEventData eventData)
//        {
//            if (Vector3.Distance(collider.transform.position, transform.position) > talkDistance)
//            {
//                if (collider.tag == "Player")
//                    handShakeImage.SetActive(false);
//            }
//        }
//    }
