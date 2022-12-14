using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FD_RewardMessage : MonoBehaviour
{
    [SerializeField] private GameObject popupUI;
    [SerializeField] private Button button;

    private CanvasGroup canvasGroup;
    private bool isOpen;
    

    private void Start()
    {
        isOpen = popupUI.gameObject.activeSelf;
        canvasGroup = popupUI.transform.GetComponent<CanvasGroup>();
        button.onClick.AddListener(OnPress);
    }

    public void OpenUI()
    {
        StopCoroutine(OpenUICoroutine());
        StartCoroutine(OpenUICoroutine());
    }

    IEnumerator OpenUICoroutine()
    {
        popupUI.gameObject.SetActive(true);
        isOpen = true;

        canvasGroup.alpha = 0;
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += 0.1f / 3;
            yield return new WaitForSeconds(0.1f);
        }

        canvasGroup.alpha = 1;
        yield return new WaitForSeconds(15);
        OnPress();
    }

    IEnumerator CloseUICoroutine()
    {
        isOpen = false;

        canvasGroup.alpha = 1;
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= 0.1f / 2;
            yield return new WaitForSeconds(0.1f);
        }

        canvasGroup.alpha = 0;
        RewardManager.Instance.GetItem();
        popupUI.gameObject.SetActive(false);
    }

    public void OnPress()
    {
        if (isOpen)
        {
            StopCoroutine(CloseUICoroutine());
            StartCoroutine(CloseUICoroutine());
        }
    }
}
