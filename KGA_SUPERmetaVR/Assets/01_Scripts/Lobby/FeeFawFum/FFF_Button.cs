using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FFF_Button : MonoBehaviour
{
    private Button mybutton;
    [SerializeField] Image timer;
    private bool isStartCouroutine = false;
    private int index = 0;
    private void Awake()
    {
        mybutton = GetComponent<Button>();
        timer.gameObject.SetActive(false);
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
    }

    public void OnClickButton()
    {
        StopCoroutine(SetTimer());
        isStartCouroutine = false;
        FFF_GameManager.Instance.PlusClearCount(true);
        mybutton.interactable = false;
        timer.sprite = null;
    }

    private IEnumerator SetTimer()
    {
        isStartCouroutine = true;
        while (index <= 4)
        {
            SetImage(index);
            index++;
            yield return new WaitForSecondsRealtime(1f);
        }
        index = 0;
        FFF_GameManager.Instance.PlusClearCount(false);
    }

    private void SetImage(int _index)
    {
        timer.sprite = FFF_GameManager.Instance.ImagePool[_index];
    }
}