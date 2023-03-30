using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Arpione : MonoBehaviour
{
    [SerializeField] public int speed;
    [HideInInspector] public bool colpito;

    private Muzzle m;

    //ROBA PER LA SATCHEL
    public float raggio_satchel;
    //Gli oggetti che verranno influenzati dalla granata.
    public LayerMask gio_formaggio;
    //Il danno che farà la satchel.
    public float quantita_danno;
    //Quando attaccata, dopo un po' esplode.
    public float durata_satchel;

    UIManager UM;

    private void Start()
    {
        m = FindObjectOfType<Muzzle>();
        UM = FindObjectOfType<UIManager>();

        AudioManager.instance.PlayAudio(GetComponent<AudioSource>(), AudioManager.instance.Arpione);
    }

    void Update()
    {
        //Arpione
        if (m.want_arpion_back == true && colpito)
        {
            GetComponent<AudioSource>().Play();
            var posizione_giocatore = GameObject.FindObjectOfType<Player_Movement>().transform.position;

            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, posizione_giocatore, speed * Time.deltaTime);

            gameObject.transform.rotation = m.gameObject.transform.rotation; //?
        }


        //SATCHEL... sempre per te Ale XD... te lo gestisci come vuoi tu...GRAZIE ANCORA EGREGIA CAPO PROGRAMMATRICE MRS BEATRICE SIPOS, PER GLI AMICI... "POS".

        if (colpito == false)
            gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision coll)
    {
        //Ferma l'audio dell'arpione
        GetComponent<AudioSource>().Pause();

        colpito = true;

        //In teoria funziona ma non si sa se funziona.
        gameObject.transform.Translate(Vector3.forward * 0 * Time.deltaTime);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        if (coll.collider.CompareTag("Player"))
        {

            m.harpoon = true;

            UM.ArpionePronto.SetActive(true);
            Destroy(gameObject);
        }
    }

}
