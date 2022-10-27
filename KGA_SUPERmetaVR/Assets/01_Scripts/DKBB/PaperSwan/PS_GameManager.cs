using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_GameManager : OnlyOneSceneSingleton<PS_GameManager>
{
    public bool IsCoolTime;
    // Start is called before the first frame update
    void Start()
    {
        IsCoolTime = false;
    }
    public void OnTriggerExit(Collider other)
    {
        other.gameObject.SetActive(false);
    }
}
