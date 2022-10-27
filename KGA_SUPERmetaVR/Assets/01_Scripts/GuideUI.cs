using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GuideUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI sizeText;
    [SerializeField] TextMeshProUGUI text;
    public string GuideText { get { return text.text; } private set { text.text = value; sizeText.text = text.text; } }

    public void ActiveGuideUI(string _guideText, float _time)
    {
        SetText(_guideText);
        StartCoroutine("ActiveGuideCoroutine", _time);
    }

    IEnumerator ActiveGuideCoroutine(float _time)
    {
        if(_time <= 0)
        {
            sizeText.gameObject.SetActive(true);
        }
        else
        {
            sizeText.gameObject.SetActive(true);
            yield return new WaitForSeconds(_time);
            sizeText.gameObject.SetActive(false);
        }
    }

    public void SetText(string _guideText)
    {
        GuideText = _guideText;
    }
}
