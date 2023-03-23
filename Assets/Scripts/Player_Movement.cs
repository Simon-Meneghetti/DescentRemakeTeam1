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
    private Quaternion facing_directionX;
    private Quaternion facing_directionY;
    private Quaternion facing_directionZ;

    //Se 1 ruota a destra se -1 a sinistra
    private bool want_rotate = false;

    [Header("Speed Settings")]
    [SerializeField, Range(0, 100)] private float velocita_movimento;
    [SerializeField, Range(0, 100)] private float velocita_decelerazione;
    [SerializeField, Range(0, 100)] private float velocita_massima;

    [SerializeField, Range(0, 100)] private float velocita_rotazione;
    [SerializeField, Range(0, 100)] private float camera_sensitivity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;

        facing_directionZ = Quaternion.identity;
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
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }

        //Ruotare sull'asse X e Y
        Quaternion rotation_target = facing_directionX * facing_directionY * facing_directionZ;

        transform.localRotation *= rotation_target;
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
}
