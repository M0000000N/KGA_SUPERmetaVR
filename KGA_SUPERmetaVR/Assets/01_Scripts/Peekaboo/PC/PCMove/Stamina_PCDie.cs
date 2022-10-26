using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina_PCDie : MonoBehaviour
{
    [SerializeField]
    private GameObject StaminaUI;

    private void Start()
    {
        StaminaUI.SetActive(true);
    }

    private void Update()
    {
        if (PeekabooGameManager.Instance.IsGameOver)
        {
            Staminabar();
        }
    }

    private void Staminabar()
    {
        StaminaUI.SetActive(false);
    }

}
