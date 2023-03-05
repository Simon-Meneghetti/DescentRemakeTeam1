using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muzzle : MonoBehaviour
{
    [SerializeField] public GameObject bulletToSpawn;

    [SerializeField] public float rate;
    [SerializeField] public float coolDown;

    [HideInInspector] public Vector3 spawnPosition;
    [SerializeField] public float stamina;
    [SerializeField] public float maxStamina;

    float spawnTimer;

    void Start()
    {
        stamina = maxStamina;
    }

    void Update()
    {
        spawnPosition = gameObject.transform.position;
        spawnTimer += Time.deltaTime;

        //tasto temporaneo... qui servi tu, Ale

        if (Input.GetButtonDown("Jump") || Input.GetButton("Jump"))
        {
            if (spawnTimer >= rate && stamina > 0)
            {
                stamina--;
                GameObject proiettile_spawnato = Instantiate(bulletToSpawn, spawnPosition, Quaternion.identity);
                proiettile_spawnato.transform.rotation = gameObject.transform.rotation;
                spawnTimer = 0;
            }
        }
        

        if (stamina <= 0)
        {
            StartCoroutine(Ricarica());
        }
    }
    IEnumerator Ricarica()
    {
        yield return new WaitForSeconds(coolDown);
        stamina = maxStamina;
    }
}
