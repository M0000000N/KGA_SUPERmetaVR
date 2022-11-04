using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using ExitGames.Client.Photon;

//// 1:1 ���̽��� 

//// ���濡�Ը� ���̴� �� 
public class Robby_InteractionVoiceUI : MonoBehaviourPun
{
    [Header("���� Ȯ��")]
    [SerializeField] float talkDistance = 2.5f;
    [SerializeField] Button handshake;

    [Header("��ư")]
    [SerializeField] Button oneOnOneTalk;
    [SerializeField] Button accept;
    [SerializeField] Button refuse;

    [Header("��ȭUI")]
    [SerializeField] GameObject doyouwannatalk; 
    [SerializeField] GameObject successpopui;
    // [SerializeField] GameObject myvociepanel; - ������ �����ϴ� �� 

    PhotonVoiceNetwork photonVoiceNetwork = new PhotonVoiceNetwork();
   
    // 1:1 ���� ���ϴ� ���̽��׷�
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
    

    // �Ÿ� ���� ������ �Ǽ� ��ư ���� 
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

