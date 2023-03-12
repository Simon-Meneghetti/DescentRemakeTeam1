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

    //Roba input
    public bool want_arpion_back;
    [HideInInspector] public bool harpoon;
    [HideInInspector] public bool attached;

    //Sachel
    private BulletType satchel;

    void Start()
    {
        stamina = maxStamina;
        harpoon = true;
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
        if (ctx.performed && harpoon)
        {
            GameObject proiettile_spawnato = Instantiate(arpioneToSpawn, spawnPosition, Quaternion.identity);

            proiettile_spawnato.transform.rotation = gameObject.transform.rotation;

            spawnTimer = 0;

            proiettile_spawnato.GetComponent<BulletType>().arpione = true;

            harpoon= false;
        }
    }

    public void Raccolta_Input_SparaSatchel(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && satchel == null)
        {
            stamina--;

            //istanzia la carica satchel e sistema la rotazione
            satchel = Instantiate(satchelToSpawn, spawnPosition, Quaternion.identity).GetComponent<BulletType>();

            satchel.transform.rotation = gameObject.transform.rotation;

            spawnTimer = 0;
        }
        else if(ctx.performed && satchel != null && ctx.performed && satchel.colpito)
        {
            satchel.EsplosioneSatchel();
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
