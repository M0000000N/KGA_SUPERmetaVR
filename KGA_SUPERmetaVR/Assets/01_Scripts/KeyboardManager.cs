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

        // inputText = inputField.text;
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

    // 廃越 脊径
    string firstConsonant = "ぁあいぇえぉけげこさざしじすずせぜそぞ";
    string middleVowel = "ただちぢっつづてでとどなにぬねのはばぱひび";
    string lastConsonant = " ぁあぃいぅうぇぉおかがきぎくぐけげごさざしじずせぜそぞ";

    public string CreateKoreanText(string _input)
    {
        string output = string.Empty;



        for (int i = 0; i < _input.Length;)
        {
            if (_input.Length - i >= 5)
            {

            }
            else if (_input.Length - i >= 5)
            {

            }
            else if (_input.Length - i >= 4)
            {



                int firstIndex = firstConsonant.IndexOf(_input[i]);
                int middleIndex = middleVowel.IndexOf(_input[i + 1]);
                int lastIndex = lastConsonant.IndexOf(_input[i + 2]);
                int nextMiddleIndex = middleVowel.IndexOf(_input[i + 3]);

                int secondVowelIndex = middleVowel.IndexOf(_input[i + 1]);
                int thirdVowelIndex = middleVowel.IndexOf(_input[i + 2]);
                int nextConsonantIndex = lastConsonant.IndexOf(_input[i + 3]);




                if (firstIndex != -1 && secondVowelIndex != -1 && thirdVowelIndex != -1 && nextConsonantIndex != -1)
                {
                    if (secondVowelIndex == middleVowel.IndexOf("で"))
                    {
                        int a = middleVowel.IndexOf("た");
                        int ae = middleVowel.IndexOf("だ");
                        int e = middleVowel.IndexOf("び");

                        if (thirdVowelIndex == a)
                        {
                            middleIndex = middleVowel.IndexOf("と");
                        }
                        else if (thirdVowelIndex == ae)
                        {
                            middleIndex = middleVowel.IndexOf("ど");
                        }
                        else if (thirdVowelIndex == e)
                        {
                            middleIndex = middleVowel.IndexOf("な");
                        }
                    }

                    int index = (0xAC00 + (firstIndex * 588) + (middleIndex * 28) + nextConsonantIndex);
                    char text = Convert.ToChar(index);
                    output += text.ToString();
                    i += 4;
                    continue;
                }
                else if (firstIndex != -1 && secondVowelIndex != -1 && thirdVowelIndex != -1 && nextConsonantIndex == -1)
                {
                    if (secondVowelIndex == middleVowel.IndexOf("で"))
                    {
                        int a = middleVowel.IndexOf("た");
                        int ae = middleVowel.IndexOf("だ");
                        int e = middleVowel.IndexOf("び");

                        if (thirdVowelIndex == a)
                        {
                            middleIndex = middleVowel.IndexOf("と");
                        }
                        else if (thirdVowelIndex == ae)
                        {
                            middleIndex = middleVowel.IndexOf("ど");
                        }
                        else if (thirdVowelIndex == e)
                        {
                            middleIndex = middleVowel.IndexOf("な");
                        }
                    }

                    lastIndex = 0;
                    int index = (0xAC00 + (firstIndex * 588) + (middleIndex * 28) + lastIndex);
                    char text = Convert.ToChar(index);
                    output += text.ToString();
                    i += 3;
                    continue;

                }
                else if (firstIndex != -1 && middleIndex != -1 && lastIndex != -1 && nextMiddleIndex == -1)
                {
                    int index = (0xAC00 + (firstIndex * 588) + (middleIndex * 28) + lastIndex);
                    char text = Convert.ToChar(index);
                    output += text.ToString();
                    i += 3;
                }
                else if(firstIndex != -1 && middleIndex != -1 && lastIndex != -1 && nextMiddleIndex != -1)
                {
                    lastIndex = 0;
                    int index = (0xAC00 + (firstIndex * 588) + (middleIndex * 28) + lastIndex);
                    char text = Convert.ToChar(index);
                    output += text.ToString();
                    i += 2;
                }
                else
                {
                    int index = _input[i];
                    char text = Convert.ToChar(index);
                    output += text.ToString();
                    i += 1;
                }
            }
            else if (_input.Length - i >= 3)
            {
                int firstIndex = firstConsonant.IndexOf(_input[i]);
                int middleIndex = middleVowel.IndexOf(_input[i + 1]);
                int lastIndex = lastConsonant.IndexOf(_input[i + 2]);

                if (firstIndex != -1 && middleIndex != -1 && lastIndex != -1)
                {
                    int index = (0xAC00 + (firstIndex * 588) + (middleIndex * 28) + lastIndex);
                    char text = Convert.ToChar(index);
                    output += text.ToString();
                    i += 3;
                }
                else
                {
                    int index = _input[i];
                    char text = Convert.ToChar(index);
                    output += text.ToString();
                    i += 1;
                }
            }
            else if (_input.Length - i >= 2)
            {
                int firstIndex = firstConsonant.IndexOf(_input[i]);
                int middleIndex = middleVowel.IndexOf(_input[i + 1]);
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
                    int index = _input[i];
                    char text = Convert.ToChar(index);
                    output += text.ToString();
                    i += 1;
                }

            }
            else if (_input.Length - i >= 1)
            {
                int index = _input[i];
                char text = Convert.ToChar(index);
                output += text.ToString();
                i += 1;
            }
        }

        return output;
    }
}
