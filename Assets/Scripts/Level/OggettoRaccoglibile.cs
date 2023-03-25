using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OggettoRaccoglibile : MonoBehaviour
{
    [HideInInspector] public bool preso;
    public float speed;
    public float startTime;
    public float forzaTrainante;
    Quaternion actualRot;
    Muzzle m;
    Rigidbody rb;
    private void Start()
    {
        m = FindObjectOfType<Muzzle>();
        rb = GetComponent<Rigidbody>();
        startTime = 0;
    }

    void Update()
    {
        actualRot = rb.transform.rotation;
        if (m.want_arpion_back == true)
        {
            if (preso == true)
            {
                Vector3 direction = m.gameObject.transform.position - gameObject.transform.position;
                rb.AddForce(direction * Time.deltaTime * forzaTrainante * 50, ForceMode.Impulse);
                //gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, m.gameObject.transform.position, speed * Time.deltaTime);
                //rb.transform.rotation = m.gameObject.transform.rotation;
                preso = false;
            }
        }
    }
    private void OnCollisionExit(Collision coll)
    {
        if (coll.collider.CompareTag("Arpione"))
        {
            preso = true;
        }
    }
    //private void OnCollisionEnter(Collision coll)
    //{
    //    if(coll.collider.CompareTag("Player"))
    //    {
    //        Destroy(gameObject);
    //        preso = false;
    //    }
    //}
}
