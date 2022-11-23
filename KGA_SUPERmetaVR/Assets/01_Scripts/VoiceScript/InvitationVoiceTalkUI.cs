using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Oculus.Interaction.PoseDetection.Debug;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq.Expressions;

public class InvitationVoiceTalkUI : SingletonBehaviour<InvitationVoiceTalkUI>
{
    public GameObject GetHandShakeImage => InteractionTalking.gameObject;
    public GameObject GetDialog {  get { return DialogUI.gameObject;  } }

    [Header("상호작용 범위 감지")]
    [SerializeField] GameObject DialogUI;
    [SerializeField] Button InteractionTalking;
    [SerializeField] Button TalkingTogether;

    [Header("보이스챗 상호작용 시작")]
    [SerializeField] private GameObject VoiceTalkingCanvas;
   // [SerializeField] private Transform viewTransform;
    [SerializeField] private Button ConfirmButton;
    [SerializeField] private Button RejectButton;
    [SerializeField] private Button OkayButton; 

    //보이스 패널에서 체크
   // [SerializeField] private TextMeshProUGUI titleText;
    //일반 글 띄우기 
    [SerializeField] private TextMeshProUGUI contentText;

    Player otherPlayer;
    private PlayerData playerData;

    private int targetUID;
    private string targetNickName;
    private int requestTargetUID;
    private string requestTargetNickName;
    private PhotonView photonView;
    private bool recentBool; 

    private void Awake()
    {
        playerData = GameManager.Instance.PlayerData;
        photonView = PhotonView.Get(this); 
    }

    private void Start()
    {
        //Part1.
        InteractionTalking.gameObject.SetActive(false);
        DialogUI.SetActive(false);
        InteractionTalking.onClick.AddListener(DialogPopUI);
        TalkingTogether.onClick.AddListener(VoiceCanvasPopUI);

        //Part2. - 요청자 
        VoiceTalkingCanvas.SetActive(false);
        OkayButton.gameObject.SetActive(false);
        ConfirmButton.onClick.AddListener(AskTalkingConfirm);
        RejectButton.onClick.AddListener(RejectCanvasPopUI);
        OkayButton.onClick.AddListener(TalkingCheckUI);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine == false) return;

        if (other.gameObject.tag == "Player")
        {
            InvitationVoiceTalkUI talkUI = other.gameObject.GetComponent<InvitationVoiceTalkUI>();

            //player 정보 
            otherPlayer = other.gameObject.GetPhotonView().Owner; // 부딪친 상대방 포톤뷰 정보

            if (talkUI != null)
            {               
                talkUI.GetHandShakeImage.SetActive(true);
                talkUI.transform.LookAt(this.transform.position);
            }
            else
            {
                return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (photonView.IsMine == false) return;

        if (other.gameObject.tag == "Player")
        {
            InvitationVoiceTalkUI talkUI = other.gameObject.GetComponent<InvitationVoiceTalkUI>();

            if (talkUI != null)
            {
                talkUI.GetDialog.SetActive(false);
                talkUI.GetHandShakeImage.SetActive(false);
            }
            else
            {
                Debug.Log("야 Null값 받아라~");
            }
        }
    }

    public void DialogPopUI()
    {
        DialogUI.SetActive(true);
    }

    public void VoiceCanvasPopUI()
    {
        VoiceTalkingCanvas.SetActive(true);
        contentText.text = otherPlayer.NickName + "님에게 대화를 요청하시겠습니까?";
    }

    public void RejectCanvasPopUI()
    {
        VoiceTalkingCanvas.SetActive(false);
    }

    public void AskTalkingConfirm()
    {
        ConfirmButton.gameObject.SetActive(false);
        RejectButton.gameObject.SetActive(false);
        OkayButton.gameObject.SetActive(true);

        contentText.text = otherPlayer.NickName + "님에게 대화를 요청하였습니다";
    }


    // OK버튼 누름과 동시에 수락/거절이 감 
    // 초대장 확인 
    public void TalkingCheckUI()
    {
        photonView.RPC("ConfirmTalkingCheck", otherPlayer, true);
        contentText.text = "요청 중";
        // 확인버튼 누르면 캔버스 꺼져야 하는데... 음...
    }

    [PunRPC]
    public void ConfirmTalkingCheck(bool _value)
    {
        VoiceTalkingCanvas.SetActive(_value);
        OkayButton.gameObject.SetActive(!_value);
        contentText.text = otherPlayer.NickName + "님이 1:1 대화를 요청하였습니다. 수락하시겠습니까?";
    }

    // 수락 or 거절 
    [PunRPC]
    public void AprroveReject(bool isYes)
    {
        ConfirmButton.gameObject.SetActive(!isYes);
        RejectButton.gameObject.SetActive(!isYes);
        OkayButton.gameObject.SetActive(isYes);
        recentBool = isYes; 
    }


}



