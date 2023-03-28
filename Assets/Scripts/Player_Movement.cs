using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Player_Movement : MonoBehaviour
{
    //Componenti giocatore
    private Rigidbody rb;
    private Vector3 mov_direction;
    private Quaternion facing_directionX;
    private Quaternion facing_directionY;
    private Quaternion facing_directionZ;

    //Se 1 ruota a destra se -1 a sinistra
    private bool want_rotate = false;
    private bool want_dash = false;

    [Header("Speed Settings")]
    [SerializeField, Range(0, 100)] private float velocita_movimento;
    private float velocita_movimento_appoggio;
    [SerializeField, Range(0, 100)] private float velocita_boost;
    [SerializeField, Range(0, 100)] private float velocita_decelerazione;
    [Range(0, 100)] public float velocita_massima;

    [SerializeField, Range(0, 100)] private float velocita_rotazione;
    [SerializeField, Range(0, 100)] private float camera_sensitivity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;

        facing_directionZ = Quaternion.identity;

        velocita_movimento_appoggio = velocita_movimento;
    }

    // Update is called once per frame
    void Update()
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

        //Ruotare sull'asse Z
        if (want_rotate)
        { 
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }

        //Ruotare sull'asse X e Y
        Quaternion rotation_target = facing_directionX * facing_directionY * facing_directionZ;

        //Applicazione rotazione
        transform.localRotation *= rotation_target;

        //Sta dashando?
        if (mov_direction.z > 0 && want_dash)
        {
            velocita_movimento = velocita_boost;
        }
        else if (mov_direction.z <= 0 || !want_dash)
        {
            velocita_movimento = velocita_movimento_appoggio;
            want_dash = false;
        }
    }

    public void ChangeSensibility(Slider slider)
    {
        camera_sensitivity = slider.value;
    }

    public void Raccolta_Input_Movimento(InputAction.CallbackContext ctx)
    {
        mov_direction = ctx.ReadValue<Vector3>();
    }

    public void Raccolta_Input_Girarsi(InputAction.CallbackContext ctx)
    {
        facing_directionX = Quaternion.AngleAxis(-ctx.ReadValue<Vector2>().y * camera_sensitivity * Time.deltaTime, Vector3.right);

        facing_directionY = Quaternion.AngleAxis(ctx.ReadValue<Vector2>().x * camera_sensitivity * Time.deltaTime, Vector3.up);
    }

    public void Raccolta_Input_Ruotarsi(InputAction.CallbackContext ctx)
    {
        
        facing_directionZ = Quaternion.AngleAxis(ctx.ReadValue<float>() * velocita_rotazione * Time.deltaTime, Vector3.forward);

        //Se sta premendo il tasto...
        if (ctx.performed)
        {
            want_rotate = true;
        }
        //Se non lo sta piu' premendo...
        else if (ctx.canceled)
        {
            want_rotate = false;
        }
    }

    public void Raccolta_Input_Dash(InputAction.CallbackContext ctx) 
    {
        if (ctx.performed)
        {
            if(want_dash)
                want_dash = false;
            else
                want_dash = true;
        }
    }
}
