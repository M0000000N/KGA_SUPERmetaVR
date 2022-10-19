using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Stamina : MonoBehaviour
{
   
    [SerializeField]
    private Color enabledColor;
    [SerializeField]
    private Color disabledColor; 

    [SerializeField]
    private bool StartFull = false;

    [SerializeField]
    private int maxValue;

    [SerializeField]
    private int minValue;

    [SerializeField]
    private int currentValue;

    [SerializeField]
    private float clickButtonB = 0.2f;

    [SerializeField]
    private float cancleButtonB = 3f; 


    [SerializeField]
    private PlayMove_Photon playerPhoton;

    private float decreaseTime = 0f;
    private float increaseTime = 0f;

    List<Image> progressSteps;

    private void Start()
    {
        maxValue = transform.childCount;

        progressSteps = new List<Image>(); 

        for(int i = 0; i < maxValue; ++i)
        {
            progressSteps.Add(transform.GetChild(i).GetComponent<Image>());
        }
        InititateProgressBar(StartFull); 
    }

    void changeSpriteColor(int index, Color newcolor) // �Լ� ���� ��Ȯ�ϰ� ���� 
    {
        progressSteps[index].GetComponent<Image>().color = newcolor;
    }
    public void InititateProgressBar(bool isFull) // Fill or Reset
    {
        if(isFull)
        {       
            for(int i = 0; i < maxValue; ++i)
            {
                changeSpriteColor(i, enabledColor); 
            }
            currentValue = maxValue;
        }
        else
        {
            for(int i =0; i < maxValue; i++)
            {
                changeSpriteColor(i, disabledColor);
            }
            currentValue = 0; 
        }
    }

    public void IncreaseProgress()
    {
        if (currentValue == maxValue)
            return;
        else
        {
            increaseTime += Time.deltaTime;
            if (increaseTime >= cancleButtonB) // �ð��� ������ �޾Ƽ� ���� // 0 ~ 8���� 
            {
                currentValue++;
                changeSpriteColor(currentValue - 1, enabledColor);
                increaseTime = 0f;
            }          
        }
    }

    public void DecreaseProgress()
    {     
        if (currentValue == minValue)
            return;
        else
        {
            decreaseTime += Time.deltaTime;
            if (decreaseTime > clickButtonB)
            {
                changeSpriteColor(currentValue - 1, disabledColor);
                currentValue--;
                decreaseTime = 0f;
            }
        }
    }

    public int GetProgress()
    {
        return currentValue; 
    }

}