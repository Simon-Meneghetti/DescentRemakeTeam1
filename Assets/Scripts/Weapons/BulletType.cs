using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletType : MonoBehaviour
{
    [SerializeField] public int speed;
    [HideInInspector] public bool colpito;
    public float startTime;
    Muzzle m;

    private void Start()
    {
        m = FindObjectOfType<Muzzle>();
        startTime = 0;
    }
    void Update()
    {
        //Arpione
        if (m.arpione == true)
        {
            if (colpito == false)
                gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);

            else if (m.want_arpion_back == true)
            {
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, m.gameObject.transform.position, Time.deltaTime - startTime);
                gameObject.transform.rotation = m.gameObject.transform.rotation;
            }
        }


        //SATCHEL... sempre per te Ale XD... te lo gestisci come vuoi tu

        //else if (m.arpione == false)







        

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
