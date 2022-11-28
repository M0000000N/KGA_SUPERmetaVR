using Photon.Voice.PUN;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;


public class VoiceTalkingCheckUI : SingletonBehaviour<VoiceInvtationUI>
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Button btnYes;
    [SerializeField] Button btnNo;

    public void Set(string txt, UnityAction onYes = null, UnityAction onNo = null)
    {
        this.text.text = txt;
        if (onYes != null) btnYes.onClick.AddListener(onYes);
        if (onNo != null) btnNo.onClick.AddListener(onNo);
    }
}
