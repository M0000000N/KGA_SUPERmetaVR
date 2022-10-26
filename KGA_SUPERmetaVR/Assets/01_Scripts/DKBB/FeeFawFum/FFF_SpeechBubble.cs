using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FFF_SpeechBubble : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI sizeText;
    [SerializeField] TextMeshProUGUI text;
    public string SpeechText { get { return text.text; } private set { text.text = value; sizeText.text = text.text; } }

    private void Start()
    {
        StartCoroutine("TextCoroutine");
    }

    private void Update()
    {
        this.transform.LookAt(Camera.main.transform);
    }

    IEnumerator TextCoroutine()
    {
        int count = StaticData.SpeechBubbleSheetData.Length;

        while(true)
        {

            for (int i = 0; i < count; i++)
            {
                SetSpeechText(StaticData.GetSpeechBubbleSheet(i).Speechtext);
                yield return new WaitForSeconds(3f);
            }
        }
    }

    public void SetSpeechText(string _speechText)
    {
        SpeechText = _speechText;
    }
}
