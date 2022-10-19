using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Peekaboo_CustomizingCharacter : MonoBehaviour
{
    [SerializeField] private Image characterImage;
    public Image CharacterImage { get { return characterImage; } set { characterImage = value; } }
    [SerializeField] private TextMeshProUGUI characterName;
    public TextMeshProUGUI CharacterName { get { return characterName; } set { characterName = value; } }
    [SerializeField] private TextMeshProUGUI characterState;
    public TextMeshProUGUI CharacterState { get { return characterName; } set { characterName = value; } }
    [SerializeField] private Button button;
    public Button Button { get { return button; } set { button = value; } }
    [SerializeField] private TextMeshProUGUI buttonText;
    public TextMeshProUGUI ButtonText { get { return buttonText; } set { buttonText = value; } }

}
