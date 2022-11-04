using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FFF_ButtonList : MonoBehaviour
{
    [SerializeField] Button[] button;
    private int clickCount;
    public bool IsDone;

    private void Start()
    {
        clickCount = 0;
        IsDone = false;
    }

    public void OnClickButton()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;
        clickCount++;
        if (clickCount == button.Length)
        {
            IsDone = true;
        }
    }
}
