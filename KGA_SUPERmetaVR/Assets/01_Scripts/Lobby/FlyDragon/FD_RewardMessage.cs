using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FD_RewardMessage : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private bool isOpen;

    private void Start()
    {
        canvasGroup = transform.GetComponent<CanvasGroup>();
    }

    public void OpenUI()
    {
        StartCoroutine(OpenUICoroutine());
    }

    IEnumerator OpenUICoroutine()
    {
        gameObject.SetActive(true);
        isOpen = true;

        canvasGroup.alpha = 0;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += 0.1f / 3;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(15);
        OnPress();
    }

    IEnumerator CloseUICoroutine()
    {
        isOpen = false;

        canvasGroup.alpha = 1;
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 0.1f / 2;
            yield return new WaitForSeconds(0.1f);
        }
        RewardManager.Instance.GetItem();
        gameObject.SetActive(false);
    }

    public void OnPress()
    {
        if (isOpen)
        {
            StartCoroutine(CloseUICoroutine());
        }
    }
}
