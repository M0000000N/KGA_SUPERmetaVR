using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string SoundName;
    public AudioClip Clip;
}

public class SoundManager : SingletonBehaviour<SoundManager>
{
    [Header("사운드 리소스")]
    [SerializeField] Sound[] bgmSoundList;
    [SerializeField] Sound[] seSoundList;

    [Header("오디오 소스")]
    [SerializeField] AudioSource bgmPlayer;
    public float BGMValue { get { return bgmPlayer.volume; } }
    [SerializeField] AudioSource[] sePlayer;
    public float SEValue { get { return sePlayer[0].volume; } }

    //[SerializeField] AudioSource micPlayer;
    //public float MICValue { get { return micPlayer.volume; } }

    //[SerializeField] AudioSource voicePlayer;
    //public float VoiceValue { get { return voicePlayer.volume; } }


    void Start()
    {
        AudioSource[] audioPlayer = GetComponents<AudioSource>();
        sePlayer = new AudioSource[audioPlayer.Length - 1];

        bgmPlayer = audioPlayer[0]; // 가장 위는 BGM
        for (int i = 1; i < audioPlayer.Length; i++) // 나머지는 SE
        {
            sePlayer[i-1] = audioPlayer[i];
            if (sePlayer.Length < i) break;
        }

        // TODO : 추후 VoiceSound가 필요하면 리스트를 수정할 계획입니다.
    }

    public void PlayBGM(string _soundName)
    {
        for (int i = 0; i < bgmSoundList.Length; i++)
        {
            if (_soundName == bgmSoundList[i].SoundName)
            {
                if (bgmPlayer.clip == bgmSoundList[i].Clip) return;
                bgmPlayer.clip = bgmSoundList[i].Clip;
                bgmPlayer.Play();
                return;
            }
        }
    }


    public void PlaySE(string _soundName)
    {
        for (int i = 0; i < seSoundList.Length; i++)
        {
            if (_soundName == seSoundList[i].SoundName)
            {
                for (int x = 0; x < seSoundList.Length; x++)
                {
                    if (sePlayer[x].isPlaying == false)
                    {
                        sePlayer[x].clip = seSoundList[i].Clip;
                        sePlayer[x].Play();
                        return;
                    }
                }
                return;
            }
        }
    }

    public void SetBGMVolume(float _volume)
    {
        bgmPlayer.volume = _volume;
    }

    public void SetSEVolume(float _volume)
    {
        for (int i = 0; i < sePlayer.Length; i++)
        {
            sePlayer[i].volume = _volume;
        }
    }
}
