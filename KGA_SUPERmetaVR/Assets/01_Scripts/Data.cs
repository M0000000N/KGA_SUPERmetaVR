using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{    
    public string Nickname { get; set; }
    public string Coin { get; set; }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
