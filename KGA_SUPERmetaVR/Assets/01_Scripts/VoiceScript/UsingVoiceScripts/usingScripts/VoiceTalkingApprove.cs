using Photon.Voice.PUN;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class VoiceTalkingApprove : OnlyOneSceneSingleton<VoiceTalkingApprove>
{
    [SerializeField] GameObject VoiceApproveUI; 
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Button btnOkay;

    public void Set(string txt, UnityAction onOkay = null, UnityAction onReject = null)
    {     
        this.text.text = txt;
        btnOkay.onClick.RemoveAllListeners(); 
        if (onOkay != null) btnOkay.onClick.AddListener(onOkay);
    }

    public void OpenPopup()
    {
        VoiceApproveUI.gameObject.SetActive(true);
    }

    public void ClosePopup()
    {
        VoiceApproveUI.gameObject.SetActive(false);
    }
}
