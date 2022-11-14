using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendList : MonoBehaviour
{
    [SerializeField] private int id;
    public int ID { get { return id; } set { id = value; } }

    [SerializeField] private TextMeshProUGUI nickNameText;
    public TextMeshProUGUI NickNameText { get { return nickNameText; } set { nickNameText = value; } }

    [SerializeField] private GameObject connectImage;
    public GameObject ConnectImage { get { return connectImage; } set { connectImage = value; } }

    [SerializeField] private Button voiceChateButton;
    public Button VoiceChateButton { get { return voiceChateButton; } set { voiceChateButton = value; } }

    [SerializeField] private Button deleteButton;
    public Button DeleteButton { get { return deleteButton; } set { deleteButton = value; } }
}
