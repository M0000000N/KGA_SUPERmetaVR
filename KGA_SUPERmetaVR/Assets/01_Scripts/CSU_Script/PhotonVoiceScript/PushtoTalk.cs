using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Photon.Voice.PUN;


public class PushtoTalk : MonoBehaviourPun
{
    // 1:1 대화 참가 
    [Header("버튼")]
    [SerializeField] Button pushToTalkPrivateButton; // 1:1 대화창
    [SerializeField] Button accept; // 대화 할거임
    [SerializeField] Button refuse; // 대화 안할거임 

    [Header("대화UI")]
    [SerializeField] GameObject DoyouwannaTalk; // 1:1 대화 고르기
    [SerializeField] GameObject SuccessPopUI; // 대화 할꺼임?
    [SerializeField] GameObject MyVociePanel; // 확인 누른 다음 나한테 voice UI 띄우기
                                              
    private PunVoiceClient punVoiceClient;
   

    // 1:1 내가 원하는 보이스그룹
    public byte AudioGroup;
    public bool Subscribed;

    private void Awake()
    {
        this.punVoiceClient = GetComponent<PunVoiceClient>();
        
    }

}
