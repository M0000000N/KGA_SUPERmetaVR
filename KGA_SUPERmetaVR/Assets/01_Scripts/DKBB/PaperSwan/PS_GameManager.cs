using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_GameManager : OnlyOneSceneSingleton<PS_GameManager>
{
    public bool IsCoolTime;
    void Start()
    {
        IsCoolTime = false;
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PaperSwan")
        {
            other.gameObject.SetActive(false);
        }
    }
}
