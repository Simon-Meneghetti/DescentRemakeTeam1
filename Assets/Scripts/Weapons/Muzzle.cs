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

    [HideInInspector] public float spawnTimer;
    [HideInInspector] public float rechargeTimer;

    //Roba input
    [HideInInspector] public bool want_arpion_back;
    [HideInInspector] public bool harpoon;
    [HideInInspector] public bool attached;
    [HideInInspector] public bool taser;
    [HideInInspector] public bool ricarica;

    //Sachel
    public int satchelCounter;
    private BulletType satchel;

    void Start()
    {
        satchelCounter = 1;
        stamina = maxStamina;
        harpoon = true;
    }

    void Update()
    {
        spawnPosition = gameObject.transform.position;
        spawnTimer += Time.deltaTime;

        if (stamina <= 0)
        {
            stamina = 0;
            ricarica = true;
        }
        if(ricarica == true)
        {
            rechargeTimer += Time.deltaTime;
            if(rechargeTimer>=coolDown)
            {
                stamina = maxStamina;
                ricarica = false;
            }
        }
        else
        {
            rechargeTimer = 0;
        }
    }


    public void Raccolta_Input_SparaArpione(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && harpoon && taser == false)
        {
            GameObject proiettile_spawnato = Instantiate(arpioneToSpawn, spawnPosition, Quaternion.identity);

            proiettile_spawnato.transform.rotation = gameObject.transform.rotation;

            spawnTimer = 0;

            proiettile_spawnato.GetComponent<BulletType>().arpione = true;

            harpoon = false;
        }
    }

    public void Raccolta_Input_SparaSatchel(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && satchel == null && satchelCounter>0)
        {
            //istanzia la carica satchel e sistema la rotazione
            satchel = Instantiate(satchelToSpawn, spawnPosition, Quaternion.identity).GetComponent<BulletType>();

            satchel.transform.rotation = gameObject.transform.rotation;

            satchelCounter--;

            spawnTimer = 0;
        }
        else if (ctx.performed && satchel != null && ctx.performed && satchel.colpito)
        {
            satchel.EsplosioneSatchel();
        }
    }

    public void Raccolta_Input_RipresaArpione(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            want_arpion_back = true;
        else if (ctx.canceled)
            want_arpion_back = false;
    }

    public void Raccolta_Input_SparaTaser(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (stamina > 0)
            {
                taser = true;
                stamina--;
            }

        }
        else
            taser = false;

    }
}
