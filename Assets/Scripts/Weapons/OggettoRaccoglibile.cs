using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OggettoRaccoglibile : MonoBehaviour
{
    [HideInInspector] public bool preso;
    public float startTime;
    Muzzle m;
    private void Start()
    {
        m = FindObjectOfType<Muzzle>();
        startTime = 0;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            if (preso == true)
            {
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, m.gameObject.transform.position, Time.deltaTime - startTime);
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
}
