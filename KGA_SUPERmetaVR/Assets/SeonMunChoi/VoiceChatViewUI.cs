using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// View
public class VoiceChatViewUI : MonoBehaviour
{
    public Text TitleText { get; private set; }
    public Text CaptionText { get; private set; }
    public Button CheckButton { get; private set; }

    // 각 UI 요소를 찾는다.
    private void Awake()
    {
        TitleText = transform.Find("TitleText").GetComponent<Text>();
        Debug.Assert(TitleText != null);

        CaptionText = transform.Find("CaptionText").GetComponent<Text>();
        Debug.Assert(CaptionText != null);

        CheckButton = transform.Find("CheckButton").GetComponent<Button>();
        Debug.Assert(CheckButton != null); 
    }
}
