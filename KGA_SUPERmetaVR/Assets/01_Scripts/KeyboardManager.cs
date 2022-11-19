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
        Initialize();

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

    // 한글 입력
    string firstConsonant = "ㄱㄲㄴㄷㄸㄹㅁㅂㅃㅅㅆㅇㅈㅉㅊㅋㅌㅍㅎ";
    string middleVowel = "ㅏㅐㅑㅒㅓㅔㅕㅖㅗㅘㅙㅚㅛㅜㅝㅞㅟㅠㅡㅢㅣ";
    string lastConsonant = " ㄱㄲㄳㄴㄵㄶㄷㄹㄺㄻㄼㄽㄾㄿㅀㅁㅂㅄㅅㅆㅇㅈㅊㅋㅌㅍㅎ";

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
            }
        }

        for (int i = 0; i < convertString.Length;)
        {
            //if (_input.Length - i >= 4)
            //{
            //    // 옩 완
            //    int firstIndex = firstConsonant.IndexOf(_input[i]);
            //    int middleIndex = middleVowel.IndexOf(_input[i + 1]);
            //    int lastIndex = lastConsonant.IndexOf(_input[i + 2]);
            //    int nextMiddleIndex = middleVowel.IndexOf(_input[i + 3]);

            //    int secondConsonantIndex = lastConsonant.IndexOf(_input[i + 2]);
            //    int thirdConsonantIndex = lastConsonant.IndexOf(_input[i + 3]);

            //    int secondVowelIndex = middleVowel.IndexOf(_input[i + 1]);
            //    int thirdVowelIndex = middleVowel.IndexOf(_input[i + 2]);
            //    int nextConsonantIndex = lastConsonant.IndexOf(_input[i + 3]);

            //    if (firstIndex != -1 && secondConsonantIndex != -1 && thirdConsonantIndex != -1)
            //    {
            //        lastIndex = CheckTwoConsonant(lastIndex, secondConsonantIndex, thirdConsonantIndex);

            //        int index = (0xAC00 + (firstIndex * 588) + (middleIndex * 28) + lastIndex);
            //        char text = Convert.ToChar(index);
            //        output += text.ToString();
            //        i += 4;
            //        continue;
            //    }
            //    else if (firstIndex != -1 && secondVowelIndex != -1 && thirdVowelIndex != -1 && nextConsonantIndex != -1)
            //    {
            //        middleIndex = CheckTwoVowel(middleIndex, secondVowelIndex, thirdVowelIndex);

            //        int index = (0xAC00 + (firstIndex * 588) + (middleIndex * 28) + nextConsonantIndex);
            //        char text = Convert.ToChar(index);
            //        output += text.ToString();
            //        i += 4;
            //        continue;
            //    }
            //    else if (firstIndex != -1 && secondVowelIndex != -1 && thirdVowelIndex != -1 && nextConsonantIndex == -1)
            //    {
            //        middleIndex = CheckTwoVowel(middleIndex, secondVowelIndex, thirdVowelIndex);
            //        lastIndex = 0;

            //        int index = (0xAC00 + (firstIndex * 588) + (middleIndex * 28) + lastIndex);
            //        char text = Convert.ToChar(index);
            //        output += text.ToString();
            //        i += 3;
            //        continue;

            //    }
            //    else if (firstIndex != -1 && middleIndex != -1 && lastIndex != -1 && nextMiddleIndex == -1)
            //    {
            //        int index = (0xAC00 + (firstIndex * 588) + (middleIndex * 28) + lastIndex);
            //        char text = Convert.ToChar(index);
            //        output += text.ToString();
            //        i += 3;
            //    }
            //    else if(firstIndex != -1 && middleIndex != -1 && lastIndex != -1 && nextMiddleIndex != -1)
            //    {
            //        lastIndex = 0;
            //        int index = (0xAC00 + (firstIndex * 588) + (middleIndex * 28) + lastIndex);
            //        char text = Convert.ToChar(index);
            //        output += text.ToString();
            //        i += 2;
            //    }
            //    else
            //    {
            //        int index = _input[i];
            //        char text = Convert.ToChar(index);
            //        output += text.ToString();
            //        i += 1;
            //    }
            //}
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

    public int CheckTwoConsonant(int _consonant, int _second, int _thrid)
    {
        int outputValue = _consonant;

        int ㄱ = lastConsonant.IndexOf('ㄱ');
        int ㅁ = lastConsonant.IndexOf('ㅁ');
        int ㅂ = lastConsonant.IndexOf('ㅂ');
        int ㅅ = lastConsonant.IndexOf('ㅅ');
        int ㅈ = lastConsonant.IndexOf('ㅈ');
        int ㅌ = lastConsonant.IndexOf('ㅌ');
        int ㅍ = lastConsonant.IndexOf('ㅍ');
        int ㅎ = lastConsonant.IndexOf('ㅎ');

        if(_second == lastConsonant.IndexOf('ㄱ'))
        {
            if (_thrid == ㅅ)
            {
                outputValue = lastConsonant.IndexOf('ㄳ');
            }
        }
        else if (_second == lastConsonant.IndexOf('ㄴ'))
        {
            if (_thrid == ㅈ)
            {
                outputValue = lastConsonant.IndexOf('ㄵ');
            }
            else if (_thrid == ㅎ)
            {
                outputValue = lastConsonant.IndexOf('ㄶ');
            }
        }
        else if (_second == lastConsonant.IndexOf('ㄹ'))
        {
            if (_thrid == ㄱ)
            {
                outputValue = lastConsonant.IndexOf('ㄺ');
            }
            else if (_thrid == ㅁ)
            {
                outputValue = lastConsonant.IndexOf('ㄻ');
            }
            else if (_thrid == ㅂ)
            {
                outputValue = lastConsonant.IndexOf('ㄼ');
            }
            else if (_thrid == ㅅ)
            {
                outputValue = lastConsonant.IndexOf('ㄽ');
            }
            else if (_thrid == ㅌ)
            {
                outputValue = lastConsonant.IndexOf('ㄾ');
            }
            else if (_thrid == ㅎ)
            {
                outputValue = lastConsonant.IndexOf('ㅀ');
            }
        }
        else if (_second == lastConsonant.IndexOf('ㅂ'))
        {
            if (_thrid == ㅅ)
            {
                outputValue = lastConsonant.IndexOf('ㅄ');
            }
        }
        return outputValue;
    }

    public int CheckTwoVowel(int _middleVowel, int _second, int _thrid)
    {
        int outputValue = _middleVowel;

        int ㅏ = middleVowel.IndexOf('ㅏ');
        int ㅓ = middleVowel.IndexOf('ㅓ');
        int ㅐ = middleVowel.IndexOf('ㅐ');
        int ㅔ = middleVowel.IndexOf('ㅔ');
        int ㅣ = middleVowel.IndexOf('ㅣ');

        if (_second == middleVowel.IndexOf('ㅗ'))
        {
            if (_thrid == ㅏ)
            {
                outputValue = middleVowel.IndexOf('ㅘ');
            }
            else if (_thrid == ㅐ)
            {
                outputValue = middleVowel.IndexOf('ㅙ');
            }
            else if (_thrid == ㅣ)
            {
                outputValue = middleVowel.IndexOf('ㅚ');
            }
        }
        else if (_second == middleVowel.IndexOf('ㅜ'))
        {
            if (_thrid == ㅓ)
            {
                outputValue = middleVowel.IndexOf('ㅝ');
            }
            else if (_thrid == ㅔ)
            {
                outputValue = middleVowel.IndexOf('ㅞ');
            }
            else if (_thrid == ㅣ)
            {
                outputValue = middleVowel.IndexOf('ㅟ');
            }
        }
        else if (_second == middleVowel.IndexOf('ㅡ'))
        {
            if (_thrid == ㅣ)
            {
                outputValue = middleVowel.IndexOf('ㅢ');
            }
        }
        return outputValue;
    }
}
