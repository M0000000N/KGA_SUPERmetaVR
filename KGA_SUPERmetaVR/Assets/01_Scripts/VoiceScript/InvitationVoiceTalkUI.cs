using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvitationVoiceTalkUI : MonoBehaviourPunCallbacks, IPunObservable
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
   // [SerializeField] private Transform viewTransform;
    [SerializeField] private Button ConfirmButton;
    [SerializeField] private Button RejectButton;
    [SerializeField] private Button OkayButton;
    [SerializeField] private GameObject VoiceChatPanel; 

    //���̽� �гο��� üũ
   // [SerializeField] private TextMeshProUGUI titleText;
    //�Ϲ� �� ���� 
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private TextMeshProUGUI Nickname;
    [SerializeField] private TextMeshProUGUI TalkingTogetherNickname;
    string OwnerNickname;
    string OtherNickname; 

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
        if (photonView.IsMine) return; 
        Nickname.text = playerData.Nickname;

        InvitationVoiceTalkUI talkUI = GetComponent<InvitationVoiceTalkUI>();

        //Part1.
        InteractionTalking.gameObject.SetActive(false);
        SpeechBubble.SetActive(false);


        //Part2. - ��û�� 
        VoiceTalkingCanvas.SetActive(false);
        OkayButton.gameObject.SetActive(false);
        VoiceChatPanel.SetActive(false);

        InteractionTalking.onClick.AddListener(DialogPopUI);
        TalkingTogether.onClick.AddListener(VoiceCanvasPopUI);
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

            //player ���� 
            //otherPlayer = other.gameObject.GetPhotonView().Owner; // �ε�ģ ���� ����� ����
            //OtherNickname = other.gameObject.GetComponent<PlayerData>().Nickname;
           
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
        contentText.text = playerData.Nickname + "Request?";
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

        contentText.text = playerData.Nickname + "Request Finish!";
    }

    // OK��ư ������ ���ÿ� ����/������ �� 
    // �ʴ��� Ȯ�� 
    public void TalkingCheckUI()
    {
        photonView.RPC("ConfirmTalkingCheck", RpcTarget.Others, true);
       // contentText.text = "��û ��";
    }

    [PunRPC]
    public void ConfirmTalkingCheck(bool _value)
    {
        VoiceTalkingCanvas.SetActive(_value);
        OkayButton.gameObject.SetActive(!_value);
        contentText.text = playerData.Nickname + "1:1 Approve Or Reject?";
    }

    // ���� or ���� 
    [PunRPC]
    public void AprroveReject(bool isYes)
    {
        if (photonView.IsMine)
        {
            ConfirmButton.gameObject.SetActive(!isYes);
            RejectButton.gameObject.SetActive(!isYes);
            OkayButton.gameObject.SetActive(isYes);

        }

        // MVC

        // Model : ������ ����
        // View : �������� ��
        // Controller : 

    }


    

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

        }
    }

}



