using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OggettoRaccoglibile : MonoBehaviour
{
    [HideInInspector] public bool preso;
    public float speed;
    public float startTime;
    Muzzle m;
    private void Start()
    {
        m = FindObjectOfType<Muzzle>();
        
        startTime = 0;
    }

    void Update()
    {
        if (m.want_arpion_back == true)
        {
            if (preso == true)
            {
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, m.gameObject.transform.position, speed * Time.deltaTime);
                gameObject.transform.rotation = m.gameObject.transform.rotation;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Arpione"))
        {
            preso = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(gameObject);
            preso = false;
        }
    }
}
