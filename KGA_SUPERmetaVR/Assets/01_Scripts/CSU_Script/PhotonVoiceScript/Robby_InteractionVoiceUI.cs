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

//// 1:1 ��ȭâ 

//// ���濡�Ը� ���̴� �� 
//public class Robby_InteractionVoiceUI : MonoBehaviourPun
//{
//    //���� ������ 
//    [Header("���� Ȯ��")]
//    [SerializeField] float talkDistance = 2.5f;
//    [SerializeField] GameObject handShakeImage;

//    [Header("��ư")]
//    [SerializeField] Button oneOnoneTalk; // 1:1 ��ȭâ
//    [SerializeField] Button accept; // ��ȭ �Ұ���
//    [SerializeField] Button refuse; // ��ȭ ���Ұ��� 

//    [Header("��ȭUI")]
//    [SerializeField] GameObject DoyouwannaTalk; // 1:1 ��ȭ ����
//    [SerializeField] GameObject SuccessPopUI; // ��ȭ �Ҳ���?
//    [SerializeField] GameObject MyVociePanel; // ���� �߱� 

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

//        // 1:1 ��ȭâ ��������
//        // ��ȭ �ҰŴ�? yes / no ���� 
//        // ���� �� �ִ��� ������ bool 
//        if (oneOnoneTalk.interactable == true) // ��ư �������� 
//        {
//            SuccessPopUI.SetActive(true); // ��ȭ �����ҰŸ���? 
//            // ���ι�ư �������� 
//            if (accept.interactable == true)
//            {
//                // ���� �г� Ȱ��ȭ 
//                accept.onClick.AddListener(makeVoiceRoom);
//                MyVociePanel.SetActive(true);

//                // ������ �˾�â ���� 
//                SuccessPopUI.SetActive(false);
//                DoyouwannaTalk.SetActive(false);
//            }
//            else if (refuse.interactable == true)
//            {
//                // �˾�â ���� 
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
//        // Ŭ���ϸ� �Ǽ��� �߱�  
//        public void OnPointerClick(PointerEventData eventData)
//        {
//            if (Vector3.Distance(collider.transform.position, transform.position) < talkDistance)
//            {
//                if (collider.tag == "Player")
//                {
//                    handShakeImage.SetActive(true);
//                    Destroy(handShakeImage, 2f); // 2�� �� ����� 
//                    DoyouwannaTalk.SetActive(true); // 1:1 ��ȭâ ������ �� �ִ� UI �� 
//                }
//            }
//        }

//        // Ŭ�� ����� �Ǽ��� ���� 
//        public void OnPointerExit(PointerEventData eventData)
//        {
//            if (Vector3.Distance(collider.transform.position, transform.position) > talkDistance)
//            {
//                if (collider.tag == "Player")
//                    handShakeImage.SetActive(false);
//            }
//        }
//    }
