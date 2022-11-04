using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using ExitGames.Client.Photon;

//// 1:1 보이스룸 

//// 상대방에게만 보이는 것 
public class Robby_InteractionVoiceUI : MonoBehaviourPun
{
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

    PhotonVoiceNetwork photonVoiceNetwork = new PhotonVoiceNetwork();
   
    // 1:1 내가 원하는 보이스그룹
    public byte audioGroup;
    public bool subscribed; 

    private void Start()
    {
        handshake.transform.gameObject.SetActive(false);
        doyouwannatalk.SetActive(false);
        successpopui.SetActive(false);
        handshake.onClick.AddListener(Popdodyouwannatalk);
        oneOnOneTalk.onClick.AddListener(SucceseePop);

    }
    

    // 거리 내에 들어오면 악수 버튼 띄우기 
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.CompareTag("Player"))
        {
            if (Vector3.Distance(gameObject.transform.position, transform.position) > 2.5)
            {
                handshake.transform.gameObject.SetActive(true);
            }
            else
            {
                handshake.transform.gameObject.SetActive(false);
            }
        }
    }

    private void Popdodyouwannatalk()
    {
        doyouwannatalk.SetActive(true);
    }

    private void SucceseePop()
    {
        doyouwannatalk.SetActive(true);
        successpopui.SetActive(true);
    }


    private void OnVoiceClientStateChanged(StatusCode state)
    {
        if(accept != null)
        {
            switch(state)
            {
                case StatusCode.Connect:
                   // accept.gameObject.SetActive(true);
                    subscribed = subscribed || Subscribe();
                    break;
                default:
                    accept.gameObject.SetActive(true);
                    break;


            }
        }
    }

    public bool Subscribe()
    {
        return !subscribed && photonVoiceNetwork.Client.ChangeAudioGroups(null, new byte[1] { audioGroup }); 
    }
}

