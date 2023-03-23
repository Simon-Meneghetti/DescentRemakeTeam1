using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //Enum 
    public enum TipologiaMenu { Menu_Principale, Menu_Pausa, Menu_Finale };
    [Header("Tipo Menu")]
    public TipologiaMenu TipoMenu;

    [Header("Menu principale")]
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button SettingsButton;
    [SerializeField] private Button QuitButton;
    [SerializeField] private Image HelpSelectable;
    private float HelpSelectableNextPosizion;
    [SerializeField] private Text Descrizione;
    [SerializeField] private string DescrizionePlay;
    [SerializeField] private string DescrizioneSettings;
    [SerializeField] private string DescrizioneQuit;
    private GameObject currentSelected;

    [Header("In Game UI")]
    public GameObject TaserPanel;
    public Image StaminaBar;
    public Image StaminaBar1;
    public Image O2Bar;
    public Image ShieldBar;
    public Text Counter;

    Muzzle m;
    PlayerStats pS;

    void Start()
    {
        //Se riesce a trovare quello che gli serve allora non è nel menu principale
        if (TipoMenu != TipologiaMenu.Menu_Principale)
        {
            m = FindObjectOfType<Muzzle>();
            pS = FindObjectOfType<PlayerStats>();
        }
        else
        {
            PlayButton.Select();
        }
    }


    void Update()
    {
        //Se sono state trovate le stats del player (quindi non siamo nel menu principale)...
        if (TipoMenu != TipologiaMenu.Menu_Principale)
        {
            StaminaBar.fillAmount = m.stamina / m.maxStamina;
            StaminaBar1.fillAmount = m.stamina / m.maxStamina;
            O2Bar.fillAmount = pS.oxigen / pS.maxO2;
            ShieldBar.fillAmount = pS.shield / pS.maxShield;
            Counter.text = m.satchelCounter.ToString("00");
        }
        else
        {
            AnimazioniMenuPrincipale();
        }
    }

    void AnimazioniMenuPrincipale()
    {
        if (EventSystem.current.currentSelectedGameObject)
            HelpSelectableNextPosizion = EventSystem.current.currentSelectedGameObject.transform.position.y;

        HelpSelectable.transform.DOMoveY(HelpSelectableNextPosizion, 0.1f, true);

        if (EventSystem.current.currentSelectedGameObject == PlayButton.gameObject && currentSelected != EventSystem.current.currentSelectedGameObject)
        {
            Descrizione.DOText(DescrizionePlay.ToUpper(), 0.15f, true, ScrambleMode.All);
        }
        else if (EventSystem.current.currentSelectedGameObject == SettingsButton.gameObject && currentSelected != EventSystem.current.currentSelectedGameObject)
        {
            Descrizione.DOText(DescrizioneSettings.ToUpper(), 0.15f, true, ScrambleMode.All);
        }
        else if (EventSystem.current.currentSelectedGameObject == QuitButton.gameObject && currentSelected != EventSystem.current.currentSelectedGameObject)
        {
            Descrizione.DOText(DescrizioneQuit.ToUpper(), 0.15f, true, ScrambleMode.All);
        }

        currentSelected = EventSystem.current.currentSelectedGameObject;
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(0);
    }
}
