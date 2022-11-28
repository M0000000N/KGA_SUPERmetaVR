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
        Select1Button.onClick.AddListener(()=> { Select(1); });
        Select2Button.onClick.AddListener(()=> { Select(2); });
    }

    private void OnEnable()
    {
        SoundManager.Instance.PlaySE("popup_open.wav");
    }

    private void OnDisable()
    {
        SoundManager.Instance.PlaySE("popup_close.wav");
    }

    public void Select(int _selectNumber)
    {
        remindSelectPopup.SelectCharacter(_selectNumber);
        gameObject.SetActive(false);
    }
}
