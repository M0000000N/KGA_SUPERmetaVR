using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncorrectPWPopupCanvas : MonoBehaviour
{
    [SerializeField] private Button okButton;
    private bool isStartCoroutine;

    private void Awake()
    {
        isStartCoroutine = false;
        okButton.onClick.AddListener(OnPressOkButton);
    }

    public void OnPressOkButton()
    {
        if (isStartCoroutine == false)
        {
            isStartCoroutine = true;
            StopCoroutine(SetPopupUICanvasCoroutine());
            StartCoroutine(SetPopupUICanvasCoroutine());
        }
    }

    IEnumerator SetPopupUICanvasCoroutine()
    {
        CanvasGroup canvasGroup = transform.GetComponent<CanvasGroup>();

        canvasGroup.alpha = 1;
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 0.1f;
            yield return new WaitForSecondsRealtime(0.03f); // (1 : 0.3 = 0.1 : x) => (0.03 = x)
            // 0.3초 페이드아웃
        }
        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
        isStartCoroutine = false;
    }
}
