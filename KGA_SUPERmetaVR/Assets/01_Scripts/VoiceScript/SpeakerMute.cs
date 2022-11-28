using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Voice.Unity;

public class SpeakerMute : MonoBehaviour
{ 
    [SerializeField] Toggle SpeakActivate;
    [SerializeField] Recorder recorder; 

    [SerializeField] Speaker speaker;
    // [SerializeField] AudioSource audioSource;
    bool mute;

    //public void Start()
    //{
    //    SpeakActivate.onValueChanged.AddListener(MuteSpeaker); 
    //}
    

    //public void MuteSpeaker()
    //{
    //    AudioSource audioSource = speaker.GetComponent<AudioSource>();
    //    if (mute)
    //        audioSource.mute = mute;
    //    else
    //        audioSource.mute = !mute; 
    //    //if (recorder.Equals(null))
    //    //    return; 

    //    //if(mute)
    //    //{
    //    //    recorder.IsRecording = !mute;
    //    //}
    //    //else
    //    //{
    //    //    recorder.IsRecording = mute;
    //    //}

    //    //if (audioSource.Equals(null))
    //    //    return;

    //    //if (mute)
    //    //{
    //    //    audioSource.mute = true;
    //    //    audioSource.volume = 0;
    //    //}
    //    //else
    //    //{
    //    //    audioSource.mute = false;
    //    //}

    //}

}
