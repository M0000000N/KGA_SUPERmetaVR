using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// �����Ȳ : 
// �г��� : �ڽ��� �г����� �߳� ���� �г����� ���� ����
// Ÿ���� ������ �����Ͽ� �������� �� 

public class InvitationVoiceTalkUI : MonoBehaviourPunCallbacks
{
    public GameObject GetHandShakeImage => InteractionTalking.gameObject;
    public GameObject GetDialog {  get { return SpeechBubble.gameObject;  } }
    public GameObject GetVoiceTalkingCanvas {  get { return VoiceTalkingCanvas.gameObject; } }

    [Header("��ȣ�ۿ� ���� ����")]
    [SerializeField] GameObject SpeechBubble;
    [SerializeField] Button InteractionTalking;
    [SerializeField] Button TalkingTogether;

    [Header("���̽�ê ��ȣ�ۿ� ����")]
    [SerializeField] private GameObject VoiceTalkingCanvas;
    [SerializeField] private Button ConfirmButton;
    [SerializeField] private Button RejectButton;
    [SerializeField] private Button OkayButton;

    [SerializeField] private TextMeshProUGUI contentText;

    string OwnerNickname;
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
            OwnerNickname = photonView.Owner.NickName;
            player = photonView.Owner;         
        }
        else
        {
            OtherNickname = photonView.Owner.NickName;
            otherPlayer = photonView.Owner; 
            if (OtherNickname.Equals(null))
                return;
        }

        // InvitationVoiceTalkUI talkUI = GetComponent<InvitationVoiceTalkUI>();

        //Part1.
        InteractionTalking.gameObject.SetActive(false);
        SpeechBubble.SetActive(false);

        //Part2. - ��û�� 
        VoiceTalkingCanvas.SetActive(false);
        OkayButton.gameObject.SetActive(false);

        InteractionTalking.onClick.AddListener(DialogPopUI);
        TalkingTogether.onClick.AddListener(VoiceCanvasPopUI);
        ConfirmButton.onClick.AddListener(AskTalkingConfirm);
        RejectButton.onClick.AddListener(RejectCanvasPopUI);
        
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
    }

    public void VoiceCanvasPopUI()
    {
        VoiceTalkingCanvas.SetActive(true);
        contentText.text = OtherNickname + "Request?";
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
            contentText.text = OtherNickname + "Request Finish!";
            //��û 
    }

}



