using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class SelectCharacter : MonoBehaviour
{
    [SerializeField] private Button Select1Button;
    [SerializeField] private Button Select2Button;
    [SerializeField] RemindSelectPopup remindSelectPopup;

    private void Start()
    {
        Select1Button.onClick.AddListener(Select1);
        Select2Button.onClick.AddListener(Select2);
    }

    private void OnEnable()
    {
        SoundManager.Instance.PlaySE("popup_open.wav");
    }

    private void OnDisable()
    {
        SoundManager.Instance.PlaySE("popup_close.wav");
    }

    public void Select1()
    {
        LoginManager.Instance.SetPopupButtonUICanvas(LoginManager.Instance.RemindSelectPopup);
        remindSelectPopup.SelectCharacter(1);
        gameObject.SetActive(false);
    }

    public void Select2()
    {
        LoginManager.Instance.SetPopupButtonUICanvas(LoginManager.Instance.RemindSelectPopup);
        remindSelectPopup.SelectCharacter(2);
        gameObject.SetActive(false);
    }
}
