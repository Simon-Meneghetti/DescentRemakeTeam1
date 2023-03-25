using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Arpione : MonoBehaviour
{
    [SerializeField] public int speed;
    [HideInInspector] public bool colpito;

    //Se arpione è vero è un arpione sennò è una satchel.
    [HideInInspector] public bool arpione;

    private Muzzle m;

    //ROBA PER LA SATCHEL
    public float raggio_satchel;
    //Gli oggetti che verranno influenzati dalla granata.
    public LayerMask gio_formaggio;
    //Il danno che farà la satchel.
    public float quantita_danno;
    //Quando attaccata, dopo un po' esplode.
    public float durata_satchel;



    private void Start()
    {
        m = FindObjectOfType<Muzzle>();
    }

    void Update()
    {
        //Arpione
        if (arpione == true)
        {
            if (m.want_arpion_back == true && colpito)
            {
                var posizione_giocatore = GameObject.FindObjectOfType<Player_Movement>().transform.position;

                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, posizione_giocatore, speed * Time.deltaTime);
                
                gameObject.transform.rotation = m.gameObject.transform.rotation; //?
            }
        }
        //Se non è un arpione sarà una satchel e se è attaccata dopo un tot esplode.
        else if(!arpione && colpito) 
        {
            if (durata_satchel <= 0)
                EsplosioneSatchel();
            else if(durata_satchel > 0)
                durata_satchel -= Time.deltaTime;
        }

        //SATCHEL... sempre per te Ale XD... te lo gestisci come vuoi tu...GRAZIE ANCORA EGREGIA CAPO PROGRAMMATRICE MRS BEATRICE SIPOS, PER GLI AMICI... "POS".

        if (colpito == false)
            gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    //Per visualizzare la grandezza dell'esplosione che avrà la satchel.
    private void OnDrawGizmos()
    {
        if (!Physics.CheckSphere(transform.position, raggio_satchel, gio_formaggio))
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
        Collider[] colliders = Physics.OverlapSphere(transform.position, raggio_satchel, gio_formaggio);
        
        foreach(Collider collider in colliders)
        {
            if (collider.GetComponent<PlayerStats>() != null)
            {
                collider.GetComponent<PlayerStats>().shield -= quantita_danno;
            }
            /*Se colpisce il nemico
            else if(collider.GetComponent<Nemico>() != null
            {
                collider.GetComponent<PlayerStats>().vitanemico -= quantita_danno;
            }
            */
        }

        //Boom!
        Destroy(gameObject);
    }


    private void OnCollisionEnter(Collision coll)
    {
        if (arpione == true)
        {
            colpito = true;

            //In teoria funziona ma non si sa se funziona.
            gameObject.transform.Translate(Vector3.forward * 0 * Time.deltaTime);
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            if (coll.collider.CompareTag("Player"))
            {
                arpione = false;

                m.harpoon = true;

                Destroy(gameObject);
            }
        }
        //Togliere questo bool se non sono entrambi trigger
        if(!arpione)
        {
            //Se non è il giocatore (Siccome non si può incollare al giocatore)
            if (!coll.collider.transform.CompareTag("Player") && !colpito)
            {
                //Ha colpito qualcosa
                colpito = true;

                //Prendiamo la scala originale
                Vector3 sizeAppoggio = transform.localScale;
                Vector3 rotationAppoggio = transform.localEulerAngles;
                //Ne è diventato figlio (seguirà questa cosa)
                transform.SetParent(coll.collider.transform);
                //Resettiamo la scale
                transform.localScale = new Vector3(sizeAppoggio.x / transform.parent.localScale.x, sizeAppoggio.y / transform.parent.localScale.y, sizeAppoggio.z / transform.parent.localScale.z);
                //Se possibile sarebbe meglio trovare una rotazione a seconda del parent a cui si attacca
                transform.localRotation= Quaternion.identity;
                

                //Non verrà più modificata la sua rotazione e/o posizione
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezeAll;
                rb.velocity = Vector3.zero;
            }
            else if (coll.collider.transform.CompareTag("Player") && colpito)
            {
                Destroy(gameObject);
            }
        }
        
    }

    //private void OnCollisionEnter(Collision other)
    //{
    //}
}
