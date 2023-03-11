using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Muzzle : MonoBehaviour
{
    [SerializeField] public GameObject bulletToSpawn;
    [SerializeField] public GameObject arpioneToSpawn;

    [SerializeField] public float rate;
    [SerializeField] public float coolDown;

    [HideInInspector] public Vector3 spawnPosition;
    [SerializeField] public float stamina;
    [SerializeField] public float maxStamina;

    [HideInInspector] public int numOfWeapon;
    [HideInInspector] public bool arpione;

    //roba input
    private bool want_arpion_back;

    float spawnTimer;

    void Start()
    {
        stamina = maxStamina;
    }

    void Update()
    {
        spawnPosition = gameObject.transform.position;
        spawnTimer += Time.deltaTime;

        /*if (Input.GetButtonDown("Jump") || Input.GetButton("Jump"))
            numOfWeapon = 0;

        else if (Input.GetKeyDown(KeyCode.F))
            numOfWeapon = 1;

        else
            numOfWeapon = 2;
        */
        if (stamina <= 0)
            StartCoroutine(Ricarica());

        WeaponChoice(numOfWeapon);

        if(want_arpion_back)
        {
            //Fai tornare l'arpione indietro
        }
    }
    public void WeaponChoice(int n)
    {
        switch (n)
        {
            case 0:
                if (spawnTimer >= rate && stamina > 0)
                {
                    stamina--;
                    GameObject proiettile_spawnato = Instantiate(bulletToSpawn, spawnPosition, Quaternion.identity);
                    proiettile_spawnato.transform.rotation = gameObject.transform.rotation;
                    spawnTimer = 0;
                    arpione = false;
                }
                break;

            case 1:
                if (spawnTimer >= rate)
                {
                    GameObject proiettile_spawnato = Instantiate(arpioneToSpawn, spawnPosition, Quaternion.identity);
                    proiettile_spawnato.transform.rotation = gameObject.transform.rotation;
                    spawnTimer = 0;
                    arpione = true;
                }
                break;

            default:
                break;
        }
    }

    public void Raccolta_Input_SparaArpione(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
            numOfWeapon = 1;
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
