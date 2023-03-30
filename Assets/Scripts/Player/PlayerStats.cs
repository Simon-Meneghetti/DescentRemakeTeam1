using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public float maxShield, maxO2;
    public float shield, oxigen;
    public float O2Recharge = 2f;
    //public int numOfKeys;

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
            oxigen -= (1 + (-(shield / 100) + 1f)) * Time.deltaTime;
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
                oxigen += O2Recharge * Time.deltaTime;
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
        
        //if(other.CompareTag("Key"))
        //{
        //    numOfKeys++;
        //    Destroy(other.gameObject);
        //}
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))       //Ricorda di mettere il tag ai bullet dell'enemy
        {
            if (shield > 0)
            {
                //Chiedere ad i designer quanto danno fa.
                shield -= collision.transform.GetComponent<Nemico>().damage;
                //ShakeEffect
                //Mettere personalizzabili dal gamemanager l'effetto
                transform.DOShakeRotation(0.3f, 45);
            }
        }
    }
    void Death()
    {
        UIManager.instance.GameLost();
    }
}
