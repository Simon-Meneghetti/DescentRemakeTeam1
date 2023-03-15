using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxShield, maxO2;
    public float shield, oxigen;

    public Transform spawnPos;

    Muzzle m;

    void Start()
    {
        m = FindObjectOfType<Muzzle>();
        shield = maxShield;
        oxigen = maxO2;
    }


    void Update()
    {
        if (oxigen > 0)
        {
            oxigen -= (1/(shield / 100) -0.2f) * Time.deltaTime;
            if (oxigen <= 0)
            {
                Death();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("O2Recharge") && oxigen < maxO2)
        {
            if (oxigen < maxO2)
            {
                oxigen += 2f * Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CollectibleSatchel"))
        {
            Destroy(other.gameObject);
            m.satchelCounter++;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("EnemyBullet"))       //Ricorda di mettere il tag ai bullet dell'enemy
        {
            if (shield > 0)
            {
                shield--;
            }
        }
    }
    void Death()
    {
        gameObject.transform.position = spawnPos.position;
        shield = maxShield;
        oxigen = maxO2;
    }
}
