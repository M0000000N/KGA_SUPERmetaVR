using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pumpkin : MonoBehaviour
{
    private AudioSource myAudio;

    private void Awake()
    {
        myAudio = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        myAudio.Stop();
        myAudio.Play();
    }
}