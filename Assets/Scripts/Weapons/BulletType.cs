using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BulletType : MonoBehaviour
{
    [SerializeField] public int speed;
    public bool colpito;

    //Se arpione ? vero ? un arpione senn? ? una satchel.
    [HideInInspector] public bool arpione;

    private Muzzle m;

    //ROBA PER LA SATCHEL
    public float raggio_satchel;
    //Gli oggetti che verranno influenzati dalla granata.
    public LayerMask gio_formaggio;
    //Il danno che far? la satchel.
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
                gameObject.transform.rotation = gameObject.transform.rotation;
            }
        }
        //Se non ? un arpione sar? una satchel e se ? attaccata dopo un tot esplode.
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

    //Per visualizzare la grandezza dell'esplosione che avr? la satchel.
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

    //Se clicca il pulsante e la satchel ? attaccata.
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

    //USARE I LAYER NON I TAGZS
    private void OnTriggerEnter(Collider other)
    {
        if (arpione == true)
        {
            colpito = true;

            //In teoria funziona ma non si sa se funziona.
            gameObject.transform.Translate(Vector3.forward * 0 * Time.deltaTime);

            if (other.CompareTag("Player"))
            {
                arpione = false;

                m.harpoon = true;

                Destroy(gameObject);
            }
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        //Togliere questo bool se non sono entrambi trigger
        if(!arpione)
        {
            if (!other.transform.CompareTag("Player"))
            {
                colpito = true;

                transform.SetParent(other.transform);

                Rigidbody rb = GetComponent<Rigidbody>();

                rb.constraints = RigidbodyConstraints.FreezeAll;
            }
            else if (other.transform.CompareTag("Player") && colpito)
            {
                Destroy(gameObject);
            }
        }
    }
}
