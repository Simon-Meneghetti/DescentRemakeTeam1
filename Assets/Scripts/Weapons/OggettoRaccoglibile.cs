using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OggettoRaccoglibile : MonoBehaviour
{
    public GameObject player;
    Arpione ap;
    public bool preso;
    void Start()
    {
        ap = FindObjectOfType<Arpione>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            if (preso == true)
                gameObject.transform.Translate(Vector3.forward * -4 * Time.deltaTime);
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
