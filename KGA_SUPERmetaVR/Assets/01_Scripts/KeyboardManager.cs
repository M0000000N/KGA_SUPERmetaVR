using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class KeyboardManager : SingletonBehaviour<KeyboardManager>
{
    TMP_InputField inputField;
    string inputText;

    [SerializeField] private Transform login;
    [SerializeField] private Transform newLogin;

    [SerializeField] private GameObject qwerty;
    [SerializeField] private GameObject numpad;

    [SerializeField] private int type;

    [SerializeField] private GameObject[] englishKorean;

    [SerializeField] private GameObject[] shiftON;
    [SerializeField] private GameObject[] shiftOFF;
    private bool isShift;

    [SerializeField] private TMP_InputField[] inputFields;
    [SerializeField] private TMP_InputField[] inputFieldsNewLogin;

    private void Awake()
    {
        inputFields = login.GetComponentsInChildren<TMP_InputField>();
        inputFieldsNewLogin = newLogin.GetComponentsInChildren<TMP_InputField>();

        for (int i = 0; i < inputFields.Length; i++)
        {
            int keyType = 0;

            if (inputFields[i].gameObject.name.Contains("Birth") || type == 1)
            {
                keyType = 1;
            }

            inputFields[i].onSelect.RemoveAllListeners();
            inputFields[i].onSelect.AddListener( delegate { OpenKeyboard(keyType); } );
        }

        for (int i = 0; i < inputFieldsNewLogin.Length; i++)
        {
            int keyType = 0;

            if (inputFieldsNewLogin[i].gameObject.name.Contains("Birth") || type == 1)
            {
                keyType = 1;
            }

            inputFieldsNewLogin[i].onSelect.RemoveAllListeners();
            inputFieldsNewLogin[i].onSelect.AddListener(delegate { OpenKeyboard(keyType); });
        }

        CloseKeyboard();
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
        Initialize();
        
        switch (_type)
        {
            case 0: // qwerty + numpad
                qwerty.SetActive(true);
                numpad.SetActive(true);
                break;
            case 1: // only numpad
                qwerty.SetActive(false);
                numpad.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void CloseKeyboard()
    {
        inputText = string.Empty;
        qwerty.SetActive(false);
        numpad.SetActive(false);
    }

    public void PressKey()
    {
        inputText += EventSystem.current.currentSelectedGameObject.name;
        inputField.text = CreateKoreanText(inputText);
    }

    public void PressBackspace()
    {
        if (inputText.Length == 0) return;
        inputText = inputText.Substring(0, inputText.Length - 1);
        inputField.text = CreateKoreanText(inputText);
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

    // =========================================================== 廃越 脊径 =========================================================== 

    string firstConsonant = "ぁあいぇえぉけげこさざしじすずせぜそぞ";
    string middleVowel = "ただちぢっつづてでとどなにぬねのはばぱひび";
    string lastConsonant = " ぁあぃいぅうぇぉおかがきぎくぐけげごさざしじずせぜそぞ";

    public string CreateKoreanText(string _input)
    {
        string convertString = string.Empty;
        string output = string.Empty;

        for (int i = 0; i < _input.Length;)
        {

            if(_input.Length - i >= 3)
            {
                int firstConsonantIndex = lastConsonant.IndexOf(_input[i]);
                int secondConsonantIndex = lastConsonant.IndexOf(_input[i + 1]);
                int thirdConsonantIndex = lastConsonant.IndexOf(_input[i + 2]);
                
                int firstVowelIndex = middleVowel.IndexOf(_input[i]);
                int secondVowelIndex = middleVowel.IndexOf(_input[i + 1]);

                if(firstConsonantIndex != -1 && secondConsonantIndex != -1 && thirdConsonantIndex != -1)
                {
                    int index = CheckTwoConsonant(firstConsonantIndex, firstConsonantIndex, secondConsonantIndex);
                    if(index == firstConsonantIndex)
                    {
                        i += 1;
                    }
                    else
                    {
                        firstConsonantIndex = index;
                        i += 2;
                    }
                    convertString += lastConsonant[firstConsonantIndex];
                }
                else if (firstVowelIndex != -1 && secondVowelIndex != -1)
                {
                    int index = CheckTwoVowel(firstVowelIndex, firstVowelIndex, secondVowelIndex);

                    if (index == firstVowelIndex)
                    {
                        i += 1;
                    }
                    else
                    {
                        firstVowelIndex = index;
                        i += 2;
                    }
                    convertString += middleVowel[firstVowelIndex];
                }
                else
                {
                    if (firstConsonantIndex != -1)
                    {
                        i += 1;
                        convertString += lastConsonant[firstConsonantIndex];
                    }
                    else if (firstVowelIndex != -1)
                    {
                        i += 1;
                        convertString += middleVowel[firstVowelIndex];
                    }
                    else
                    {
                        convertString += _input[i];
                        i += 1;
                    }
                }
            }
            else if (_input.Length - i >= 2)
            {
                int firstConsonantIndex = lastConsonant.IndexOf(_input[i]);
                int secondConsonantIndex = lastConsonant.IndexOf(_input[i + 1]);

                int firstVowelIndex = middleVowel.IndexOf(_input[i]);
                int secondVowelIndex = middleVowel.IndexOf(_input[i + 1]);

                if (firstConsonantIndex != -1 && secondConsonantIndex != -1)
                {
                    int index = CheckTwoConsonant(firstConsonantIndex, firstConsonantIndex, secondConsonantIndex);
                    if (index == firstConsonantIndex)
                    {
                        i += 1;
                    }
                    else
                    {
                        firstConsonantIndex = index;
                        i += 2;
                    }
                    convertString += lastConsonant[firstConsonantIndex];
                }
                else if (firstVowelIndex != -1 && secondVowelIndex != -1)
                {
                    int index = CheckTwoVowel(firstVowelIndex, firstVowelIndex, secondVowelIndex);

                    if (index == firstVowelIndex)
                    {
                        i += 1;
                    }
                    else
                    {
                        firstVowelIndex = index;
                        i += 2;
                    }
                    convertString += middleVowel[firstVowelIndex];
                }
                else
                {
                    if (firstConsonantIndex != -1)
                    {
                        i += 1;
                        convertString += lastConsonant[firstConsonantIndex];
                    }
                    else if (firstVowelIndex != -1)
                    {
                        i += 1;
                        convertString += middleVowel[firstVowelIndex];
                    }
                    else
                    {
                        convertString += _input[i];
                        i += 1;
                    }
                }
            }
            else if (_input.Length - i >= 1)
            {
                int firstConsonantIndex = lastConsonant.IndexOf(_input[i]);

                int firstVowelIndex = middleVowel.IndexOf(_input[i]);

                if (firstConsonantIndex != -1)
                {
                    i += 1;
                    convertString += lastConsonant[firstConsonantIndex];
                }
                else if (firstVowelIndex != -1)
                {
                    i += 1;
                    convertString += middleVowel[firstVowelIndex];
                }
                else
                {
                    convertString += _input[i];
                    i += 1;
                }
            }
        }

        for (int i = 0; i < convertString.Length;)
        {
            if (convertString.Length - i >= 3)
            {
                int firstIndex = firstConsonant.IndexOf(convertString[i]);
                int middleIndex = middleVowel.IndexOf(convertString[i + 1]);
                int lastIndex = lastConsonant.IndexOf(convertString[i + 2]);

                int secondVowelIndex = middleVowel.IndexOf(convertString[i + 1]);
                int thirdVowelIndex = middleVowel.IndexOf(convertString[i + 2]);

                if (firstIndex != -1 && secondVowelIndex != -1 && thirdVowelIndex != -1)
                {
                    middleIndex = CheckTwoVowel(middleIndex, secondVowelIndex, thirdVowelIndex);
                    lastIndex = 0;

                    int index = (0xAC00 + (firstIndex * 588) + (middleIndex * 28) + lastIndex);
                    char text = Convert.ToChar(index);
                    output += text.ToString();
                    i += 3;
                    continue;
                }
                else if (firstIndex != -1 && middleIndex != -1 && lastIndex != -1)
                {
                    int index = (0xAC00 + (firstIndex * 588) + (middleIndex * 28) + lastIndex);
                    char text = Convert.ToChar(index);
                    output += text.ToString();
                    i += 3;
                }
                else
                {
                    int index = convertString[i];
                    char text = Convert.ToChar(index);
                    output += text.ToString();
                    i += 1;
                }
            }
            else if (convertString.Length - i >= 2)
            {
                int firstIndex = firstConsonant.IndexOf(convertString[i]);
                int middleIndex = middleVowel.IndexOf(convertString[i + 1]);
                int lastIndex = 0;

                if (firstIndex != -1 && middleIndex != -1 && lastIndex != -1)
                {
                    int index = (0xAC00 + (firstIndex * 588) + (middleIndex * 28) + lastIndex);
                    char text = Convert.ToChar(index);
                    output += text.ToString();
                    i += 2;
                }
                else
                {
                    int index = convertString[i];
                    char text = Convert.ToChar(index);
                    output += text.ToString();
                    i += 1;
                }
            }
            else if (convertString.Length - i >= 1)
            {
                int index = convertString[i];
                char text = Convert.ToChar(index);
                output += text.ToString();
                i += 1;
            }
        }

        return output;
    }

    // =========================================================== 戚掻切乞製 =========================================================== 

    public int CheckTwoConsonant(int _consonant, int _second, int _thrid)
    {
        int outputValue = _consonant;

        int ぁ = lastConsonant.IndexOf('ぁ');
        int け = lastConsonant.IndexOf('け');
        int げ = lastConsonant.IndexOf('げ');
        int さ = lastConsonant.IndexOf('さ');
        int じ = lastConsonant.IndexOf('じ');
        int ぜ = lastConsonant.IndexOf('ぜ');
        int そ = lastConsonant.IndexOf('そ');
        int ぞ = lastConsonant.IndexOf('ぞ');

        if(_second == lastConsonant.IndexOf('ぁ'))
        {
            if (_thrid == さ)
            {
                outputValue = lastConsonant.IndexOf('ぃ');
            }
        }
        else if (_second == lastConsonant.IndexOf('い'))
        {
            if (_thrid == じ)
            {
                outputValue = lastConsonant.IndexOf('ぅ');
            }
            else if (_thrid == ぞ)
            {
                outputValue = lastConsonant.IndexOf('う');
            }
        }
        else if (_second == lastConsonant.IndexOf('ぉ'))
        {
            if (_thrid == ぁ)
            {
                outputValue = lastConsonant.IndexOf('お');
            }
            else if (_thrid == け)
            {
                outputValue = lastConsonant.IndexOf('か');
            }
            else if (_thrid == げ)
            {
                outputValue = lastConsonant.IndexOf('が');
            }
            else if (_thrid == さ)
            {
                outputValue = lastConsonant.IndexOf('き');
            }
            else if (_thrid == ぜ)
            {
                outputValue = lastConsonant.IndexOf('ぎ');
            }
            else if (_thrid == ぞ)
            {
                outputValue = lastConsonant.IndexOf('ぐ');
            }
        }
        else if (_second == lastConsonant.IndexOf('げ'))
        {
            if (_thrid == さ)
            {
                outputValue = lastConsonant.IndexOf('ご');
            }
        }
        return outputValue;
    }

    public int CheckTwoVowel(int _middleVowel, int _second, int _thrid)
    {
        int outputValue = _middleVowel;

        int た = middleVowel.IndexOf('た');
        int っ = middleVowel.IndexOf('っ');
        int だ = middleVowel.IndexOf('だ');
        int つ = middleVowel.IndexOf('つ');
        int び = middleVowel.IndexOf('び');

        if (_second == middleVowel.IndexOf('で'))
        {
            if (_thrid == た)
            {
                outputValue = middleVowel.IndexOf('と');
            }
            else if (_thrid == だ)
            {
                outputValue = middleVowel.IndexOf('ど');
            }
            else if (_thrid == び)
            {
                outputValue = middleVowel.IndexOf('な');
            }
        }
        else if (_second == middleVowel.IndexOf('ぬ'))
        {
            if (_thrid == っ)
            {
                outputValue = middleVowel.IndexOf('ね');
            }
            else if (_thrid == つ)
            {
                outputValue = middleVowel.IndexOf('の');
            }
            else if (_thrid == び)
            {
                outputValue = middleVowel.IndexOf('は');
            }
        }
        else if (_second == middleVowel.IndexOf('ぱ'))
        {
            if (_thrid == び)
            {
                outputValue = middleVowel.IndexOf('ひ');
            }
        }
        return outputValue;
    }
}
