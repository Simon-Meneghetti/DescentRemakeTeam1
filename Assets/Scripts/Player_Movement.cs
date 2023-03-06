using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class Player_Movement : MonoBehaviour
{
    //Componenti giocatore
    private Rigidbody rb;
    private Vector3 mov_direction;
    private Vector2 facing_direction;
    //Se 1 ruota a destra se -1 a sinistra
    private float rotate_direction;
    private Vector3 rotate_target;
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
        //transform.localEulerAngles = new Vector2(facing_direction.x * camera_sensitivity, facing_direction.y * camera_sensitivity) * Time.deltaTime;

        //transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(facing_direction.x * camera_sensitivity, facing_direction.y * camera_sensitivity, rotate_target.z * velocita_rotazione), Time.deltaTime);

        //Se sta tenendo premuto un tasto per la rotazione continuiamo ad aggiornare la rotazione
        if (want_rotate)
        {

        }
        else
        {
            //facing_direction.z = transform.localEulerAngles.z;

            //float z_target = 0;

            /*//0
            if (transform.localEulerAngles.z <= 45 || transform.localEulerAngles.z >= 315)
            {

                facing_direction.z = 0;
            }
            //90
            else if (transform.localEulerAngles.z > 45 && transform.localEulerAngles.z <= 135)
            {
                facing_direction.z = 90;
            }
            //-90
            else if (transform.localEulerAngles.z > 225 && transform.localEulerAngles.z < 315)
            {
                facing_direction.z = -90;
            }
            //180
            else if (transform.localEulerAngles.z > 135 && transform.localEulerAngles.z <= 225)
            {
                facing_direction.z = 180;
            }
            */
        }

    }

    public void Raccolta_Input_Movimento(InputAction.CallbackContext ctx)
    {
        mov_direction = ctx.ReadValue<Vector3>();
    }

    public void Raccolta_Input_Girarsi(InputAction.CallbackContext ctx)
    {
        //0
        if (transform.localEulerAngles.z <= 45 || transform.localEulerAngles.z >= 315)
        {
            facing_direction += new Vector2(-ctx.ReadValue<Vector2>().y, ctx.ReadValue<Vector2>().x);
        }
        //90
        else if (transform.localEulerAngles.z > 45 && transform.localEulerAngles.z <= 135)
        {
            facing_direction += new Vector2(-ctx.ReadValue<Vector2>().x, -ctx.ReadValue<Vector2>().y);
        }
        //-90
        else if (transform.localEulerAngles.z > 225 && transform.localEulerAngles.z < 315)
        {
            facing_direction += new Vector2(ctx.ReadValue<Vector2>().x, ctx.ReadValue<Vector2>().y);
        }
        //180
        else if (transform.localEulerAngles.z > 135 && transform.localEulerAngles.z <= 225)
        {
            facing_direction += new Vector2(ctx.ReadValue<Vector2>().y, -ctx.ReadValue<Vector2>().x);
        }
        
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
