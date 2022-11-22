using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class FindCanvas : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputName;
    [SerializeField] private TMP_InputField inputBirthYear;
    [SerializeField] private TMP_InputField inputBirthMonth;
    [SerializeField] private TMP_InputField inputBirthDay;

    [SerializeField] private TMP_InputField inputID;
    [SerializeField] private TMP_InputField inputHint;
    [SerializeField] private TMP_InputField inputHintAnswer;


    [SerializeField] private Button findIDButton;
    [SerializeField] private Button findPWButton;
    [SerializeField] private Button cancelButton;

    private void Start()
    {
        findIDButton.onClick.AddListener(FindID);
        findPWButton.onClick.AddListener(FindPW);
        cancelButton.onClick.AddListener(Cancel);
    }

    private void OnEnable()
    {
        inputName.text = string.Empty;
        inputBirthYear.text = string.Empty;
        inputBirthMonth.text = string.Empty;
        inputBirthDay.text = string.Empty;

        inputID.text = string.Empty;
        inputHint.text = string.Empty;
        inputHintAnswer.text = string.Empty;
    }

    public void FindID()
    {
        // ID 찾기 기능 추가
    }

    public void FindPW()
    {
        // PW 찾기 기능 추가
    }

    public void Cancel()
    {
        //UI 교체
    }
}
