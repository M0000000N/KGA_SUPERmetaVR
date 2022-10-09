using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class CustomizingUI : MonoBehaviour
{
    int pageNumber;
    int characterCountInPage;

    [SerializeField] Button CheckButton;
    [SerializeField] Button LeftButton;
    [SerializeField] Button RightButton;

    [SerializeField] GameObject[] Character;
    //[SerializeField] GameObject Character1;
    //[SerializeField] GameObject Character2;
    //[SerializeField] GameObject Character3;
    //[SerializeField] GameObject Character4;
    //[SerializeField] GameObject Character5;

    Button[] CharacterButton;
    //private Button Character1Button;
    //private Button Character2Button;
    //private Button Character3Button;
    //private Button Character4Button;
    //private Button Character5Button;

    void Awake()
    {
        CharacterButton = new Button[Character.Length];
        for (int i = 0; i < Character.Length; i++)
        {
            CharacterButton[i] = Character[i].GetComponentInChildren<Button>();
        }
        //Character1Button = Character1.GetComponentInChildren<Button>(); 
        //Character2Button = Character2.GetComponentInChildren<Button>();
        //Character3Button = Character3.GetComponentInChildren<Button>();
        //Character4Button = Character4.GetComponentInChildren<Button>();
        //Character5Button = Character5.GetComponentInChildren<Button>();

        Initialize();
    }

    public void Initialize()
    {
        pageNumber = 0;
        characterCountInPage = 3;
        RefreshUI();
    }

    void Start()
    {
        CheckButton.onClick.AddListener(OnClickCheckButton);
        LeftButton.onClick.AddListener(OnClickLeftButton);
        RightButton.onClick.AddListener(OnClickRightButton);
        for (int i = 0; i < CharacterButton.Length; i++)
        {
            CharacterButton[i].onClick.AddListener(OnClickCharacterButton);
        }
        //Character1Button.onClick.AddListener(OnClickCharacter1Button);
        //Character2Button.onClick.AddListener(OnClickCharacter2Button);
        //Character3Button.onClick.AddListener(OnClickCharacter3Button);
        //Character4Button.onClick.AddListener(OnClickCharacter4Button);
        //Character5Button.onClick.AddListener(OnClickCharacter5Button);        
    }



    public void OnClickCheckButton()
    {
        gameObject.SetActive(false);
    }
    public void OnClickLeftButton()
    {
        if (pageNumber <= 0) return;

        LeftButton.interactable = false;
        RightButton.interactable = true;
        pageNumber--;

        RefreshUI();
    }
    public void OnClickRightButton()
    {
        if (Character.Length <= characterCountInPage * (pageNumber + 1)) return;

        LeftButton.interactable = true;
        RightButton.interactable = false;
        pageNumber++;

        RefreshUI();
        //Character1.gameObject.SetActive(false);
        //Character2.gameObject.SetActive(false);
        //Character3.gameObject.SetActive(false);
        //Character4.gameObject.SetActive(true);
        //Character5.gameObject.SetActive(true);
    }
    public void OnClickCharacterButton()
    {
        // player.Character1 모델키고 나머지 다 꺼
    }

    //public void OnClickCharacter2Button()
    //{
    //    // player.Character2 모델키고 나머지 다 꺼
    //}
    //public void OnClickCharacter3Button()
    //{
    //    // player.Character3 모델키고 나머지 다 꺼
    //}
    //public void OnClickCharacter4Button()
    //{
    //    // player.Character4 모델키고 나머지 다 꺼
    //}

    //public void OnClickCharacter5Button()
    //{
    //    // player.Character5 모델키고 나머지 다 꺼
    //}

    public void RefreshUI()
    {
        ChangePage();
    }

    public void ChangePage()
    {
        for (int i = 0; i < Character.Length; i++)
        {
            // 유저가 가지고 있나요 없나요
            if (characterCountInPage * pageNumber <= i && i < characterCountInPage * (pageNumber + 1))
            {
                Character[i].gameObject.SetActive(true);
                continue;
            }
            Character[i].gameObject.SetActive(false);
        }
    }
}
