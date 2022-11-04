using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using System.Collections.Generic;
using UnityEngine.XR;
using System.Linq;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit;

//// 1:1 보이스룸 

//// 상대방에게만 보이는 것 
public class Robby_InteractionVoiceUI : MonoBehaviourPun
{
    [SerializeField]
    public XRNode XrNode = XRNode.LeftHand;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;
    private bool triggerIsPressed;

    [SerializeField] XRRayInteractor leftRayInteractor;
    [SerializeField] XRRayInteractor RightRayInteractor;
    private RaycastHit leftRayHit;
    private RaycastHit RightRayHit;
    [SerializeField]
    private int maxDistance = 3;
    GameObject targetObject;

    [Header("범위 확인")]
    [SerializeField] float talkDistance = 2.5f;
    [SerializeField] Button handshake;

    [Header("버튼")]
    [SerializeField] Button oneOnOneTalk;
    [SerializeField] Button accept;
    [SerializeField] Button refuse;

    [Header("대화UI")]
    [SerializeField] GameObject doyouwannatalk; 
    [SerializeField] GameObject successpopui;
    // [SerializeField] GameObject myvociepanel; - 나한테 떠야하는 것 

    PhotonVoiceNetwork photonVoiceNetwork;

    // 1:1 내가 원하는 보이스그룹
    public byte audioGroup;
    public bool subscribed;
    private bool[] voiceroomNumber = new bool[256];

    private void Start()
    {
        handshake.transform.gameObject.SetActive(false);
        //doyouwannatalk.SetActive(false);
        successpopui.SetActive(false);

        handshake.onClick.AddListener(Popdodyouwannatalk);
        oneOnOneTalk.onClick.AddListener(SucceseePop);
        accept.onClick.AddListener(AcceptButton);
        refuse.onClick.AddListener(RefuseButton);

        photonVoiceNetwork = GetComponent<PhotonVoiceNetwork>();
    }
    

    private void Popdodyouwannatalk()
    {
        doyouwannatalk.SetActive(true);
    }

    private void SucceseePop()
    {
        doyouwannatalk.SetActive(true);
        successpopui.SetActive(true);

        if (RightRayInteractor.TryGetCurrent3DRaycastHit(out RightRayHit, out maxDistance))
        {
            targetObject = RightRayHit.transform.gameObject;
          //targetObject =  instance.client.Connect(voiceMasterAddress, null, null, voiceNickname, new LoadBalancing.AuthenticationValues(voiceUserId));
        // photonVoiceNetwork.VoiceClient.voiced
        }
    } 


   
    // 바이트 안 숫자 
   // [PunRPC]
    public void AcceptButton()
    {
        // doyouwannatalk.SetActive(true);
        for (int i = 1; i < 256; ++i)
        {
            if (voiceroomNumber[i] == false) // 방이 없을 때 
            {
                // byte[] remove = new byte[] { 0 }; // 기본그룹(전체음성) 삭제           
                byte[] add = new byte[] { (byte)i };
                photonVoiceNetwork.Client.OpChangeGroups(null, add);
                VoiceroomManager.Instance.recorder.InterestGroup = (byte)i;
                Debug.Log(i);
            }
            else { continue; }
        }
        //  photonVoiceNetwork.AutoConnectAndJoin = true;
        //  photonVoiceNetwork.AutoLeaveAndDisconnect = true; 
    }


    public void FinishButton()
    {
        // connectandjoinroom 
        photonVoiceNetwork.AutoLeaveAndDisconnect = true; 
        // 레코더 초기화
       // photonVoiceNetwork.InitRecorder(VoiceroomManager.Instance.recorder);
    }


    public void RefuseButton()
    {
        // 1:1 생성된 방을 없애고
        // 모두가 속해있는 그룹으로 돌아간다
        for (int i = 1; i < 256; ++i)
        {
            if(voiceroomNumber[i]) // 방이 있을 때
            {
                byte[] remove = new byte[] { (byte)i };
                byte[] add = new byte[] { 0 };
                photonVoiceNetwork.Client.OpChangeGroups(remove, add);
                VoiceroomManager.Instance.recorder.InterestGroup = 0;
                Debug.Log("모두 들리나");
                voiceroomNumber[i] = false;          
            }
        }      
    }
}


    //// 거리 내에 들어오면 악수 버튼 띄우기 
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.collider.gameObject.CompareTag("Player"))
    //    {
    //        if (Vector3.Distance(gameObject.transform.position, transform.position) > 2.5)
    //        {
    //            handshake.transform.gameObject.SetActive(true);
    //        }
    //        else
    //        {
    //            handshake.transform.gameObject.SetActive(false);
    //        }
    //    }
    //}

    //public void AcceptButton()
    //{
    //    byte[] remove = new byte[] { 0, 1 };
    //    byte[] add = new byte[] { 2 };
    //    photonVoiceNetwork.Client.OpChangeGroups(remove, add);
    //    PeekabooSoundManager.Instance.recorder.InterestGroup = 2;
        
    //}