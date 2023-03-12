using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BulletType : MonoBehaviour
{
    [SerializeField] public int speed;
    public bool colpito;

    //Se arpione è vero è un arpione sennò è una satchel.
    [HideInInspector] public bool arpione;

    private Muzzle m;

    //Velocità arpione, da vedere.
    public float startTime;

    //ROBA PER LA SATCHEL
    public float raggio_satchel;
    //Gli oggetti che verranno influenzati dalla granata.
    public LayerMask gio_formaggio;
    //Il danno che farà la satchel.
    public float quantita_danno;
    //Quando attaccata, dopo un pò esplode.
    public float durata_sachel;



    private void Start()
    {
        startTime = 0;
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

                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, posizione_giocatore, Time.deltaTime - startTime);
                gameObject.transform.rotation = gameObject.transform.rotation;
            }
        }
        //Se non è un arpione sarà una satchel e se è attaccata dopo un tot esplode.
        else if(!arpione && colpito) 
        {
            if (durata_sachel <= 0)
                EsplosioneSatchel();
            else if(durata_sachel > 0)
                durata_sachel -= Time.deltaTime;
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


    private void OnCollisionEnter(Collision collision)
    {
        if (arpione == true)
        {
            colpito = true;

            //In teoria funziona ma non si sa se funziona.
            gameObject.transform.Translate(Vector3.forward * 0 * Time.deltaTime);

            if (collision.collider.CompareTag("Player"))
            {
                arpione = false;

                m.harpoon = true;

                Destroy(gameObject);
            }
        }
        else
        {
            if (!collision.transform.CompareTag("Player"))
            {
                colpito = true;

                transform.SetParent(collision.transform);

                Rigidbody rb = GetComponent<Rigidbody>();

                rb.constraints = RigidbodyConstraints.FreezeAll;
            }
            else if(collision.transform.CompareTag("Player") && colpito)
            {
                Destroy(gameObject);
            }
        }
    }
}
