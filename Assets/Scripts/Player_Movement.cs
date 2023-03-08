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
    private Quaternion rotate_target;
    private bool rotating = false;

    [Header("Speed Settings")]
    [SerializeField, Range(0, 20)] private float velocita_movimento;
    [SerializeField, Range(0, 20)] private float velocita_decelerazione;
    [SerializeField, Range(0, 20)] private float velocita_massima;

    [SerializeField, Range(0, 20)] private float durata_rotazione;
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

        if(!rotating)
            //Rotazione telecamera X & Y
            transform.localEulerAngles = new Vector3(facing_direction.x * Time.deltaTime * camera_sensitivity, facing_direction.y * Time.deltaTime * camera_sensitivity, transform.localEulerAngles.z) ;

        //transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(facing_direction.x * camera_sensitivity, facing_direction.y * camera_sensitivity, rotate_target.z * velocita_rotazione), Time.deltaTime);

        //Se sta tenendo premuto un tasto per la rotazione continuiamo ad aggiornare la rotazione

        //transform.localRotation = Quaternion.Lerp(transform.localRotation, rotate_target, velocita_rotazione * Time.deltaTime);
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
        //Debug.Log(rotate_target);
        //Debug.Log(rotating);
        if (rotate_direction != 0 && !rotating)
        {
            rotate_target = Quaternion.AngleAxis(transform.localEulerAngles.z + (rotate_direction * 90), Vector3.forward);

            rotating = true;

            StartCoroutine(Rotating());
        }
    }

    //Coroutine chiamata quando viene deciso in che direzione muoversi sull'asse delle Z
    IEnumerator Rotating()
    {
        Debug.Log(durata_rotazione);
        for (float t = 0.0f; t < durata_rotazione; t += Time.deltaTime)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, rotate_target, t / durata_rotazione);

            yield return new WaitForEndOfFrame();

            //Debug.Log("passa");
        }

        transform.localRotation = rotate_target;
        //Debug.Log("can move");

        rotating = false;
    }
}
