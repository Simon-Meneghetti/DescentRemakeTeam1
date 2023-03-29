using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip Ambient;
    public AudioClip PlayerMoving;
    public AudioClip Enemie1;
    public AudioClip Enemie2;
    public AudioClip Enemie3;

    /////////////////////////////////////
    
    public static AudioManager instance;

    void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    /////////////////////////////////////
    // Start is called before the first frame update
    void Start()
    {
        PlayAudio(Camera.main.GetComponent<AudioSource>(), Ambient);
    }

    public void PlayAudio(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

}
