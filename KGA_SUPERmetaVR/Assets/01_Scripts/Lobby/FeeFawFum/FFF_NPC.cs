// #define ?????ʾ??׽?Ʈ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FFF_NPC : MonoBehaviour
{
    [SerializeField] TextMeshPro[] npcName;
    [SerializeField] Button exclamationButton;

    private void Start()
    {
        exclamationButton.onClick.AddListener(OnClickExclamationButton);
        exclamationButton.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (FFF_GameManager.Instance.Flow == 0 && other.tag == "Player")
        {
            detectPlayer(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (FFF_GameManager.Instance.Flow == 0 && other.tag == "Player")
        {
            detectPlayer(false);
        }
    }

    private void detectPlayer(bool _isActive)
    {
        if (_isActive)
        {
            for (int i = 0; i < npcName.Length; i++)
            {
                npcName[i].color = new Color32(135, 247, 242, 255);
            }
            exclamationButton.gameObject.SetActive(true);
#if ?????ʾ??׽?Ʈ
            exclamationButton.onClick.Invoke();
#endif
        }
        else
        {
            for (int i = 0; i < npcName.Length; i++)
            {
                npcName[i].color = new Color32(192, 0, 0, 255);
            }
            exclamationButton.gameObject.SetActive(false);
        }
    }

    public void OnClickExclamationButton()
    {
        exclamationButton.gameObject.SetActive(false);
        FFF_GameManager.Instance.Flow = 1;
        // TODO : NPC ??ȭ ???;???.  25002?? 4?????? ???? ?? 2?? ?? StartDanceMode(); ????
#if ?????ʾ??׽?Ʈ
        FFF_GameManager.Instance.StartDance();
#endif
    }    
}
