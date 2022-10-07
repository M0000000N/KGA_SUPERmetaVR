using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class CustomizingUI : MonoBehaviour
{
    [SerializeField] Button CheckButton;
    [SerializeField] Button LeftButton;
    [SerializeField] Button RightButton;

    [SerializeField] GameObject Character1;
    [SerializeField] GameObject Character2;
    [SerializeField] GameObject Character3;
    [SerializeField] GameObject Character4;
    [SerializeField] GameObject Character5;

    private Button Character1Button;
    private Button Character2Button;
    private Button Character3Button;
    private Button Character4Button;
    private Button Character5Button;

    private void Awake()
    {
        Character1Button = Character1.GetComponentInChildren<Button>(); 
        Character2Button = Character2.GetComponentInChildren<Button>();
        Character3Button = Character3.GetComponentInChildren<Button>();
        Character4Button = Character4.GetComponentInChildren<Button>();
        Character5Button = Character5.GetComponentInChildren<Button>();

        Character4.gameObject.SetActive(false);
        Character5.gameObject.SetActive(false);

        LeftButton.interactable = false;
        Character2Button.interactable = false;
        Character3Button.interactable = false;
        Character4Button.interactable = false;
        Character5Button.interactable = false;
    }

    private void Start()
    {
        CheckButton.onClick.AddListener(OnClickCheckButton);
        LeftButton.onClick.AddListener(OnClickLeftButton);
        RightButton.onClick.AddListener(OnClickRightButton);
        Character1Button.onClick.AddListener(OnClickCharacter1Button);
        Character2Button.onClick.AddListener(OnClickCharacter2Button);
        Character3Button.onClick.AddListener(OnClickCharacter3Button);
        Character4Button.onClick.AddListener(OnClickCharacter4Button);
        Character5Button.onClick.AddListener(OnClickCharacter5Button);        
    }

    public void OnClickCheckButton()
    {
        gameObject.SetActive(false);
    }
    public void OnClickLeftButton()
    {
        LeftButton.interactable = false;
        RightButton.interactable = true;

        Character1.gameObject.SetActive(true);
        Character2.gameObject.SetActive(true);
        Character3.gameObject.SetActive(true);
        Character4.gameObject.SetActive(false);
        Character5.gameObject.SetActive(false);
    }
    public void OnClickRightButton()
    {
        LeftButton.interactable = true;
        RightButton.interactable = false;

        Character1.gameObject.SetActive(false);
        Character2.gameObject.SetActive(false);
        Character3.gameObject.SetActive(false);
        Character4.gameObject.SetActive(true);
        Character5.gameObject.SetActive(true);
    }
    public void OnClickCharacter1Button()
    {
        // player.Character1 ¸ðµ¨Å°°í ³ª¸ÓÁö ´Ù ²¨
    }
    public void OnClickCharacter2Button()
    {
        // player.Character2 ¸ðµ¨Å°°í ³ª¸ÓÁö ´Ù ²¨
    }
    public void OnClickCharacter3Button()
    {
        // player.Character3 ¸ðµ¨Å°°í ³ª¸ÓÁö ´Ù ²¨
    }
    public void OnClickCharacter4Button()
    {
        // player.Character4 ¸ðµ¨Å°°í ³ª¸ÓÁö ´Ù ²¨
    }

    public void OnClickCharacter5Button()
    {
        // player.Character5 ¸ðµ¨Å°°í ³ª¸ÓÁö ´Ù ²¨
    }

}
