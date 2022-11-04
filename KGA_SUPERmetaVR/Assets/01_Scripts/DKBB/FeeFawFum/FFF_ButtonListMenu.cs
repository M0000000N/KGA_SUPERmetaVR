using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFF_ButtonListMenu : MonoBehaviour
{
    [SerializeField] FFF_ButtonList[] buttonList;
    private int index;

    private void Start()
    {
        buttonList[0].gameObject.SetActive(true);
        for (int i = 1; i < buttonList.Length; i++)
        {
            buttonList[i].gameObject.SetActive(false);
        }
        index = 0;
    }

    private void Update()
    {
        SetNextButtonList();
    }
    private void SetNextButtonList()
    {
        if (buttonList[index].IsDone)
        {
            buttonList[index].gameObject.SetActive(false);
            if (index == buttonList.Length - 1)
            {
                return;
            }
            else
            {
                index++;
                buttonList[index].gameObject.SetActive(true);
            }
        }
    }
}
