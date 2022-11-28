using Photon.Voice.PUN;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class VoiceTalkingApprove : SingletonBehaviour<VoiceTalkingApprove>
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Button btnOkay;

    public void Set(string txt, UnityAction onOkay = null, UnityAction onReject = null)
    {
        this.text.text = txt;
        if (onOkay != null) btnOkay.onClick.AddListener(onOkay);
    }

}
