using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Peekaboo_CustomizingCharacter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI characterName;
    public TextMeshProUGUI CharacterName { get { return characterName; } set { characterName = value; } }
    [SerializeField] private TextMeshProUGUI characterState;
    public TextMeshProUGUI CharacterState { get { return characterName; } set { characterName = value; } }
}
