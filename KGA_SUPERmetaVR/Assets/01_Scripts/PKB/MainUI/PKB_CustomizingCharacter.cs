using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class PKB_CustomizingCharacter : MonoBehaviour
{
    [SerializeField] private Image characterImage;
    public Image CharacterImage { get { return characterImage; } set { characterImage = value; } }

    [SerializeField] private TextMeshProUGUI characterName;
    public TextMeshProUGUI CharacterName { get { return characterName; } set { characterName = value; } }

    [SerializeField] private Button button;
    public Button Button { get { return button; } set { button = value; } }

    [SerializeField] private Sprite[] buttonImage;

    /// <summary>
    /// _imageIndex : 0-미보유, 1-적용중, 2-변경
    /// </summary>
    /// <param name="_imageIndex"></param>
    /// <param name="_isInteractable"></param>
    public void SetStartButton(int _imageIndex, bool _isInteractable)
    {
        button.interactable = _isInteractable;
        button.transform.GetComponent<Image>().sprite = buttonImage[_imageIndex];
    }
}
