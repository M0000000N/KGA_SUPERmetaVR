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
        // View���� �߻��� �̺�Ʈ�� ���� Model�� �����Ѵ�.
        _voiceChatViewUI.CheckButton.onClick.AddListener(ChangeContent);

        // Model�� ������Ʈ �� ���� View�� �����Ѵ�.
        VoiceChatViewModel.OnChangeTitleText += AdoptToTitleUI;

        VoiceChatViewModel.OnChangeCaptionText += AdoptToCaptionUI;   
    }

    private bool _temp = false;

    private void ChangeContent()
    {
        _temp = !_temp;

        if (_temp)
        {
            VoiceChatViewModel.TitleText = "1��";
            VoiceChatViewModel.CaptionText = "�̰��� 1�� ĸ��";
        }
        else
        {
            VoiceChatViewModel.TitleText = "2��";
            VoiceChatViewModel.CaptionText = "�̰��� 2�� ĸ��";
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
