using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static Action onPause;

    public static GameManager instance;

    private bool justOnce;
    // Start is called before the first frame update
    void OnEnable()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Raccolta_Input_Pausa(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
        {
            if (justOnce)
            {
                justOnce = false;
            }
            else
            {
                Time.timeScale = 0;
                FindObjectOfType<UIManager>().Menu_game_UI.gameObject.SetActive(false);
                FindObjectOfType<UIManager>().Menu_pausa.gameObject.SetActive(true);
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;

        justOnce = true;

        FindObjectOfType<UIManager>().Menu_game_UI.gameObject.SetActive(true);
        FindObjectOfType<UIManager>().Menu_pausa.gameObject.SetActive(false);
    }    
}
