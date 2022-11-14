using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PKB_CustomizingUI : MonoBehaviour
{
    private int pageNumber;
    private int characterCountInPage;

    [SerializeField] private Button checkButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    [SerializeField] private GameObject characterBase;
    [SerializeField] private GameObject[] character;
    
    private int characterCount;

    private void Awake()
    {
        characterCount = StaticData.PeekabooCustiomizingData.Length;
        character = new GameObject[characterCount];

        Initialize();
    }

    public void Initialize()
    {
        pageNumber = 0;
        characterCountInPage = 3;

        for (int i = 0; i < StaticData.PeekabooCustiomizingData.Length; i++)
        {
            character[i] = Instantiate(characterBase, characterBase.transform.parent);
            character[i].GetComponent<PKB_CustomizingCharacter>().CharacterName.text = StaticData.GetPeekabooCustomizingData(i).Ghostname;
            character[i].GetComponent<PKB_CustomizingCharacter>().CharacterImage.sprite = Resources.Load<Sprite>("Image/Peekaboo/Character/" + StaticData.GetPeekabooCustomizingData(i).Ghostimage);
            int index = i;
            character[i].GetComponent<PKB_CustomizingCharacter>().Button.onClick.AddListener(() => OnClickCharacterButton(index));
        }

        RefreshUI();
    }

    void Start()
    {
        checkButton.onClick.AddListener(OnClickCheckButton);
        leftButton.onClick.AddListener(OnClickLeftButton);
        rightButton.onClick.AddListener(OnClickRightButton);
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

    public void OnClickCharacterButton(int _characterValue)
    {
        UnityEngine.Debug.Log("_characterValue : " + _characterValue);
        GameManager.Instance.PlayerData.PeekabooData.SelectCharacter = _characterValue;
        PKB_MainUIManager.Instance.MainUI.RefreshUI();
        RefreshUI();
        PeekabooDataBase.Instance.SaveSelectCharater();
    }

    public void RefreshUI()
    {
        for (int i = 0; i < characterCount; i++)
        {
            ChangePageUI(i);
            ChangeCharacterUI(i);
        }
    }

    public void ChangePageUI(int _countNumber)
    {
        if (characterCountInPage * pageNumber <= _countNumber && _countNumber < characterCountInPage * (pageNumber + 1))
        {
            character[_countNumber].gameObject.SetActive(true);
        }
        else
        {
            character[_countNumber].gameObject.SetActive(false);
        }
    }

    public void ChangeCharacterUI(int _countNumber)
    {
        if (GameManager.Instance.PlayerData.PeekabooData.CharacterList.Character[_countNumber] > 0)
        {
            if (GameManager.Instance.PlayerData.PeekabooData.SelectCharacter == _countNumber)
            {
                character[_countNumber].GetComponent<PKB_CustomizingCharacter>().Button.interactable = false;
                character[_countNumber].GetComponent<PKB_CustomizingCharacter>().ButtonText.text = "적용중";
            }
            else
            {
                character[_countNumber].GetComponent<PKB_CustomizingCharacter>().Button.interactable = true;
                character[_countNumber].GetComponent<PKB_CustomizingCharacter>().ButtonText.text = "변경";
            }
        }
        else
        {
            character[_countNumber].GetComponent<PKB_CustomizingCharacter>().Button.interactable = false;
            character[_countNumber].GetComponent<PKB_CustomizingCharacter>().ButtonText.text = "미보유";
        }
    }
}
