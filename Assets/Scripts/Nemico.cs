using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using DG.Tweening;
using UnityEditorInternal;
using UnityEngine;

public class Nemico : MonoBehaviour
{
    private GameObject player;

    [Header("Stealth settings")]
    //La distanza del suo cono visivo
    public float visionRange;
    //L'apertura del suo cono visivo
    public float angleRange;
    //La distanza con cui può sentire il giocatore.
    public float hearRange;

    ///////////////////////////////////// 

    //Percorso
    [Header("Percorso")]
    public List<Transform> defaultPath;
    [SerializeField] private List<Vector3> comeBackPath;

    private int index = 0;
    private int indexApp;

    private bool contrario;
    private bool coroutineRunning;

    [Header("Cosa può istruire la visione?")]
    //Maschera del raycast.
    public LayerMask lineOfSight;

    [Header("Stats")]
    public float speed;
    public float damage;
    public float knockback;


    //Componenti nemico che ci servono
    private Rigidbody rb;
    private bool can_move; 


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<Player_Movement>().gameObject;
        rb = GetComponent<Rigidbody>();
        can_move = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(can_move)
            EnemyMovement(Detection());

        Debug.Log(Detection());
    }

    void EnemyMovement(bool player_spotted)
    {
        //Se il giocatore è spottato...
        if (player_spotted)
        {
            //Fissa il player in modo inquietante
            transform.LookAt(player.transform.position);


            if(!coroutineRunning)
                StartCoroutine(CreateComeBackPath());
            

            /* Varie opzioni di movimento 
             * Se uso la posizione trapassa i muri
                //transform.position = Vector3.Lerp(transform.position, player.transform.position, speed * Time.deltaTime);
             * Se uso addforce non riesce a raggiungere il player se si muove al suo lato, o almeno lo raggiunge tramite una spirale
                //rb.AddForce((player.transform.position - transform.position).normalized * speed * Time.deltaTime);
             * Velocity se ne frega di qualsiasi altra forza applicata e lo muove verso il player.
             */

            rb.velocity = (player.transform.position - transform.position).normalized * speed * 100 * Time.deltaTime;
        }
        //Se il giocatore non è spottato...
        else
        {
            coroutineRunning = false;
            //Torna a fissare il prossimo punto in cui dovrà andare
            //Si muove dove dovrà andare
            //rb.AddForce((player.transform.position - transform.position).normalized * speed * Time.deltaTime);
            rb.velocity = Vector3.zero;

            index = indexApp;

            //Se il comeBackPath è vuoto...
            if (comeBackPath.Count <= 0)
            {
                //Guarda la direzione in cui deve andare
                transform.LookAt(defaultPath[index]);
                //Il nemico si muove verso il prossimo patrol point.
                transform.position = Vector3.MoveTowards(transform.position, defaultPath[index].transform.position, speed * Time.deltaTime);

                //Se è arrivato...
                if (Vector3.Distance(transform.position, defaultPath[index].transform.position) < 0.2)
                {
                    //Sta seguendo il percorso normalmente?
                    if (index < defaultPath.Count - 1 && !contrario)
                    {
                        index++;
                    }
                    //O al contrario?
                    else if (index > 0 && contrario)
                    {
                        index--;
                    }
                    //Se ha raggiunto la fine del percorso lo percorrerà al contrario.
                    else if (index >= defaultPath.Count - 1 || index <= 0)
                    {
                        StartCoroutine(StunTime(1.5f));
                        contrario = !contrario;
                    }
                }
                indexApp = index;
            }
            else
            {
                //Partiamo dall'ultima posizione creata
                index = comeBackPath.Count - 1;

                //Il nemico si muove verso il prossimo patrol point.
                transform.position = Vector3.MoveTowards(transform.position, comeBackPath[index], speed * Time.deltaTime);

                //Se è arrivato...
                if (Vector3.Distance(transform.position, comeBackPath[index]) < 0.2)
                {
                    //O al contrario?
                    if (index >= 0)
                    {
                        comeBackPath.RemoveAt(index);
                        if (index != 0)
                        {
                            index--;
                            //Guarda la direzione in cui deve andare
                            transform.LookAt(comeBackPath[index]);
                        }
                    }
                    //Se ha raggiunto la fine del percorso lo percorrerà al contrario.
                    else
                    {

                    }

                }
            }
        }
    }

    IEnumerator CreateComeBackPath()
    {
        //La coroutine sta andando!
        coroutineRunning = true;

        while (coroutineRunning)
        {
            //Ogni 2 secondi 
            yield return new WaitForSeconds(1);
            //Aggiunge un punto per tornare indietro
            comeBackPath.Add(transform.position);
        }

        StopCoroutine(CreateComeBackPath());
    }

    //Funzione che serve al nemico per trovare il giocatore.
    bool Detection()
    {
        //Il vettore che ci da la direzione da questo nemico al player.
        Vector3 directionToPlayer = player.transform.position - transform.position;

        //Velocità del giocatore.
        var playerCurrentSpeed = player.GetComponent<Rigidbody>().velocity.magnitude;

        var calcoloRumore = ((playerCurrentSpeed / 1.8f) / Vector3.Distance(transform.position, player.transform.position) * hearRange * 2);

        //Debug.Log(calcoloRumore);

        //Se il giocatore non è troppo distante...
        if (Vector3.Distance(transform.position, player.transform.position) <= visionRange
            || calcoloRumore >= hearRange || Vector3.Distance(transform.position, player.transform.position) <= hearRange / 2)
        {


            //...e si trova nell'angolo di visione del nemico...
            if (Vector3.Angle(transform.forward, directionToPlayer) <= angleRange || calcoloRumore >= hearRange || Vector3.Distance(transform.position, player.transform.position) <= hearRange / 2)
            {
                //Il nemico ha una visione sul player non bloccata da enviroment
                if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hitInfo, hearRange, lineOfSight) && hitInfo.transform.CompareTag("Player"))
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
        if (collision.transform.CompareTag("Player") && can_move)
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

    public IEnumerator StunTime(float time)
    {
        rb.velocity = Vector3.zero;
        can_move = false;
        //Aspetta per un tot
        yield return new WaitForSeconds(time);
        //Restituisce l'opposto del bool
        can_move = true;
        rb.velocity = Vector3.zero;
    }

    void OnDrawGizmosSelected()
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

        for(int i = 0; i < defaultPath.Count - 1; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(defaultPath[i].position, defaultPath[i + 1].position - defaultPath[i].position);
        }

        //Direzione = B - A
        //A = B - Direzione
    }
}
