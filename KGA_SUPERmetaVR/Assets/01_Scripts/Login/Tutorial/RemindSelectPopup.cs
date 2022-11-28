using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemindSelectPopup : MonoBehaviour
{
    [SerializeField] GameObject Select1Image;
    [SerializeField] GameObject Select2Image;

    [SerializeField] private Button YesButton;
    [SerializeField] private Button noButton;
    
    private bool isStartCoroutine;
    private int selectType;

    private void Awake()
    {
        selectType = 0;
        isStartCoroutine = false;
        YesButton.onClick.AddListener(OnPressOkButton);
        noButton.onClick.AddListener(OnPressNoButton);
    }

    public void SelectCharacter(int _select)
    {
        switch (_select)
        {
            case 1:
                {
                    // 추워요
                    selectType = 15000; 
                    Select1Image.SetActive(true);
                    Select2Image.SetActive(false);
                }
                break;

            case 2:
                {
                    // 구미호가 되고 싶은 캐스퍼
                    selectType = 15001;
                    Select1Image.SetActive(false);
                    Select2Image.SetActive(true);
                }
                break;
        }
        gameObject.SetActive(true);
    }

    public void OnPressOkButton()
    {
        if (isStartCoroutine == false)
        {
            SoundManager.Instance.PlaySE("popup_click.wav");
            isStartCoroutine = true;
            StopCoroutine(SetPopupUICanvasCoroutine(1));
            StartCoroutine(SetPopupUICanvasCoroutine(1));
        }
    }

    public void OnPressNoButton()
    {
        if (isStartCoroutine == false)
        {
            SoundManager.Instance.PlaySE("popup_click.wav");
            isStartCoroutine = true;
            StopCoroutine(SetPopupUICanvasCoroutine(2));
            StartCoroutine(SetPopupUICanvasCoroutine(2));
        }
    }

    IEnumerator SetPopupUICanvasCoroutine(int _Buttontype)
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

        switch (_Buttontype)
        {
            case 1:
                {
                    // OK
                    GameManager.Instance.PlayerData.DefaultCustomize = selectType;
                    GameManager.Instance.PlayerData.Customize = selectType;
                    DataBase.Instance.sqlcmdall($"UPDATE {UserTableInfo.table_name} SET {UserTableInfo.default_customize} = {selectType}," +
                        $"{UserTableInfo.customize} = {selectType} WHERE {UserTableInfo.id} = '{LoginManager.Instance.UserID}'");
                }
                break;

            case 2:
                {
                    // NO
                    LoginManager.Instance.SetUICanvas(LoginManager.Instance.SelectCharacter);
                }
                break;
        }

        gameObject.SetActive(false);
        isStartCoroutine = false;
    }
}
