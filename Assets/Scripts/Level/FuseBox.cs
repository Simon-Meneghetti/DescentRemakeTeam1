using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBox : MonoBehaviour
{
    [Tooltip("Inserisci l'oggetto chiamato Script dei prefab delle porte")] public GameObject[] doorToOpen;
    //tutti gli script OpenDoor delle porte inserite in questo array, verranno attivati, di conseguenza, le porte in questione si apriranno
    
    public void Update()
    {
        foreach (GameObject door in doorToOpen)
        {
            door.SetActive(true);
        }
    }
}
