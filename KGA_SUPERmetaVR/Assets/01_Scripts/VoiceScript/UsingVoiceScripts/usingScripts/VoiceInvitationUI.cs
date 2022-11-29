using ExitGames.Demos.DemoPunVoice;
using Photon.Voice.PUN;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class VoiceInvitationUI : OnlyOneSceneSingleton<VoiceInvitationUI>
{
  
    [SerializeField] GameObject VoiceInvitatinoUI;
    [SerializeField] GameObject VoiceTalkingUI;
    [SerializeField] TextMeshProUGUI text; 
    [SerializeField] Button btnYes;
    [SerializeField] Button btnNo;

    public void Set(string txt, UnityAction onYes = null, UnityAction onNo = null)
    {
        btnYes.onClick.RemoveAllListeners();
        btnNo.onClick.RemoveAllListeners(); 

        this.text.text = txt;
        if (onYes != null) btnYes.onClick.AddListener(onYes);
        if (onNo != null) btnNo.onClick.AddListener(onNo);
    }
    public void OpenPopup()
    {
        VoiceInvitatinoUI.gameObject.SetActive(true);
    }

    public void ClosePopup()
    {
        VoiceInvitatinoUI.gameObject.SetActive(false);
    }

    public void TalkingOpenPopUp()
    {
        VoiceTalkingUI.gameObject.SetActive(true);
    }

    public void TalkingClosePopUp()
    {
        VoiceTalkingUI.gameObject.SetActive(false);
    }
}
