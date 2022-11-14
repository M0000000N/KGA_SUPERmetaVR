using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooSoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource mainBGM;
    private void Start()
    {
        mainBGM.Play();
    }
}
