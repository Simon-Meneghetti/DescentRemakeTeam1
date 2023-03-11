using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Runtime.InteropServices.ComTypes;

public class Muzzle : MonoBehaviour
{
    [SerializeField] public GameObject arpioneToSpawn;
    [SerializeField] public GameObject satchelToSpawn;

    [SerializeField] public float rate;
    [SerializeField] public float coolDown;

    [HideInInspector] public Vector3 spawnPosition;
    [SerializeField] public float stamina;
    [SerializeField] public float maxStamina;

    float spawnTimer;

    //roba input
    public bool want_arpion_back;

    void Start()
    {
        stamina = maxStamina;
    }

    void Update()
    {
        spawnPosition = gameObject.transform.position;
        spawnTimer += Time.deltaTime;

        if (stamina <= 0)
            StartCoroutine(Ricarica());
    }

    public void Raccolta_Input_SparaArpione(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && spawnTimer >= rate)
        {
            GameObject proiettile_spawnato = Instantiate(arpioneToSpawn, spawnPosition, Quaternion.identity);

            proiettile_spawnato.transform.rotation = gameObject.transform.rotation;

            spawnTimer = 0;
            proiettile_spawnato.GetComponent<BulletType>().arpione = true;
        }
    }

    public void Raccolta_Input_SparaSatchel(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && spawnTimer >= rate)
        {
            stamina--;

            //istanzia la carica satchel e sistema la rotazione
            GameObject proiettile_spawnato = Instantiate(satchelToSpawn, spawnPosition, Quaternion.identity);

            proiettile_spawnato.transform.rotation = gameObject.transform.rotation;

            spawnTimer = 0;
        }
    }

    public void Raccolta_Input_RipresaArpione(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
            want_arpion_back= true;
        else if(ctx.canceled)
            want_arpion_back= false;
    }

    IEnumerator Ricarica()
    {
        yield return new WaitForSeconds(coolDown);
        stamina = maxStamina;
    }
}
