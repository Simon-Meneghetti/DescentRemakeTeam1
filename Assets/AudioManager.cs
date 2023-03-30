using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Ambient & Player")]
    public AudioClip Ambient;
    public AudioClip RandomAmbient;
    [SerializeField, Tooltip("NON TOCCARE UAGLIU'")] private AudioSource RandomAudioSource;
    [Tooltip("Il minimo di tempo che deve passare tra un audio e l'altro")]
    public float RandomTimerMin;
    [Tooltip("Il massimo di tempo che deve passare tra un audio e l'altro")]
    public float RandomTimerMax;
    public AudioClip PlayerMoving;
    public AudioClip Porta;
    public AudioClip Ossigeno;
    [Header("Armi")]
    public AudioClip Taser;
    public AudioClip Arpione;
    public AudioClip Esplosione;
    [Header("Nemici")]
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
        StartCoroutine(PlayRandom());
    }

    public void PlayAudio(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    IEnumerator PlayRandom()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(RandomTimerMin, RandomTimerMax));
            PlayAudio(RandomAudioSource, RandomAmbient);
        }
    }

}
