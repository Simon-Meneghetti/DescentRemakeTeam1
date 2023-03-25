using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBox : MonoBehaviour
{
    //tutti gli script OpenDoor delle porte inserite in questo array, verranno attivati, di conseguenza, le porte in questione si apriranno
    [Tooltip("Door prefab")] public GameObject[] doorToOpen;
    [Tooltip("Lights prefab")] public GameObject[] lightsToTurnOn;
    [Tooltip("Enemies prefab")] public GameObject[] enemiesToSpawn;
    public void Update()
    {
        foreach (GameObject door in doorToOpen)
        {
            door.gameObject.SetActive(true);
            OpenDoor openDoor = door.GetComponentInChildren<OpenDoor>();
            if (openDoor != null)
            {
                openDoor.enabled = true;
            }
        }
        foreach (GameObject light in lightsToTurnOn)
        {
            light.gameObject.SetActive(true);
        }
        foreach (GameObject enemy in enemiesToSpawn)
        {
            enemy.gameObject.SetActive(true);
        }
    }
}
