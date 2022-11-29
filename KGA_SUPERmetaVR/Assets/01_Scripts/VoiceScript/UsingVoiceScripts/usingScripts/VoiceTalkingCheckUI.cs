using Photon.Voice.PUN;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using WebSocketSharp;

public class VoiceTalkingCheckUI : OnlyOneSceneSingleton<VoiceInvitationUI>
{
    public GameObject GetVoiceTalkingUI { get { return VoiceTalkingUI.gameObject; } }

    [SerializeField] GameObject VoiceTalkingUI; 
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Button btnYes;
    [SerializeField] Button btnNo;

    public void Set(string txt, UnityAction onYes = null, UnityAction onNo = null)
    {
        this.text.text = txt;
        btnYes.onClick.RemoveAllListeners();
        btnNo.onClick.RemoveAllListeners();

        if (onYes != null) btnYes.onClick.AddListener(onYes);
        if (onNo != null) btnNo.onClick.AddListener(onNo);
    }

}
