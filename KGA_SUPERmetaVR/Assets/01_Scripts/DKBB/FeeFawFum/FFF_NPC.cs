using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class FFF_NPC : MonoBehaviour
{
    [SerializeField] TextMeshPro npcName;
    [SerializeField] Button exclamationButton;
    [SerializeField] Button startButton;
    private bool readyState;

    private void Start()
    {
        npcName.color = new Color(192, 0, 0, 0);

        exclamationButton.onClick.AddListener(OnClickExclamationButton);
        startButton.onClick.AddListener(OnClickStartButton);

        exclamationButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);

        readyState = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(readyState && other.tag == "Player")
        {
            detectPlayer(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (readyState && other.tag == "Player")
        {
            detectPlayer(false);
        }
    }
    private void detectPlayer(bool _isActive)
    {
        if(_isActive)
        {
            npcName.color = new Color(135, 247, 242, 0);
            exclamationButton.gameObject.SetActive(true);
        }
        else
        {
            npcName.color = new Color(192, 0, 0, 0);
            exclamationButton.gameObject.SetActive(false);
        }
    }

    public void OnClickExclamationButton()
    {
        exclamationButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
    }

    public void OnClickStartButton()
    {
        startButton.gameObject.SetActive(false);
        readyState = false;
    } 
}
