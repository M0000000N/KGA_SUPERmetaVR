using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooSoundManager : OnlyOneSceneSingleton<PeekabooSoundManager>
{
    [SerializeField]
    private AudioSource mainBGM;

    public Recorder recorder;

    private void Start()
    {
        //mainBGM.Play();
    }

}
