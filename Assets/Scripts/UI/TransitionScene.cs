using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScene : MonoBehaviour
{
    public void LoadScene(int indexScene)
    {
        SceneManager.LoadScene(indexScene);
    }

    public void Seppuku()
    {
        gameObject.SetActive(false);
    }

    public void StartGame()
    {
        if(GameObject.FindObjectOfType<GameManager>())
            GameManager.instance.AttivaPlayer();
    }
}
