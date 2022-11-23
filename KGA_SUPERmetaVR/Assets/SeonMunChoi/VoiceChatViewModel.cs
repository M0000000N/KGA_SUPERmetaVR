using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Model
public static class VoiceChatViewModel
{
    private static string _titleText = string.Empty;
    private static string _captionText = string.Empty;
    
    public static event Action<string> OnChangeTitleText;
    public static event Action<string> OnChangeCaptionText;

    public static string TitleText
    {
        get
        {
            return _titleText;
        }
        set
        {
            _titleText = value;
            OnChangeTitleText(_titleText);
        }
    }

    public static string CaptionText
    {
        get
        {
            return _captionText;
        }
        set
        {
            _captionText = value;
            OnChangeCaptionText(_captionText);
        }
    }
}
