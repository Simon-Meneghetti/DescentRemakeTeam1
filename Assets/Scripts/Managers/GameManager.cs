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
    void OnEnable()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void AttivaPlayer()
    {
        GameObject.FindObjectOfType<PlayerInput>().enabled = true;
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
