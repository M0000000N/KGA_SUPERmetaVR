using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FFF_Button : MonoBehaviour
{
    private Button button;
    private Image timer;
    private bool done;
    FFF_ButtonList FFF_ButtonListMenu;

    private void Start()
    {
        button = GetComponent<Button>();
        timer = GetComponentInChildren<Image>();
    }

    private void OnEnable()
    {
        StartCoroutine(SetTimer());
    }

    public void OnClickButton()
    {
        StopCoroutine(SetTimer());
        FFF_GameManager.Instance.Score++;
        FFF_GameManager.Instance.DoneCount++;
        button.interactable = false;
    }

    private IEnumerator SetTimer()
    {
        yield return new WaitForSeconds(1f);
        SetImage(0);
        new WaitForSeconds(1f);
        SetImage(1);
        new WaitForSeconds(1f);
        SetImage(2);
        new WaitForSeconds(1f);
        SetImage(3);
        FFF_GameManager.Instance.DoneCount++;
    }

    private void SetImage(int _index)
    {
        timer.sprite = FFF_ButtonListMenu.ImagePool[_index];
    }
}
