using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FFF_Button : MonoBehaviour
{
    private Button mybutton;
    [SerializeField] Image timer;
    private bool isStartCouroutine = false;
    private void Awake()
    {
        mybutton = GetComponent<Button>();
        timer.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (isStartCouroutine == false)
        {
            StartCoroutine(SetTimer());
        }
    }
    private void OnDisable()
    {
        StopCoroutine(SetTimer());
        isStartCouroutine = false;
        mybutton.interactable = true;
    }

    public void OnClickButton()
    {
        mybutton.interactable = false;
        SoundManager.Instance.PlaySE("popup_click.wav");
        StopCoroutine(SetTimer());
        isStartCouroutine = false;
        FFF_GameManager.Instance.PlusClearCount(true);
        timer.gameObject.SetActive(false);
    }

    private IEnumerator SetTimer()
    {
        int index = 0;
        timer.gameObject.SetActive(true);

        isStartCouroutine = true;
        while (index < 4)
        {
            SetImage(index);
            index++;
            yield return new WaitForSecondsRealtime(1f);
        }
        index = 0;
        isStartCouroutine = false;
        FFF_GameManager.Instance.PlusClearCount(false);
        mybutton.interactable = true;
    }

    private void SetImage(int _index)
    {
        timer.sprite = FFF_GameManager.Instance.ImagePool[_index];
    }
}