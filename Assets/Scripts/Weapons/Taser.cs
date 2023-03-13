using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taser : MonoBehaviour
{
    [HideInInspector] public float timer;
    [HideInInspector] public bool colpisci;
    public Transform player;
    public float radius;
    UIManager UM;
    Muzzle m;

    private void Start()
    {
        UM = FindObjectOfType<UIManager>();
        m = FindObjectOfType<Muzzle>();
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
                Debug.LogWarning("Zot!");
            }

        }
    }
}
