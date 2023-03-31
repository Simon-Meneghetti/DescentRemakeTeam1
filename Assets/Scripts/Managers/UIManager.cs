using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

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
    [SerializeField] private Image HelpSelectableMenuFinale;
    private Vector3 HelpSelectableNextPosition;

    [Header("Descrizione")]
    //Testi
    [SerializeField] private Text Descrizione;
    [SerializeField] private Text MessaggioFinale;
    [SerializeField] private string ScrittaVittoria;
    [SerializeField] private string ScrittaSconfitta;

    //////////////////////////////////////////////

    [Header("Menu pausa")]
    public GameObject Menu_pausa;

    //////////////////////////////////////////////

    [Header("Schermata Caricamento")]
    [SerializeField] private Animator Caricamento;

    //////////////////////////////////////////////
    [Header("In Game UI")]
    

    public GameObject Menu_game_UI;
    //public GameObject TaserPanel;
    public GameObject TaserPronto;
    public GameObject SatchelPronta;
    public GameObject ArpionePronto;

    public Image StaminaBar;
    public Image O2Bar;
    public Image ShieldBar;
    public Image velocityBar;
    public Text Counter;
    //public float maxSpeed = 0.0f;
    //public float currentSpeed = 0.0f;
    //public Rigidbody target;
    Muzzle m;
    PlayerStats pS;
    Player_Movement pM;
    ///////////////////////////////////////////////

    public static UIManager instance;

   
    void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    ///////////////////////////////////////////////

    void Start()
    {
        //Se riesce a trovare quello che gli serve allora non è nel menu principale
        if (TipoMenu != TipologiaMenu.Menu_Principale)
        {
            m = FindObjectOfType<Muzzle>();
            pS = FindObjectOfType<PlayerStats>();
            pM = FindObjectOfType<Player_Movement>();
        }

        Caricamento.Play("LoadOut");
    }

    void Update()
    {
        //Se sono state trovate le stats del player (quindi non siamo nel menu principale)...
        if (TipoMenu != TipologiaMenu.Menu_Principale)
        {
            StaminaBar.fillAmount = m.stamina / m.maxStamina;
            O2Bar.fillAmount = pS.oxigen / pS.maxO2;
            ShieldBar.fillAmount = pS.shield / pS.maxShield;
            Counter.text = m.satchelCounter.ToString("00");
            //currentSpeed = target.velocity.magnitude * 3.6f;
            //velocityBar.fillAmount = currentSpeed / maxSpeed;
            velocityBar.fillAmount = pM.dashCounter / pM.maxDashCounter;
        }

        if (HelpSelectableMenuFinale != null && HelpSelectableMenuFinale.IsActive())
            HelpSelectable = HelpSelectableMenuFinale;
    }

    //Esce dal gioco
    public void Quit()
    {
        Application.Quit();
    }

    //GameLost
    public void GameLost()
    {
        MessaggioFinale.text = ScrittaSconfitta.ToUpper();
        MessaggioFinale.transform.parent.gameObject.SetActive(true);
    }

    //GameWon
    public void GameWon()
    {
        MessaggioFinale.text = ScrittaVittoria.ToUpper();
        MessaggioFinale.transform.parent.gameObject.SetActive(true);

    }

    //Richiamato per cambiare la descrizione quando si trova su un'opzione diversa.
    public void ChangeDescription(string newText)
    {
        //Animazione per scrivere
        Descrizione.DOText(newText.ToUpper(), 0.15f, true, ScrambleMode.All).SetUpdate(true);

        //Se c'è un oggetto selezionato...
        if (EventSystem.current.currentSelectedGameObject)
        {
            //La sua Y sarà la destinazione del nostro HelpSelectable
            var transformSelezionato = EventSystem.current.currentSelectedGameObject.transform;

            HelpSelectableNextPosition = new Vector2(transformSelezionato.position.x - transformSelezionato.GetComponent<RectTransform>().rect.width / 1.8f, transformSelezionato.position.y);
        }

        //Se è attivo il selectable delle impostazioni allora significa che è nelle impostazioni e muoviamo quello
        if (HelpSelectableSettings == null || !HelpSelectableSettings.IsActive())
            HelpSelectable.transform.DOMove(HelpSelectableNextPosition, 0.1f, true).SetUpdate(true);
        //Sennò muoviamo quello del menu generale.
        else
            HelpSelectableSettings.transform.DOMoveY(HelpSelectableNextPosition.y, 0.1f, true).SetUpdate(true);
    }
    
}
