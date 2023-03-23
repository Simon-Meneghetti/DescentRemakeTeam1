using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Raccolta_Input_Pausa(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if(Time.timeScale != 0)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }
    }
}
