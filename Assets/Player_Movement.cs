using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    //Componenti giocatore
    private Rigidbody rb;
    private Vector3 mov_direction;
    private Vector3 facing_direction;
    private float rotate_direction;
    private bool want_rotate;

    [Header("Speed Settings")]
    [SerializeField, Range(0, 20)] private float velocita_movimento;
    [SerializeField, Range(0, 20)] private float velocita_decelerazione;
    [SerializeField, Range(0, 20)] private float velocita_massima;

    [SerializeField, Range(0, 20)] private float velocita_rotazione;
    [SerializeField, Range(0, 20)] private float camera_sensitivity;

    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movimento_Giocatore();
    }

    public void Movimento_Giocatore()
    {
        //Aggiunge una forza relativa alla direzione che sta guardando che gli permetterà il movimento a seconda del tasto premuto.

        //Se si vuole muovere...
        if (mov_direction != Vector3.zero)
        {
            //Si muove nella direzione stabilita
            rb.AddRelativeForce(mov_direction * velocita_movimento, ForceMode.Force);
        }
        //Se non si vuole muovere...
        else
        {
            //Decelera
            rb.AddForce(-rb.velocity * velocita_decelerazione, ForceMode.Acceleration);
        }

        //Massima velocità
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, velocita_massima);

        //Rotazione telecamera X & Y
        transform.localRotation = Quaternion.Euler(facing_direction * camera_sensitivity * Time.deltaTime);

        //Se sta tenendo premuto un tasto per la rotazione continuiamo ad aggiornare la rotazione
        if (want_rotate)
        {
            //Rotazione telecamera Z
            facing_direction.z += rotate_direction * velocita_rotazione;
        }
        
    }

    public void Raccolta_Input_Movimento(InputAction.CallbackContext ctx)
    {
        mov_direction = ctx.ReadValue<Vector3>();
    }

    public void Raccolta_Input_Girarsi(InputAction.CallbackContext ctx)
    {
        facing_direction += new Vector3(-ctx.ReadValue<Vector2>().y, ctx.ReadValue<Vector2>().x, 0);
    }

    public void Raccolta_Input_Ruotarsi(InputAction.CallbackContext ctx)
    {
        rotate_direction = ctx.ReadValue<float>();

        //Se sta premendo il tasto...
        if (ctx.performed)
        {
            //...vuole sparare
            want_rotate = true;
        }
        //Se ha rilasciato il tasto...
        else if (ctx.canceled)
        {
            //...non vuole più sparare
            want_rotate = false;
        }

    }
}
