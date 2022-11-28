using Photon.Voice.PUN;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class VoiceInvitationUI : OnlyOneSceneSingleton<VoiceInvitationUI>
{
    [SerializeField] TextMeshProUGUI text; 
    [SerializeField] Button btnYes;
    [SerializeField] Button btnNo;
    [SerializeField] Button btnOkay; 

    public void Set(string txt, UnityAction onYes = null, UnityAction onNo = null, UnityAction onOkay = null)
    {
        this.text.text = txt;
        if (onYes != null) btnYes.onClick.AddListener(onYes);
        if (onNo != null) btnNo.onClick.AddListener(onNo);
        if (onOkay != null) btnOkay.onClick.AddListener(onOkay);
    }
}
