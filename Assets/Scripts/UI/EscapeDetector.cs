using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeDetector : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        //Se contiene lo script del movimento del giocatore allora è il player! (evito di usare i tag)
        if (other.GetComponent<Player_Movement>())
        {
            //Facciamo partire la funzione per attivare la UI della vittoria
            UIManager.instance.GameWon();
        }    
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().size);
    }
}
