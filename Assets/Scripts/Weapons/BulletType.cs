using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletType : MonoBehaviour
{
    [SerializeField] public int speed;
    [HideInInspector] public bool colpito;
    [HideInInspector] public bool riprendi;
    public float startTime;
    Muzzle m;
    //[SerializeField] public int damage; --> questo va messo sull'enemy/player

    private void Start()
    {
        m = FindObjectOfType<Muzzle>();
        startTime = 0;
    }
    void Update()
    {
        if (m.arpione == true)
        {
            if (colpito == false)
                gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);

            else if (riprendi == true)
            {
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, m.gameObject.transform.position, Time.deltaTime - startTime);
                gameObject.transform.rotation = m.gameObject.transform.rotation;
            }

            if (Input.GetKey(KeyCode.R))
                riprendi = true;

            else
                riprendi = false;
        }

        else if (m.arpione == false)
            gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);

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

        else
            Destroy(gameObject);
    }
}
