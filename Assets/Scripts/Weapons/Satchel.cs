using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satchel : MonoBehaviour
{
    [HideInInspector] public float shootForce;

    //Se la satchel è attaccata
    [HideInInspector] public bool attaccata;
    [HideInInspector] public bool boom;

    //ROBA PER LA SATCHEL
    public float raggio_satchel;
    //Il danno che farà la satchel.
    public float quantita_danno;
    //Quando attaccata, dopo un po' esplode.
    public float durata_satchel;
    //Gli oggetti che verranno influenzati dalla granata.
    public LayerMask contatto;


    private Rigidbody rb;

    void Start()
    {
        rb= GetComponent<Rigidbody>();

        rb.AddRelativeForce(transform.forward * shootForce * 500 * Time.deltaTime, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (attaccata)
        {
            if (durata_satchel <= 0)
                EsplosioneSatchel();
            else if (durata_satchel > 0)
                durata_satchel -= Time.deltaTime;
        }
        if (boom && !GetComponent<AudioSource>().isPlaying)
            Destroy(gameObject);
    }

    //Per visualizzare la grandezza dell'esplosione che avrà la satchel.
    private void OnDrawGizmos()
    {
        if (!Physics.CheckSphere(transform.position, raggio_satchel, contatto))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, raggio_satchel);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, raggio_satchel);
        }
    }

    //Se clicca il pulsante e la satchel è attaccata.
    public void EsplosioneSatchel()
    {
        boom = true;

        AudioManager.instance.PlayAudio(GetComponent<AudioSource>(), AudioManager.instance.Esplosione);

        Collider[] colliders = Physics.OverlapSphere(transform.position, raggio_satchel, contatto);

        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<PlayerStats>() != null)
            {
                collider.GetComponent<PlayerStats>().shield -= quantita_danno;
            }
            else if(collider.GetComponent<Nemico>() != null)
            {
                collider.GetComponent<Nemico>().vita -= quantita_danno;
            }
        }

        GetComponentInChildren<MeshRenderer>().forceRenderingOff = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        //Togliere questo bool se non sono entrambi trigger
        //Se non è il giocatore (Siccome non si può incollare al giocatore)
        if (!other.transform.CompareTag("Player") && !attaccata)
        {
            //Ha colpito qualcosa
            attaccata = true;

            //Prendiamo la scala originale
            Vector3 sizeAppoggio = transform.localScale;
            Vector3 rotationAppoggio = transform.localEulerAngles;
            //Ne è diventato figlio (seguirà questa cosa)
            transform.SetParent(other.transform);
            //Resettiamo la scale
            transform.localScale = new Vector3(sizeAppoggio.x / transform.parent.localScale.x, sizeAppoggio.y / transform.parent.localScale.y, sizeAppoggio.z / transform.parent.localScale.z);
            //Se possibile sarebbe meglio trovare una rotazione a seconda del parent a cui si attacca
            transform.localRotation = Quaternion.identity;


            //Non verrà più modificata la sua rotazione e/o posizione
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.velocity = Vector3.zero;
        }
        else if (other.transform.CompareTag("Player") && attaccata)
        {
            Destroy(gameObject);
        }
    }

}
