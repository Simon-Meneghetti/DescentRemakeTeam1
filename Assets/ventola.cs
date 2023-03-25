using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ventola : MonoBehaviour
{
    [SerializeField] private float distanza;
    [SerializeField] private float wind_force;
    [SerializeField] private LayerMask oggettiVentabili;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Physics.SphereCast(transform.position - transform.right * 2, transform.localScale.y / 2, transform.right, out RaycastHit hit1, distanza, oggettiVentabili))
        {
            Vector3 direzione = ((transform.position - transform.right) - hit1.transform.position).normalized;

            //hit1.rigidbody.AddForce(direzione * wind_force * Time.deltaTime * 50, ForceMode.Acceleration);

            hit1.rigidbody.AddForce(direzione * wind_force * Time.deltaTime * 100, ForceMode.Acceleration);
        }
    }

    private void OnDrawGizmos()
    {
        if(Physics.SphereCast(transform.position - transform.right * 2, transform.localScale.y / 2, transform.right, out RaycastHit hit, distanza))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position - transform.right * 2, transform.right * hit.distance);
            Gizmos.DrawWireSphere(transform.position + transform.right * hit.distance, transform.localScale.y / 2);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position - transform.right * 2, transform.right * distanza);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
        else if (other.transform.CompareTag("Player"))
        {
            other.transform.GetComponent<PlayerStats>().shield = 0;
        }
        else if (other.transform.CompareTag("OggettoArpionabile"))
        {
            Destroy(gameObject);
        }
    }
}
