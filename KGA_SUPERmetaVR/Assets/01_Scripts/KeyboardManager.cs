using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class KeyboardManager : SingletonBehaviour<KeyboardManager>
{
    TMP_InputField inputField;

    [SerializeField] GameObject qwerty;
    [SerializeField] GameObject numpad;

    [SerializeField] int type;

    [SerializeField] GameObject[] englishKorean;

    [SerializeField] GameObject[] shiftON;
    [SerializeField] GameObject[] shiftOFF;
    private bool isShift;

    private void Awake()
    {
        TMP_InputField[] inputFields = this.transform.parent.GetComponentsInChildren<TMP_InputField>();
        for (int i = 0; i < inputFields.Length; i++)
        {
            inputFields[i].onSelect.AddListener(delegate { OpenKeyboard(type); });
        }
    }

    public void Initialize()
    {
        isShift = false;
        shiftOFF[0].SetActive(!isShift);
        shiftOFF[1].SetActive(!isShift);
        shiftON[0].SetActive(isShift);
        shiftON[1].SetActive(isShift);

        englishKorean[0].SetActive(false);
        englishKorean[1].SetActive(true);
    }

    public void OpenKeyboard(int _type)
    {
        CloseKeyboard();

        inputField = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();

        switch (_type)
        {
            case 0: // qwerty + numpad
                qwerty.SetActive(true);
                numpad.SetActive(true);
                break;
            case 1: // only numpad
                numpad.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void CloseKeyboard()
    {
        qwerty.SetActive(false);
        numpad.SetActive(false);
    }

    public void PressKey()
    {
        inputField.text += EventSystem.current.currentSelectedGameObject.name;
    }

    public void PressBackspace()
    {
        if (inputField.text.Length == 0) return;
        inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
    }

    public void PressShiftButton()
    {
        isShift = !isShift;
        shiftOFF[0].SetActive(!isShift);
        shiftOFF[1].SetActive(!isShift);

        shiftON[0].SetActive(isShift);
        shiftON[1].SetActive(isShift);
    }

    public void ChangeLanguage()
    {
        if(englishKorean[0].activeSelf)
        {
            englishKorean[0].SetActive(false);
            englishKorean[1].SetActive(true);
        }
        else
        {
            englishKorean[0].SetActive(true);
            englishKorean[1].SetActive(false);
        }
    }
}
