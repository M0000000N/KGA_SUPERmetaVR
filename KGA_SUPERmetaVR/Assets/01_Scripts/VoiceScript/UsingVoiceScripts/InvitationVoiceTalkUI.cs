using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvitationVoiceTalkUI : MonoBehaviourPunCallbacks
{
    public GameObject GetHandShakeImage => InteractionTalking.gameObject;
    public GameObject GetDialog {  get { return SpeechBubble.gameObject;  } }
    public GameObject GetVoiceTalkingCanvas {  get { return VoiceTalkingCanvas.gameObject; } }

    // ���� ����, ���� ����
    public Player GetotherPlayerPhotonview { get { return otherPlayer; } }

    [Header("��ȣ�ۿ� ���� ����")]
    [SerializeField] GameObject SpeechBubble;
    [SerializeField] Button InteractionTalking;
    [SerializeField] Button TalkingTogether;

    [Header("���̽�ê ��ȣ�ۿ� ����")]
    [SerializeField] private GameObject VoiceTalkingCanvas;
    [SerializeField] private Button ConfirmButton;
    [SerializeField] private Button RejectButton;
    [SerializeField] Button OkayButton;
    [SerializeField] private GameObject VoiceTalkingCheck; 

    [SerializeField] private TextMeshProUGUI TalkingNickName; 
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private TextMeshProUGUI Check_ContentText;

    string OtherNickname;

    Player player; 
    Player otherPlayer;

    private PhotonView photonView;
    private PhotonVoiceNetwork voiceNetwork; 

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        voiceNetwork = GetComponent<PhotonVoiceNetwork>(); 
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            player = photonView.Owner;         
           // OwnerNickname = photonView.Owner.NickName;
           // myActorNum = photonView.Owner.ActorNumber;
        }
        else
        {
            otherPlayer = photonView.Owner;
            OtherNickname = photonView.Owner.NickName;
          //  otherActorNum = photonView.Owner.ActorNumber;
            if (OtherNickname.Equals(null))
                return;
        }
        //Part1.
        InteractionTalking.gameObject.SetActive(false);
        SpeechBubble.SetActive(false);

        //Part2. - ��û�� 
        VoiceTalkingCanvas.SetActive(false);
        OkayButton.gameObject.SetActive(false);
        VoiceTalkingCheck.SetActive(false);

        InteractionTalking.onClick.AddListener(DialogPopUI);
        TalkingTogether.onClick.AddListener(VoiceCanvasPopUI);
        ConfirmButton.onClick.AddListener(AskTalkingConfirm);
        RejectButton.onClick.AddListener(RejectCanvasPopUI);
        OkayButton.onClick.AddListener(TalkingRequest);

    }

    private void Update() 
    {
        InteractionTalking.transform.forward = Camera.main.transform.forward;    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (photonView.IsMine == false) return;
            InvitationVoiceTalkUI talkUI = other.gameObject.GetComponent<InvitationVoiceTalkUI>();

            //player ���� 
            otherPlayer = other.gameObject.GetPhotonView().Owner; // �ε�ģ ���� ����� ����

            if (talkUI != null)
            {               
                talkUI.GetHandShakeImage.SetActive(true);
            }
            else
            {
                return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
        if (photonView.IsMine == false) return;
            InvitationVoiceTalkUI talkUI = other.gameObject.GetComponent<InvitationVoiceTalkUI>();

            Debug.Assert(talkUI != null);

            talkUI.GetDialog.SetActive(false);
            talkUI.GetHandShakeImage.SetActive(false);
        }
    }

    public void DialogPopUI()
    {
        SpeechBubble.SetActive(true);
        TalkingNickName.text = OtherNickname;
    }

    public void VoiceCanvasPopUI()
    {
        VoiceTalkingCanvas.SetActive(true);
        contentText.text = OtherNickname + "�Կ��� ��ȭ�� ��û�Ͻðڽ��ϱ�";
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
            contentText.text = OtherNickname + "�Կ��� 1:1 ��ȭ�� ��û�Ͽ����ϴ�";
            //��û 
    }

    public void TalkingRequest()
    {
        photonView.RPC("ConfirmTalkingCheck", RpcTarget.Others, true);
    }

    [PunRPC]
    public void confrimTalkingCheck(bool _value)
    {
        VoiceTalkingCheck.SetActive(_value);
        Check_ContentText.text = otherPlayer.NickName + "ä�η� �����Ͻðڽ��ϱ�??";
    }

}



