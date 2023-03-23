using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OpenDoor : MonoBehaviour
{
    public GameObject LeftDoor;
    public GameObject RightDoor;
   //questo script va sul gameObject chiamato Script all'interno di Door che di default � disattivato
   //quando la fusebox lo attiver�, la porta di sinistra andr� a sinistra in quattro secondi, mentre la destra andr� a destra e non sar� pi� possibile aprire/chiudere le porte in questione
    void Start()
    {
            LeftDoor.transform.DOMove(LeftDoor.transform.position + Vector3.left * 2, 4);
            RightDoor.transform.DOMove(RightDoor.transform.position + Vector3.right * 2, 4);
    }
}
