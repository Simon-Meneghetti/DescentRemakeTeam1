using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OpenDoor : MonoBehaviour
{
    [Tooltip("Anta sinistra")] public GameObject LeftDoor;
    [Tooltip("Anta destra")] public GameObject RightDoor;
    [Tooltip("Spazio percorso dalle porte")] public float apertura;
    [Tooltip("Tempo di apertura porte")] public float timeApertura;
    //questo script va sul gameObject chiamato Script all'interno di Door che di default � disattivato
    //quando la fusebox lo attiver�, la porta di sinistra andr� a sinistra in quattro secondi, mentre la destra andr� a destra e non sar� pi� possibile aprire/chiudere le porte in questione
    void Start()
    {
        LeftDoor.transform.DOMove(LeftDoor.transform.position -transform.right * apertura, timeApertura);
        RightDoor.transform.DOMove(RightDoor.transform.position + transform.right * apertura, timeApertura);

        AudioManager.instance.PlayAudio(GetComponent<AudioSource>(), AudioManager.instance.Porta);
    }
}
