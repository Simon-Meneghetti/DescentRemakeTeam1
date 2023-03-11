using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletType : MonoBehaviour
{
    [SerializeField] public int speed;
    [HideInInspector] public bool colpito;
    [HideInInspector] public bool arpione;
    public float startTime;

    private void Start()
    {
        startTime = 0;
    }
    void Update()
    {
        //Arpione
        if (arpione == true)
        {
            if (colpito == false)
                gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);

            else if (want_arpion_back == true)
            {
                var posizione_giocatore = GameObject.FindObjectOfType<Player_Movement>().transform.position;

                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, posizione_giocatore, Time.deltaTime - startTime);
                gameObject.transform.rotation = gameObject.transform.rotation;
            }
        }
        //SATCHEL... sempre per te Ale XD... te lo gestisci come vuoi tu...GRAZIE ANCORA EGREGIA CAPO PROGRAMMATRICE MRS BEATRICE SIPOS, PER GLI AMICI... "POS".
        /*else
        {

        }
        */
        








        

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (m.arpione == true)
        {
            colpito = true;
            gameObject.transform.Translate(Vector3.forward * 0 * Time.deltaTime);

            if (collision.collider.CompareTag("Player"))
            {
                Destroy(gameObject);
                m.arpione = false;
            }
        }

        //SATCHEL... suonerò ripetitiva, ma è così che funziona...
        //else





    }
}
