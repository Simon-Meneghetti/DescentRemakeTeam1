using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBox : MonoBehaviour
{
    [Tooltip("Door prefab")] public GameObject[] doorToOpen;
    //tutti gli script OpenDoor delle porte inserite in questo array, verranno attivati, di conseguenza, le porte in questione si apriranno
    
    public void Update()
    {
        foreach (GameObject door in doorToOpen)
        {
            door.GetComponentInChildren<OpenDoor>().enabled = true;
        }
    }
}
