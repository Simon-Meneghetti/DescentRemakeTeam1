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
    public enum TipologiaMenu { Menu_Principale, Menu_InGame };

    [Header("Tipo Menu")]
    public TipologiaMenu TipoMenu;

    [Header("Bottoni")]
    [Header("Menu principale")]
    //Bottoni
    [SerializeField] private Button StarterButton;

    [Header("Selezionabili")]
    //Selezionabile
    [SerializeField] private Image HelpSelectable;
    [SerializeField] private Image HelpSelectableSettings;
    private float HelpSelectableNextPosizion;

    [Header("Descrizione")]
    //Testi
    [SerializeField] private Text Descrizione;


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
        //Se riesce a trovare quello che gli serve allora non � nel menu principale
        if (TipoMenu != TipologiaMenu.Menu_Principale)
        {
            m = FindObjectOfType<Muzzle>();
            pS = FindObjectOfType<PlayerStats>();
        }
        else
        {
            StarterButton.Select();
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
        //Se c'� un oggetto selezionato...
        if (EventSystem.current.currentSelectedGameObject)
            //La sua Y sar� la destinazione del nostro HelpSelectable
            HelpSelectableNextPosizion = EventSystem.current.currentSelectedGameObject.transform.position.y;

        //Se � attivo il selectable delle impostazioni allora significa che � nelle impostazioni e muoviamo quello
        /*if(!HelpSelectableSettings.IsActive())
            HelpSelectable.transform.DOMoveY(HelpSelectableNextPosizion, 0.1f, true);
        //Senn� muoviamo quello del menu generale.
        else
            HelpSelectableSettings.transform.DOMoveY(HelpSelectableNextPosizion, 0.1f, true);
        */
    }

    //Carica il gioco
    public void LoadGame()
    {
        SceneManager.LoadScene(0);
    }

    //Esce dal gioco
    public void Quit()
    {
        Application.Quit();
    }

    //Richiamato per cambiare la descrizione quando si trova su un'opzione diversa.
    public void ChangeDescription(string newText)
    {
        Descrizione.DOText(newText.ToUpper(), 0.15f, true, ScrambleMode.All);
    }
}
