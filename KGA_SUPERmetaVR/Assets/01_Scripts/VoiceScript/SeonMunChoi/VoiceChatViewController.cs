using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controller
public class VoiceChatViewController : MonoBehaviour
{
    private VoiceChatViewUI _voiceChatViewUI;

    private void Awake()
    {
        _voiceChatViewUI = GetComponentInChildren<VoiceChatViewUI>();
        Debug.Assert(_voiceChatViewUI != null);
    }

    private void Start()
    {
        // View에서 발생된 이벤트를 토대로 Model에 전달한다.
        _voiceChatViewUI.CheckButton.onClick.AddListener(ChangeContent);

        // Model에 업데이트 된 것을 View에 전달한다.
        VoiceChatViewModel.OnChangeTitleText += AdoptToTitleUI;

        VoiceChatViewModel.OnChangeCaptionText += AdoptToCaptionUI;   
    }

    private bool _temp = false;

    private void ChangeContent()
    {
        _temp = !_temp;

        if (_temp)
        {
            VoiceChatViewModel.TitleText = "1번";
            VoiceChatViewModel.CaptionText = "이것은 1번 캡션";
        }
        else
        {
            VoiceChatViewModel.TitleText = "2번";
            VoiceChatViewModel.CaptionText = "이것은 2번 캡션";
        }
    }

    private void AdoptToTitleUI(string title)
    {
        _voiceChatViewUI.TitleText.text = title;
    }

    private void AdoptToCaptionUI(string caption)
    {
        _voiceChatViewUI.CaptionText.text = caption;
    }
}
