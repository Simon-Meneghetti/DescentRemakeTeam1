using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TypeOfObject
{
    FuseBox,
    Enemy
}

public class Taser : MonoBehaviour
{
    [HideInInspector] public float timer;
    [HideInInspector] public bool electricShock;
    public GameObject FuseBoxScript;
    public Transform player;
    public float radius;
    [SerializeField] public TypeOfObject type;
    UIManager UM;
    Muzzle m;
    Nemico n;
    FuseBox fB;

    private void Start()
    {
        UM = FindObjectOfType<UIManager>();
        m = FindObjectOfType<Muzzle>();
        n = FindObjectOfType<Nemico>();
        fB = FindObjectOfType<FuseBox>();
    }
    void Update()
    {
        //se utilizzo il taser (electricShock = true) scatta un timer durante il quale la UI del taser si attiva, dopodichè si disattiva
        PlayerDistance();
        if (electricShock == true)
        {
            timer += Time.deltaTime;

            if (timer <= 1)
                UM.TaserPanel.SetActive(true);

            else
            {
                UM.TaserPanel.SetActive(false);
                timer = 0;
                electricShock = false;
                m.spawnTimer = 0;
            }

        }
    }

    /*
    se utilizzo il taser la scarica elettrica viene rilasciata e viene calcolata la distanza tra l'oggetto in questione e il taser del player
    se la distanza è minore del raggio d'azione del taser e l'oggetto in questione è un enemy, verrà stunnato per 2 secondi,
    se l'oggetto in questione è una fusebox, le porte ad essa collegate si apriranno
     */
    void PlayerDistance()
    {
        if (player && m.taser == true)
        {
            electricShock = true;
            float d = Vector3.Distance(player.position, transform.position);
            if (d <= radius)
            {
                if (n != null && type == TypeOfObject.Enemy)
                {
                    StartCoroutine(n.StunTime(2));
                }
                else if (type == TypeOfObject.FuseBox)
                {
                    FuseBoxScript.SetActive(true);
                }
            }
        }
    }
}
