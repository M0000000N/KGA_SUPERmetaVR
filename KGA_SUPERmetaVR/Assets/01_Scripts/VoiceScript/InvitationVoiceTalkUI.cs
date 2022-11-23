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

    [Header("��ȣ�ۿ� ���� ����")]
    [SerializeField] GameObject DialogUI;
    [SerializeField] Button InteractionTalking;
    [SerializeField] Button TalkingTogether;

    [Header("���̽�ê ��ȣ�ۿ� ����")]
    [SerializeField] private GameObject VoiceTalkingCanvas;
   // [SerializeField] private Transform viewTransform;
    [SerializeField] private Button ConfirmButton;
    [SerializeField] private Button RejectButton;
    [SerializeField] private Button OkayButton; 

    //���̽� �гο��� üũ
   // [SerializeField] private TextMeshProUGUI titleText;
    //�Ϲ� �� ���� 
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

        //Part2. - ��û�� 
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

            //player ���� 
            otherPlayer = other.gameObject.GetPhotonView().Owner; // �ε�ģ ���� ����� ����

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
                Debug.Log("�� Null�� �޾ƶ�~");
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
        contentText.text = otherPlayer.NickName + "�Կ��� ��ȭ�� ��û�Ͻðڽ��ϱ�?";
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

        contentText.text = otherPlayer.NickName + "�Կ��� ��ȭ�� ��û�Ͽ����ϴ�";
    }


    // OK��ư ������ ���ÿ� ����/������ �� 
    // �ʴ��� Ȯ�� 
    public void TalkingCheckUI()
    {
        photonView.RPC("ConfirmTalkingCheck", otherPlayer, true);
        contentText.text = "��û ��";
        // Ȯ�ι�ư ������ ĵ���� ������ �ϴµ�... ��...
    }

    [PunRPC]
    public void ConfirmTalkingCheck(bool _value)
    {
        VoiceTalkingCanvas.SetActive(_value);
        OkayButton.gameObject.SetActive(!_value);
        contentText.text = otherPlayer.NickName + "���� 1:1 ��ȭ�� ��û�Ͽ����ϴ�. �����Ͻðڽ��ϱ�?";
    }

    // ���� or ���� 
    [PunRPC]
    public void AprroveReject(bool isYes)
    {
        ConfirmButton.gameObject.SetActive(!isYes);
        RejectButton.gameObject.SetActive(!isYes);
        OkayButton.gameObject.SetActive(isYes);
        recentBool = isYes; 
    }


}



