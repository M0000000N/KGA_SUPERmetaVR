using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FindIDPopupCanvas : MonoBehaviour
{
    [SerializeField] private Button okButton;
    private TextMeshProUGUI infomation;
    private bool isStartCoroutine;

    private void Awake()
    {
        isStartCoroutine = false;
        okButton.onClick.AddListener(OnPressOkButton);
    }

    public void SetInfomation(string _id)
    {
        infomation.text = $"회원님의 아이디는 \"{_id}\" 입니다.\n다시 로그인 해주세요 !";
    }

    public void OnPressOkButton()
    {
        if(isStartCoroutine == false)
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
