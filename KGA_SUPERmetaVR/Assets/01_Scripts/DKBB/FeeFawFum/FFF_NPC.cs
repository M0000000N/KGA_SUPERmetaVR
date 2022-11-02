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

    private void Start()
    {
        exclamationButton.onClick.AddListener(OnClickExclamationButton);
        startButton.onClick.AddListener(OnClickStartButton);

        exclamationButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (FFF_GameManager.Instance.flow == 0 && other.tag == "Player")
        {
            detectPlayer(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (FFF_GameManager.Instance.flow == 0 && other.tag == "Player")
        {
            detectPlayer(false);
        }
    }

    private void detectPlayer(bool _isActive)
    {
        if (_isActive)
        {
            // TODO : 세마리 다 이렇게 바뀌어야함
            npcName.color = new Color32(135, 247, 242, 255);
            exclamationButton.gameObject.SetActive(true);
        }
        else
        {
            npcName.color = new Color32(192, 0, 0, 255);
            exclamationButton.gameObject.SetActive(false);
        }
    }

    public void OnClickExclamationButton()
    {
        exclamationButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
        // TODO : 나머지 두마리 모여모여
    }

    public void OnClickStartButton()
    {
        startButton.gameObject.SetActive(false);
        FFF_GameManager.Instance.flow = 1;
    }
}
