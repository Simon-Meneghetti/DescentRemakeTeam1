using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
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

    public LayerMask lineOfSight;

    [Header("Stats")]
    public float speed;
    public float damage;
    public float shootRange;
    public float knockback;


    //Componenti nemico che ci servono
    private Rigidbody rb;
    private bool can_chase = true; 


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<Player_Movement>().gameObject;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(can_chase)
            EnemyMovement(Detection());
    }

    void EnemyMovement(bool player_spotted)
    {
        //Se il giocatore è spottato...
        if (player_spotted)
        {
            //Fissa il player in modo inquietante
            transform.LookAt(player.transform.position);

            //Se il nemico è quello a distanza...
            if (enemy_type == Type.Ravvicinato)
            {
                /* Varie opzioni di movimento 
                 * Se uso la posizione trapassa i muri
                    //transform.position = Vector3.Lerp(transform.position, player.transform.position, speed * Time.deltaTime);
                 * Se uso addforce non riesce a raggiungere il player se si muove al suo lato, o almeno lo raggiunge tramite una spirale
                    //rb.AddForce((player.transform.position - transform.position).normalized * speed * Time.deltaTime);
                 * Velocity se ne frega di qualsiasi altra forza applicata e lo muove verso il player.
                 */

                rb.velocity = (player.transform.position - transform.position).normalized * speed * 100 * Time.deltaTime;
            }
            //Se il nemico è quello ravvicinato...
            else
            {
                //Se la distanza non è abbastanza per sparare continua a muoversi nella direzione del player.
                if (Vector3.Distance(transform.position, player.transform.position) > shootRange)
                {
                    //transform.position = Vector3.Lerp(transform.position, player.transform.position, speed * Time.deltaTime);
                    rb.velocity = (player.transform.position - transform.position).normalized * speed * 50 * Time.deltaTime;
                }
                //Se può sparargli
                else
                {
                    //Si ferma
                    rb.velocity = Vector3.zero;

                    //Spara al player
                    //Shoot();
                }
            }
        }
        //Se il giocatore non è spottato...
        else
        {
            //Torna a fissare il prossimo punto in cui dovrà andare
            //transform.LookAt(nextwaypoint.transform.position);
            //Si muove dove dovrà andare
            //rb.AddForce((player.transform.position - transform.position).normalized * speed * Time.deltaTime);
            rb.velocity = Vector3.zero;
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
        if (Vector3.Distance(transform.position, player.transform.position) <= visionRange)
        {
            //...e si trova nell'angolo di visione del nemico...
            if (Vector3.Angle(transform.forward, directionToPlayer) <= angleRange || Vector3.Distance(transform.position, player.transform.position) /* - rumore prodotto dal player */ <= hearRange)
            {
                //Il nemico ha una visione sul player non bloccata da enviroment
                if(Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hitInfo, visionRange, lineOfSight) && hitInfo.transform.CompareTag("Player"))
                {
                    Debug.DrawRay(transform.position, directionToPlayer, Color.green);
                    return true;
                }
                else
                    Debug.DrawRay(transform.position, directionToPlayer, Color.red);
            }
        }

        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && can_chase)
        {
            //Direzione del knockback
            Vector3 direzione = (collision.transform.position - transform.position).normalized;

            //Knockback
            player.GetComponent<Rigidbody>().AddForce(direzione * knockback, ForceMode.Impulse);
            
            //Stop chase per un pò.
            //enemy_type = Type.Distanza;
            StartCoroutine(StunTime(1));
        }
    }

    IEnumerator StunTime(float time)
    {
        rb.velocity = Vector3.zero;
        can_chase = false;
        //Aspetta per un tot
        yield return new WaitForSeconds(time);
        //Restituisce l'opposto del bool
        can_chase = true;
        rb.velocity = Vector3.zero;
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
