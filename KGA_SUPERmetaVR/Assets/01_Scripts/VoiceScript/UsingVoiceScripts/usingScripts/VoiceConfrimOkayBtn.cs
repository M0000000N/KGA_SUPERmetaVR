using Photon.Voice.PUN;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class VoiceConfrimOkayBtn : OnlyOneSceneSingleton<VoiceConfrimOkayBtn>
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Button btnOkay;

    public void Set(string txt, UnityAction onOkay = null)
    {
        this.text.text = txt;
        if (onOkay != null) btnOkay.onClick.AddListener(onOkay);
    }
}
