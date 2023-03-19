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
    [HideInInspector] public bool colpisci;
    public Transform player;
    public float radius;
    [SerializeField] public TypeOfObject type;
    UIManager UM;
    Muzzle m;
    Nemico n;
    OpenDoor oD;

    private void Start()
    {
        UM = FindObjectOfType<UIManager>();
        m = FindObjectOfType<Muzzle>();
        n = FindObjectOfType<Nemico>();
        oD = FindObjectOfType<OpenDoor>();
    }
    void Update()
    {
        PlayerDistance();
        if (colpisci == true)
        {
            timer += Time.deltaTime;

            if (timer <= 1)
                UM.TaserPanel.SetActive(true);

            else
            {
                UM.TaserPanel.SetActive(false);
                timer = 0;
                colpisci = false;
                m.spawnTimer = 0;
            }

        }
    }
    void PlayerDistance()
    {
        if (player && m.taser == true)
        {
            colpisci = true;
            float d = Vector3.Distance(player.position, transform.position);
            //Debug.Log("Distance: " + d);
            if (d <= radius)
            {
                if (n != null && type == TypeOfObject.Enemy)
                {
                    StartCoroutine(n.StunTime(2));
                    Debug.LogWarning("Zot!");
                }
                else if(oD!=null && type == TypeOfObject.FuseBox)
                {
                    oD.Opening();
                }
            }
        }
    }
}
