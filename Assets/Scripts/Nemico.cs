using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Nemico : MonoBehaviour
{
    private GameObject player;
    public enum Type { Distanza, Ravvicinato };

    [Header("Enemy Type")]
    public Type enemy_type;

    [Header("Stealth settings")]
    //La distanza del suo cono visivo
    public float visionRange;
    //L'apertura del suo cono visivo
    public float angleRange;
    //La distanza con cui può sentire il giocatore.
    public float hearRange;
    //Ha visto il giocatore?
    public bool playerSpotted;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<Player_Movement>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement(Detection());
        

    }

    void EnemyMovement(bool player_spotted)
    {
        //Se il giocatore è spottato...
        if (playerSpotted)
        {
            
        }
        //Se il giocatore non è spottato...
        else
        {

        }
    }

    //Funzione che serve al nemico per trovare il giocatore.
    bool Detection()
    {
        //Prodotto saclare tra la direzione e il forward del nemico 
        //Posso anche calcolarmi l'angolo compreso tra i due vettori

        //Il vettore che ci da la direzione da questo nemico al player.
        Vector3 directionToPlayer = player.transform.position - transform.position;

        //Se il giocatore non è troppo distante...
        if (Vector3.Distance(transform.position, player.transform.position) <= visionRange || Vector3.Distance(transform.position, player.transform.position) /* - rumore prodotto dal player */ <= hearRange)
        {
            //...e si trova nell'angolo di visione del nemico...
            if (Vector3.Angle(transform.forward, directionToPlayer) <= angleRange || Vector3.Distance(transform.position, player.transform.position) /* - rumore prodotto dal player */ <= hearRange)
            {
                //Il nemico ha una visione sul player.
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    void OnDrawGizmos()
    {

        Quaternion leftRayRotation = Quaternion.AngleAxis(-angleRange, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(angleRange, Vector3.up);

        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;


        if (player != null && Detection())
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, hearRange);
        Gizmos.DrawRay(transform.position, leftRayDirection * visionRange);
        Gizmos.DrawRay(transform.position, rightRayDirection * visionRange);
    }
}
