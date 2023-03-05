using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arpione : MonoBehaviour
{
    [SerializeField] public int speed;
    public bool colpito;
    public bool riprendi;
    OggettoRaccoglibile or;
    private void Start()
    {
        or = FindObjectOfType<OggettoRaccoglibile>();
    }

    void Update()
    {
        if (colpito == false)
        {
            gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else if (riprendi == true && or != null)
        {
            gameObject.transform.Translate(Vector3.forward * -speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.R))
        {
            riprendi = true;
        }
        else
        {
            riprendi = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        colpito = true;
        gameObject.transform.Translate(Vector3.forward * 0 * Time.deltaTime);

        if (collision.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
    
}
