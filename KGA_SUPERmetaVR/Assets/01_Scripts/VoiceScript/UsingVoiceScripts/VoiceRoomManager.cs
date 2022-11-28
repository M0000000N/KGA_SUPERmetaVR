using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class VoiceRoomManager : OnlyOneSceneSingleton<VoiceRoomManager>
{
    [SerializeField] Text txt;
    [SerializeField] Button btnYes;
    [SerializeField] Button btnNo;


    private void Awake()
    {
        VoiceRoomManager.Instance.gameObject.SetActive(true);
        VoiceRoomManager.Instance.Set("____�� 1:1 ��ȭ�� �Ͻðڽ��ϱ�?", OnClickYes, OnClickNo);

    }

    void OnClickYes()
    {
        Debug.Log("Yes");
        VoiceRoomManager.Instance.gameObject.SetActive(false);
    }

    void OnClickNo()
    {
        Debug.Log("No");
        VoiceRoomManager.Instance.gameObject.SetActive(false);
    }

    public void Set(string txt, UnityAction onYes = null, UnityAction onNo = null)
    {
        this.txt.text = txt;
        if(onYes != null) btnYes.onClick.AddListener(onYes);
        if(onNo != null) btnNo.onClick.AddListener(onNo);
    }
    // �� �տ� ���� ��
    // �г��� , ��ư Yes, No, okay 


    // ������ ��ܿ� ���� �� 



}
