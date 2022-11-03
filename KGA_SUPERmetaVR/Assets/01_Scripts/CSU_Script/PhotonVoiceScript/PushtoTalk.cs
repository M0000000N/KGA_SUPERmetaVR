using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Photon.Voice.PUN;


public class PushtoTalk : MonoBehaviourPun
{
    // 1:1 ��ȭ ���� 
    [Header("��ư")]
    [SerializeField] Button pushToTalkPrivateButton; // 1:1 ��ȭâ
    [SerializeField] Button accept; // ��ȭ �Ұ���
    [SerializeField] Button refuse; // ��ȭ ���Ұ��� 

    [Header("��ȭUI")]
    [SerializeField] GameObject DoyouwannaTalk; // 1:1 ��ȭ ����
    [SerializeField] GameObject SuccessPopUI; // ��ȭ �Ҳ���?
    [SerializeField] GameObject MyVociePanel; // Ȯ�� ���� ���� ������ voice UI ����
                                              
    private PunVoiceClient punVoiceClient;
   

    // 1:1 ���� ���ϴ� ���̽��׷�
    public byte AudioGroup;
    public bool Subscribed;

    private void Awake()
    {
        this.punVoiceClient = GetComponent<PunVoiceClient>();
        
    }

}
