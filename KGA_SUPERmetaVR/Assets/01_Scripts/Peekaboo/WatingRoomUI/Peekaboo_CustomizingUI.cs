using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class Peekaboo_CustomizingUI : MonoBehaviour
{
    private int pageNumber;
    private int characterCountInPage;

    [SerializeField] private Button checkButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    [SerializeField] private GameObject characterBase;
    [SerializeField] private GameObject[] character;
    
    private Button[] characterButton;
    private int characterCount;

    private void Awake()
    {
        characterCount = StaticData.PeekabooCustiomizingData.Length;
        characterButton = new Button[characterCount];
        character = new GameObject[characterCount];

        Initialize();
    }

    private void OnEnable()
    {
        
    }

    public void Initialize()
    {
        pageNumber = 0;
        characterCountInPage = 3;

        for (int i = 0; i < StaticData.PeekabooCustiomizingData.Length; i++)
        {
            character[i] = Instantiate(characterBase, characterBase.transform.parent);
            characterButton[i] = character[i].GetComponentInChildren<Button>();
            character[i].GetComponent<Peekaboo_CustomizingCharacter>().CharacterName.text = StaticData.GetPeekabooCustomizingData(i).Ghostname;
        }

        RefreshUI();
    }

    void Start()
    {
        checkButton.onClick.AddListener(OnClickCheckButton);
        leftButton.onClick.AddListener(OnClickLeftButton);
        rightButton.onClick.AddListener(OnClickRightButton);

        for (int i = 0; i < characterButton.Length; i++)
        {
            characterButton[i].onClick.AddListener(() => OnClickCharacterButton(i));
        }  
    }

    public void OnClickCheckButton()
    {
        gameObject.SetActive(false);
    }

    public void OnClickLeftButton()
    {
        if (pageNumber <= 0) return;

        leftButton.interactable = false;
        rightButton.interactable = true;
        pageNumber--;

        RefreshUI();
    }

    public void OnClickRightButton()
    {
        if (characterCount <= characterCountInPage * (pageNumber + 1)) return;

        leftButton.interactable = true;
        rightButton.interactable = false;
        pageNumber++;

        RefreshUI();
    }

    public void OnClickCharacterButton(int _characterNumber)
    {
        GameManager.Instance.PlayerData.PlayerPeekabooData.SelectCharacter = _characterNumber;
    }

    public void RefreshUI()
    {
        ChangePageUI();
    }

    public void ChangePageUI()
    {
        for (int i = 0; i < characterCount; i++)
        {
            // 유저가 가지고 있나요 없나요

            if (characterCountInPage * pageNumber <= i && i < characterCountInPage * (pageNumber + 1))
            {
                character[i].gameObject.SetActive(true);
                continue;
            }
            character[i].gameObject.SetActive(false);
        }
    }

    public void ChangeCharacterUI()
    {
        for (int i = 0; i < characterCount; i++)
        {

        }
    }
}
