using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }
    
    public SoundType[] sounds;

    public AudioSource soundEffect;

    public bool IsMute = false;
    public float Volume = 1f;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Mute(bool status)
    {
        IsMute = status;
    }

    public void Play(Sounds sound)
    {
        if (IsMute)
            return;

        AudioClip clip = getSoundClip(sound);
        if (clip != null)
        {
            soundEffect.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("Clip not found for sound type : " + sound);
        }        
    }

    private AudioClip getSoundClip(Sounds sound)
    {
        SoundType returnSound = Array.Find(sounds, item => item.soundType == sound);
        return returnSound.soundClip;
    }
}

[Serializable]
public class SoundType
{
    public Sounds soundType;
    public AudioClip soundClip;
}
public enum Sounds
{
    ButtonClick,
    ButtonOver,
    SnakeMove,
    SnakeEat,
    SnakeDie
}
